using MSSM_Clone.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string _selectedTable;
        public HomeControl()
        {
            InitializeComponent();
            CreateCombox();
            HomeViewModel.StartAddingData += this.HomeControl_StartAddingData;
            HomeViewModel.StartUpdatingData += this.HomeViewModel_StartUpdatingData;
            HomeViewModel.StartDeletingData += this.HomeViewModel_StartDeletingData;
            HomeViewModel.RefreshindgData += this.HomeViewModel_RefreshindgData;
        }

        private void HomeViewModel_RefreshindgData()
        {
            if (_selectedTable == String.Empty)
                return;
            CreateTable(_selectedTable);
        }

        private void HomeViewModel_StartDeletingData()
        {
            if (dataGrid.SelectedItem == null)
                return;
            object id = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[0];
            MainViewModel.GetInstance().ServerContoller.DeleteData(_selectedTable, id);
        }

        private void HomeViewModel_StartUpdatingData()
        {
            if (dataGrid.SelectedItem == null)
                return;
            object id = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[0];
            List<string> data = new List<string>();
            foreach (var item in inputPanel.Children)
            {
                data.Add((item as TextBox).Text);
            }
            MainViewModel.GetInstance().ServerContoller.Update(_selectedTable, id, data);
        }

        private void HomeControl_StartAddingData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in inputPanel.Children)
            {
                data.Add((item as TextBox).Name, (item as TextBox).Text);
            }
            MainViewModel.GetInstance().ServerContoller.InsertDataToDataBase(_selectedTable, data);

        }

        private void CreateCombox()
        {
            var names = MainViewModel.GetInstance().ServerContoller.GetAllTablesNames();
            for (int i = 0; i < names.Count; i++)
            {
                ComboBoxItem boxItem = new ComboBoxItem();
                boxItem.Content = names[i];
                namesComboBox.Items.Add(boxItem);
                boxItem.Selected += this.BoxItem_Selected;
            }
            
        }

        private void BoxItem_Selected(object sender, RoutedEventArgs e)
        {
            object name = (sender as ComboBoxItem).Content;
            CreateTable(name as string);
            _selectedTable = name as string;
        }

        //DESKTOP-GLH25AE\SQLEXPRESS

        private void CreateTable(string table)
        {
            dataGrid.IsReadOnly = true;
            dataGrid.Columns.Clear();
            dataGrid.ItemsSource = MainViewModel.GetInstance().ServerContoller.GetTable(table).DefaultView;
            CreateInputPanel(MainViewModel.GetInstance().ServerContoller.GetFieldsName(table));
        }

        private void CreateInputPanel(List<string> names)
        {
            inputPanel.Children.Clear();
            for (int i = 0; i < names.Count; i++)
            {
                TextBox tb = new TextBox();
                tb.Text = names[i];
                tb.ToolTip = names[i];
                tb.Name = names[i];
                tb.Margin = new Thickness(10);
                inputPanel.Children.Add(tb);
            }
        }
    }
}
