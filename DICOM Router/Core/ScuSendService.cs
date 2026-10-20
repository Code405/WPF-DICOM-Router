using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
using DICOM_Router.Models;
using FellowOakDicom;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;
using Microsoft.Extensions.Hosting;

namespace DICOM_Router.Core
{
    /*
**********************************************************************************************************
**********************************************************************************************************
*                            
*               
*               DICOM C-STORE SCU Client Implementation using fo-dicom library
*               
*               
*               Documentation and example that was used to create this can be found at:
*               https://fo-dicom.github.io/dev/v4/api/Dicom.Network.DicomCStoreRequest.html
*               
**********************************************************************************************************
**********************************************************************************************************
*/
    public class ScuSendService
    {
        private string Host { get; set; }
        private int Port { get; set; }
        private bool UseTls { get; set; } = false;
        private string CallingAET { get; set; }
        private string CalledAET { get; set; }  
   
        public ScuSendService(Device device)
        {
            this.Host = device.IpAddress;
            this.Port = device.Port;
            this.CallingAET = device.CallingAe;
            this.CalledAET = device.CalledAe;
        }

        public async Task Send(DicomFile dicomFile, int recordID)
        {
            // Create a C-STORE request for the DICOM file
            var client = DicomClientFactory.Create(this.Host, this.Port,
                                                   this.UseTls, this.CallingAET, this.CalledAET);

            // Wait for 1 second. We need a delay or the file is locked by FileWatcher.
            //Thread.Sleep(1000); 

            // 2. Load the DICOM file
            //var dicomFile = await DicomFile.OpenAsync(filePath);

            InsertOutputDB insertOutputDB = new InsertOutputDB();
            insertOutputDB.InsertSendingStatus(recordID);

            bool success = false;

            var request = new DicomCStoreRequest(dicomFile);


            request.OnResponseReceived = (DicomCStoreRequest req, DicomCStoreResponse response) =>
            {
                DicomStatus status = response.Status;

                if (status.State == DicomState.Success)
                {
                    insertOutputDB.InsertSentStatus(recordID);
                }
                else if (status.State == DicomState.Warning)
                {
                    insertOutputDB.InsertFailedStatus(recordID);
                }
                else if (status.State == DicomState.Failure)
                {
                    insertOutputDB.InsertFailedStatus(recordID);
                }
            };

            try
            {
                await client.AddRequestAsync(request);
                await client.SendAsync();
            }
            catch(FellowOakDicom.Network.DicomNetworkException ex)
            {
                //Log Error
                insertOutputDB.InsertFailedStatus(recordID);
                Logger.Log(1, $"[ERROR] SCU error: {ex}");
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                //Log Error
                insertOutputDB.InsertFailedStatus(recordID);
                Logger.Log(1, $"[ERROR] SCU error: {ex}");
            }
            catch(System.AggregateException ex)
            {
                //Log Error
                insertOutputDB.InsertFailedStatus(recordID);
                Logger.Log(1, $"[ERROR] SCU error: {ex}");
            }


        }
    }
}
