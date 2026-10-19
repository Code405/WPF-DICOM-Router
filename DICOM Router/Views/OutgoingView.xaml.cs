using DICOM_Router.Dummy;
using DICOM_Router.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<OutgingStudy> outgoingStudiesList;
        public OutgoingView()
        {
            InitializeComponent();

            // This is how the data is loaded from the DB to the DataGrid.
            DatabaseContext context = new DatabaseContext();

            outgoingStudiesList = new ObservableCollection<OutgingStudy>(context.OutgingStudies.ToList());

            OutgoingDataGrid.ItemsSource = outgoingStudiesList;
            
        }
    }
}
