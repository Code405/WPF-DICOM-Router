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
    /// Interaction logic for OutgoingView.xaml
    /// </summary>
    public partial class OutgoingView : UserControl
    {
        public OutgoingView()
        {
            InitializeComponent();

            OutgoingDicomData data = new OutgoingDicomData(DateTime.Now, 12345, "John Doe", "CT", 10, "Archive1", "Archive", "Success", "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            OutgoingDicomData data2 = new OutgoingDicomData(DateTime.Now, 67890, "Jane Smith", "MRI", 20, "Archive1", "Archive", "Success", "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            OutgoingDicomData data3 = new OutgoingDicomData(DateTime.Now, 54321, "Alice Johnson", "X-Ray", 5, "Archive1", "Archive", "Success", "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            OutgoingDicomData data4 = new OutgoingDicomData(DateTime.Now, 98765, "Bob Brown", "Ultrasound", 15, "Archive1", "Archive", "Success", "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");
            OutgoingDicomData data5 = new OutgoingDicomData(DateTime.Now, 24680, "Charlie Davis", "PET", 8, "Archive1", "Archive", "Success", "1.2.826.0.1.3680043.8.1055.1.20111103112244831.40200514.30965937");


            OutgoingDataGrid.Items.Add(data);
            OutgoingDataGrid.Items.Add(data2);
            OutgoingDataGrid.Items.Add(data3);
            OutgoingDataGrid.Items.Add(data4);
            OutgoingDataGrid.Items.Add(data5);
        }
    }
}
