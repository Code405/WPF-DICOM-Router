using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;
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
   
        public ScuSendService(string host, int port, string callingAET, string calledAET)
        {
            this.Host = host;
            this.Port = port;
            this.CallingAET = callingAET;
            this.CalledAET = calledAET;
        }

        public async Task Send(DicomFile dicomFile)
        {
            // Create a C-STORE request for the DICOM file
            var client = DicomClientFactory.Create(this.Host, this.Port,
                                                   this.UseTls, this.CallingAET, this.CalledAET);

            // Wait for 1 second. We need a delay or the file is locked by FileWatcher.
            //Thread.Sleep(1000); 

            // 2. Load the DICOM file
            //var dicomFile = await DicomFile.OpenAsync(filePath);


            var request = new DicomCStoreRequest(dicomFile);

            // TODO: Implement error handling and logging for the C-STORE request

            await client.AddRequestAsync(request);
            await client.SendAsync();

            // Add status update to DB:

        }
    }
}
