using FellowOakDicom;
using FellowOakDicom.Network;
using DICOM_Router.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FellowOakDicom.Log;
using static Microsoft.Extensions.Logging.ILogger;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/*
**********************************************************************************************************
**********************************************************************************************************
*                            
*               
*               DICOM C-STORE SCP Implementation using fo-dicom library
*               
*               The Service is started by the StartScpService class
*               which is a BackgroundService that runs in the background and started in app.xaml.cs
*               
*               Documentation and example that was used to create this can be found at:
*               https://github.com/fo-dicom/fo-dicom-samples/blob/master/Core/C-Store%20SCP/StoreScp.cs
*               
**********************************************************************************************************
**********************************************************************************************************
*/

// TODO: Implement logging and error handling for the DICOM C-STORE SCP service.

namespace DICOM_Router.Core
{
    // The ScpReceiveService class implements the DICOM C-STORE SCP service.
    public class ScpReceiveService :
          DicomService,
          IDicomServiceProvider,
          IDicomCStoreProvider,
          IDicomCEchoProvider
    {
        public ScpReceiveService(
            INetworkStream stream,
            Encoding fallbackEncoding,
            Microsoft.Extensions.Logging.ILogger logger,
            DicomServiceDependencies dependencies)
            : base(stream, fallbackEncoding, logger, dependencies)
        {
        }

        // The Proposed Accepted Transfer Syntaxes for the Association.
        private static readonly DicomTransferSyntax[] _acceptedTransferSyntaxes = new DicomTransferSyntax[]
        {
               DicomTransferSyntax.ExplicitVRLittleEndian,
               DicomTransferSyntax.ExplicitVRBigEndian,
               DicomTransferSyntax.ImplicitVRLittleEndian
        };

        // The Proposed Accepted Transfer Syntaxes for the C-STORE Requests.
        private static readonly DicomTransferSyntax[] _acceptedImageTransferSyntaxes = new DicomTransferSyntax[]
        {
               // Lossless
               DicomTransferSyntax.JPEGLSLossless,
               DicomTransferSyntax.JPEG2000Lossless,
               DicomTransferSyntax.JPEGProcess14SV1,
               DicomTransferSyntax.JPEGProcess14,
               DicomTransferSyntax.RLELossless,
               // Lossy
               DicomTransferSyntax.JPEGLSNearLossless,
               DicomTransferSyntax.JPEG2000Lossy,
               DicomTransferSyntax.JPEGProcess1,
               DicomTransferSyntax.JPEGProcess2_4,
               // Uncompressed
               DicomTransferSyntax.ExplicitVRLittleEndian,
               DicomTransferSyntax.ExplicitVRBigEndian,
               DicomTransferSyntax.ImplicitVRLittleEndian
        };

        // Handle incoming DICOM association requests.
        public Task OnReceiveAssociationRequestAsync(
            DicomAssociation association)
        {
            Debug.WriteLine($"Association request from {association.CallingAE}");

            if (association.CalledAE != "STORESCP")
            {
                Debug.WriteLine(
                    $"Association rejected: Called AE {association.CalledAE} not recognized"); // TODO: Log this.
                return SendAssociationRejectAsync(
                    DicomRejectResult.Permanent,
                    DicomRejectSource.ServiceUser,
                    DicomRejectReason.CalledAENotRecognized);
            }

            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification)
                {
                    pc.AcceptTransferSyntaxes(_acceptedTransferSyntaxes);
                }
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None)
                {
                    pc.AcceptTransferSyntaxes(_acceptedImageTransferSyntaxes);
                }
            }

            return SendAssociationAcceptAsync(association);
        }

        // Handle incoming DICOM association release requests.
        public async Task OnReceiveAssociationReleaseRequestAsync()
        {
            await SendAssociationReleaseResponseAsync();
        }

        // Handle incoming DICOM association aborts.
        public void OnReceiveAbort(
            DicomAbortSource source,
            DicomAbortReason reason)
        {
            Debug.WriteLine(
                $"DICOM association aborted: {source}, {reason}");
        }

        // Handle incoming DICOM association closure.
        public void OnConnectionClosed(Exception? exception)
        {
            if (exception != null)
            {
                Debug.WriteLine(
                    $"DICOM connection closed: {exception.Message}");
            }
        }

        // Handle incoming DICOM C-STORE requests.
        public async Task<DicomCStoreResponse> OnCStoreRequestAsync(
            DicomCStoreRequest request)
        {
            try
            {
                string incomingFolder =
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "Incoming");

                Directory.CreateDirectory(incomingFolder);


                // Store the DICOM file in the Incoming folder with the SOPInstanceUID as the filename.
                string sopInstanceUid =
                    request.SOPInstanceUID.UID;

                string filePath =
                    Path.Combine(
                        incomingFolder,
                        $"{sopInstanceUid}.dcm");

                /* Get DICOM file SUID and check if it already exists in Incoming_Studies Table.
                   If it does, increment Image_Count and proceed. Otherwise, 
                   add a new entry to the Incoming_Studies Table with Image_Count = 1.
                */

                // Have to convert to DicomFile because request type has no access to Suid.
                DicomFile dicomFile = request.File;
                string studyInstanceUid = dicomFile.Dataset.GetSingleValueOrDefault(DicomTag.StudyInstanceUID, "UNKNOWN");
                Debug.WriteLine($"We have received: {studyInstanceUid}"); // TODO: Covert to Logging.

                // Calling helper method to check if the study exists in the database.
                bool studyExists = await CheckIfStudyExists(studyInstanceUid);
                if (studyExists)
                {
                    // If it is already in DB, just increase the ImageCount of the row.
                    await IncrementImageCountDB(studyInstanceUid);

                    Debug.WriteLine($"Study exists in DB: {studyInstanceUid}");
                }
                else
                {
                    // If Study not in DB, then we need to add to DB.
                    await WriteDicomIntoDB(dicomFile);

                    Debug.WriteLine($"Study does not exist in DB: {studyInstanceUid}");
                }



                // Save the DICOM file to the Incoming Directory.
                if (request.File != null)
                {
                    await request.File.SaveAsync(filePath);
                }

                Debug.WriteLine(
                    $"Received DICOM: {filePath}");

                // Return a success response for the C-STORE request.
                return new DicomCStoreResponse(
                    request,
                    DicomStatus.Success);
            }
            catch (Exception e)
            {
                // Something bad happened. Log the error and return a failure response.
                Debug.WriteLine(
                    $"Error storing DICOM: {e}");

                return new DicomCStoreResponse(
                    request,
                    DicomStatus.ProcessingFailure);
            }
        }

        // Handle exceptions during DICOM C-STORE requests.
        public async Task OnCStoreRequestExceptionAsync(
            string tempFileName,
            Exception e)
        {
            Debug.WriteLine(
                $"C-STORE exception: {e}"); // TODO: Log this.

            await Task.CompletedTask;
        }

        // Handle incoming DICOM C-ECHO requests.
        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }

        // Helper Method: DB Check if SUID exists in Incoming Studies Table.
        public async Task<bool> CheckIfStudyExists(string studyInstanceUid)
        {
            // TODO: Change to using statement.
            DatabaseContext databaseContext = new DatabaseContext();

            var study = await databaseContext.IncomingStudies.FirstOrDefaultAsync(s => s.Suid == studyInstanceUid);  

            return study != null;
        }

        // Helper Method to Write Dicom attributes to Incoming DB.
        public async Task<bool> WriteDicomIntoDB(DicomFile dicomFile)
        {
            string dateString = DateTime.Now.ToString("yyyyMMdd");
            bool status;

            using (var databaseContext = new DatabaseContext())
            {
                var study = new IncomingStudy
                {
                    DateRec = dateString,
                    PatientId = dicomFile.Dataset.GetString(DicomTag.PatientID),
                    PatientName = dicomFile.Dataset.GetString(DicomTag.PatientName),
                    ModalityType = dicomFile.Dataset.GetString(DicomTag.Modality),
                    ImageCount = 1,
                    Suid = dicomFile.Dataset.GetString(DicomTag.StudyInstanceUID)
                };

                databaseContext.IncomingStudies.Add(study);

                try
                {
                    await databaseContext.SaveChangesAsync();
                    status = true;
                }
                catch(DbUpdateException e)
                {
                    Debug.WriteLine(e);
                    status = false;
                }
                
            }

            return status;
        }

        // Helper Method to increment the row's Image Count if the SUID is already there. 
        public async Task<bool> IncrementImageCountDB(string studyInstanceUid)
        {
            bool status;
            try
            {
                using (var databaseContext = new DatabaseContext())
                {
                    databaseContext.IncomingStudies
                        .Where(e => e.Suid == studyInstanceUid)
                        .ExecuteUpdate(setters => setters
                            .SetProperty(e => e.ImageCount, e => e.ImageCount + 1));
                }
                status = true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                status = false;
            }

            return status;
        }


    }
}

       