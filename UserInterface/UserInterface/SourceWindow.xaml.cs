using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for SourceWindow.xaml
    /// </summary>
    public partial class SourceWindow : MetroWindow
    {
        public string Filter { get; set; }
        public Dictionary<string, string> Devices { get; set; }
        public string[] SelectedDevices { get; set; }

        public SourceWindow(ManagerService.ManagerServiceClient client)
        {
            InitializeComponent();
            Devices = Utilities.ParseDevicesList(client.GetDevicesList());

            foreach(string key in Devices.Keys)
            {
                CheckBox cb = new CheckBox();
                cb.Content = String.Format("{0} ({1})", Devices[key], key);
                lbDevices.Items.Add(cb);
            }
        }

        private void bCheckPropriety_Click(object sender, RoutedEventArgs e)
        {
            bool test = Utilities.CheckFilter(tbFilter.Text);

            if (test)
            {
                lFilterIsCorrected.Content = "Заданный фильтр корректен";
                lFilterIsCorrected.Visibility = Visibility.Visible;
                bSaveFilter.IsEnabled = true;
            }
            else
            {
                lFilterIsCorrected.Content = "Заданный фильтр некорректен";
                lFilterIsCorrected.Visibility = Visibility.Visible; 
            }
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            bSaveFilter.IsEnabled = false;
            lFilterIsCorrected.Visibility = Visibility.Hidden;
        }

        private void bSaveFilter_Click(object sender, RoutedEventArgs e)
        {
            List<string> listDevices = new List<string>();

            foreach(var item in lbDevices.Items)
            {
                CheckBox cbItem = (CheckBox)item;
                if(cbItem.IsChecked == true)
                {
                    listDevices.Add(Utilities.ParseDeviceTitle(cbItem.Content.ToString()));
                }  
            }

            if(listDevices.Count == 0)
            {
                MessageBox.Show("Выберите устройство");
            }
            else
            {
                Filter = tbFilter.Text;
                SelectedDevices = listDevices.ToArray();
                this.DialogResult = true;
            }
        }

        private void cbSetFilter_Checked(object sender, RoutedEventArgs e)
        {
            tbFilter.IsEnabled = true;
            bCheckPropriety.IsEnabled = true;
            bSaveFilter.IsEnabled = false;
        }

        private void cbSetFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            tbFilter.IsEnabled = false;
            bCheckPropriety.IsEnabled = false;
            bSaveFilter.IsEnabled = true;
        }
    }
}
