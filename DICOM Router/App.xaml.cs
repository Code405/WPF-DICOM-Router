using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using DICOM_Router.Core;
using Microsoft.EntityFrameworkCore;

namespace DICOM_Router
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        // The IHost instance for managing the background service.
        private IHost? _host;

        // Create FileWatcherService variable from core.FileWatcherService.   
        public FileWatcherService fsw;

        // Override the OnStartup method to start the background service when the application starts.
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //using var db = new DICOM_Router.Models.DicomContext();

            fsw = new FileWatcherService(); //Initialize a global service with fsw variable. 

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<StartScpService>();
                })
                .Build();

            await _host.StartAsync();

            fsw.Start();
        }

        // Override the OnExit method to stop the background service when the application exits.
        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            fsw?.Dispose(); // Dispose the FileWatcherService when the application exits.

            base.OnExit(e);
        }
    }

}
