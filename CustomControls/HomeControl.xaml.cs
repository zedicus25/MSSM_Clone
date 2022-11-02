using MSSM_Clone.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSSM_Clone.CustomControls
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        public HomeControl()
        {
            InitializeComponent();
            CreateTable();
        }
        //DESKTOP-GLH25AE\SQLEXPRESS

        private void CreateTable()
        {
            dataGrid.AutoGenerateColumns = false;
            var data = MainViewModel.Instance.ServerContoller.GetFieldsData("SHIPPERS");
            var names = MainViewModel.Instance.ServerContoller.GetFieldsName("SHIPPERS");

            for (int j = 0; j < data[0].Count; j++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = names[j];
                column.Binding = new Binding($"[{j}]");
                dataGrid.Columns.Add(column);
            }
            dataGrid.ItemsSource = data;
            
        }
    }
}
