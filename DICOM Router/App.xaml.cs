using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using DICOM_Router.Core;
namespace DICOM_Router
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        // The IHost instance for managing the background service.
        private IHost? _host;

        // Override the OnStartup method to start the background service when the application starts.
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<StartScpService>();
                })
                .Build();

            await _host.StartAsync();
        }

        // Override the OnExit method to stop the background service when the application exits.
        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }

            base.OnExit(e);
        }
    }

}
