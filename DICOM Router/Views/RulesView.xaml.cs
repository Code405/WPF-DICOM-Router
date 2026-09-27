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
    /// Interaction logic for RulesView.xaml
    /// </summary>
    public partial class RulesView : UserControl
    {
        public RulesView()
        {
            InitializeComponent();

            RulesData rule1 = new RulesData("Rule 1", "Replace", "0010,0010", "0010,0020", "New Value", "Match Value");
            RulesData rule2 = new RulesData("Rule 2", "Match", "0010,0030", "0010,0040", "Replace Value", "Match Value");
            RulesData rule3 = new RulesData("Rule 3", "Replace", "0010,0050", "0010,0060", "New Value", "Match Value");
            RulesData rule4 = new RulesData("Rule 4", "Match", "0010,0070", "0010,0080", "Replace Value", "Match Value");
            RulesData rule5 = new RulesData("Rule 5", "Replace", "0010,0090", "0010,00A0", "New Value", "Match Value");

            RulesDataGrid.Items.Add(rule1);
            RulesDataGrid.Items.Add(rule2);
            RulesDataGrid.Items.Add(rule3);
            RulesDataGrid.Items.Add(rule4);
            RulesDataGrid.Items.Add(rule5);
        }
    }
}
