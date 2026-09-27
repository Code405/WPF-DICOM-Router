using FellowOakDicom;
using FellowOakDicom.Network;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FellowOakDicom.Log;
using static Microsoft.Extensions.Logging.ILogger;

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
                    $"Association rejected: Called AE {association.CalledAE} not recognized");
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
                        "Incoming"); // TODO: Update this to pull from DB.

                Directory.CreateDirectory(incomingFolder);

                //          Standard way of storage structure for DICOM files:
                //          /Incoming/StudyInstanceUID/SeriesInstanceUID/SOPInstanceUID.dcm
                string studyUid =
                    request.Dataset.GetSingleValue<string>(
                        DicomTag.StudyInstanceUID);

                string seriesUid =
                    request.Dataset.GetSingleValue<string>(
                        DicomTag.SeriesInstanceUID);

                string sopInstanceUid =
                    request.SOPInstanceUID.UID;

                string studyFolder =
                    Path.Combine(incomingFolder, studyUid);

                string seriesFolder =
                    Path.Combine(studyFolder, seriesUid);

                Directory.CreateDirectory(seriesFolder);

                string filePath =
                    Path.Combine(
                        seriesFolder,
                        $"{sopInstanceUid}.dcm");

                // Save the DICOM file to the specified path.
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
            catch (Exception ex)
            {
                // Something bad happened. Log the error and return a failure response.
                Debug.WriteLine(
                    $"Error storing DICOM: {ex}");

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
                $"C-STORE exception: {e}");

            await Task.CompletedTask;
        }

        // Handle incoming DICOM C-ECHO requests.
        public Task<DicomCEchoResponse> OnCEchoRequestAsync(DicomCEchoRequest request)
        {
            return Task.FromResult(new DicomCEchoResponse(request, DicomStatus.Success));
        }
    }
}

       