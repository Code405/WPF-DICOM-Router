using DICOM_Router.Dummy;
using DICOM_Router.ViewModels;
using DICOM_Router.Models;
using Microsoft.EntityFrameworkCore;
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
using System.Collections.ObjectModel;

namespace DICOM_Router.Views
{
    /// <summary>
    /// Interaction logic for IncomingView.xaml
    /// </summary>
    public partial class IncomingView : UserControl
    {
        private ObservableCollection<IncomingStudy> incomingStudiesList;
        public IncomingView()
        { 
            InitializeComponent();

            DatabaseContext context = new DatabaseContext();

            //context.IncomingStudies.Load();

            incomingStudiesList = new ObservableCollection<IncomingStudy>(context.IncomingStudies.ToList());

            IncomingDataGrid.ItemsSource = incomingStudiesList;

        }
    }
}
