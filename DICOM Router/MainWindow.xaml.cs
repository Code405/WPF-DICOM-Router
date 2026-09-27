using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DICOM_Router.Views;
using DICOM_Router.ViewModels;
using DICOM_Router.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DICOM_Router
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Launcher();
        }

        // Menu Button Click Handlers
        private void incomingButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new IncomingViewModel();
        }
        private void outgoingButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new OutgoingViewModel();
        }

        private void serverButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new SettingsViewModel();    
        }

        private void rulesButton_Click(object sender, RoutedEventArgs e)
        {
            DataContext = new RulesViewModel();
        }

        // This will be Relocated to App.xaml.cs in the future for better structure and organization.
        // Launcher for starting Background services before use.
        private async void Launcher()
        {
            LoadingProgressBar.Value += await Task.Run(() => loadingDB());
            loadingLabel.Content = "Connecting to Database...";
            LoadingProgressBar.Value += 25; // Receiver Service Startup Need to figure out how to do this from app.xaml.cs
            loadingLabel.Content = "Starting DICOM Receiver Server";
            LoadingProgressBar.Value += await Task.Run(() => loadingRules());
            loadingLabel.Content = "Loading Rules from Database...";

            // Hide Loading Panel and Load Incoming View after everything is ready.
            LoadingPanel.Visibility = Visibility.Collapsed;
            ShowMenu();
            DataContext = new IncomingViewModel();
        }

        // Methods to start indivdual services.
        // Thses need to be moved to app.xaml.cs in the future for better structure and organization.
        private int loadingDB()
        {
            Thread.Sleep(500);
            return 25;
        }
        private int loadingRules()
        {
            Thread.Sleep(500);
            return 25;
        }

        /*   CLEAN UP MOVED TO APP.XAML.CS
        async Task LoadingServer()
        {

            Thread.Sleep(500);
            //return 25;
            
        } 
        */

        // Method to show the menu buttons after loading is complete.
        private void ShowMenu()
        {
            incomingButton.Visibility = Visibility.Visible;
            outgoingButton.Visibility = Visibility.Visible;
            serverButton.Visibility = Visibility.Visible;
            rulesButton.Visibility = Visibility.Visible;
        }
    }
}