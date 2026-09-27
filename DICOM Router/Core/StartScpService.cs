using FellowOakDicom;
using FellowOakDicom.Network;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
**********************************************************************************************************
**********************************************************************************************************
*                            
*               
*               DICOM C-STORE SCP Implementation using fo-dicom library
*               
*               Startup for a background service that runs the ScpReceiveService class.
*               This class is started in app.xaml.cs and runs in the background.
*               
**********************************************************************************************************
**********************************************************************************************************
*/

// TODO: Implement logging and error handling. Port needs to be pulled from DB.

namespace DICOM_Router.Core
{
    class StartScpService : BackgroundService
    {
        private IDicomServer? _dicomServer;

        private const int DicomPort = 11112; 

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            try
            {
                Debug.WriteLine(
                    $"Starting DICOM SCP on port {DicomPort}...");

                // Create and start the DICOM server
                _dicomServer =
                    DicomServerFactory.Create<ScpReceiveService>(
                        DicomPort,
                        userState: null);

                Debug.WriteLine(
                    $"DICOM SCP started on port {DicomPort}.");


                // Run the service until cancellation is requested
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(
                        TimeSpan.FromSeconds(5),
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown
            }
            catch (Exception ex)
            {
                // Something bad happened, log the error
                Debug.WriteLine(
                    $"DICOM SCP error: {ex}");
            }
        }

        // Stop the DICOM server when the service is stopped
        public override Task StopAsync(
            CancellationToken cancellationToken)
        {
            Debug.WriteLine(
                "Stopping DICOM SCP...");

            _dicomServer?.Stop();
            _dicomServer?.Dispose();

            _dicomServer = null;

            return base.StopAsync(cancellationToken);
        }
    }
}


