using DICOM_Router.Dummy;
using System;
using System.Collections.Generic;
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

namespace DICOM_Router.Views
{
    /// <summary>
    /// Interaction logic for IncomingView.xaml
    /// </summary>
    public partial class IncomingView : UserControl
    {
        public IncomingView()
        {
            InitializeComponent();

            IncomingDicomData data = new IncomingDicomData(DateTime.Now, 12345, "John Doe", "CT", 10, "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            IncomingDicomData data2 = new IncomingDicomData(DateTime.Now, 67890, "Jane Smith", "MRI", 20, "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            IncomingDicomData data3 = new IncomingDicomData(DateTime.Now, 54321, "Alice Johnson", "X-Ray", 5, "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            IncomingDicomData data4 = new IncomingDicomData(DateTime.Now, 98765, "Bob Brown", "Ultrasound", 15, "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            IncomingDicomData data5 = new IncomingDicomData(DateTime.Now, 24680, "Charlie Davis", "PET", 8, "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");


            IncomingDataGrid.Items.Add(data);
            IncomingDataGrid.Items.Add(data2);
            IncomingDataGrid.Items.Add(data3);
            IncomingDataGrid.Items.Add(data4);
            IncomingDataGrid.Items.Add(data5);
        }
    }
}
