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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        // Launcher for starting Background services before use.
        private async void Launcher()
        {
            LoadingProgressBar.Value += await Task.Run(() => loadingDB());
            LoadingProgressBar.Value += await Task.Run(() => loadingServer());
            LoadingProgressBar.Value += await Task.Run(() => loadingRules());
            loadingLabel.Content = "Done Loading";
            // Load Incoming View after everything is ready.
        }


        // Methods to start indivdual services.
        private int loadingDB()
        {
            Thread.Sleep(3500);
            return 25;
        }
        private int loadingRules()
        {
            Thread.Sleep(1500);
            return 25;
        }
        private int loadingServer()
        {
            Thread.Sleep(500);
            return 25;
        }


    }
}