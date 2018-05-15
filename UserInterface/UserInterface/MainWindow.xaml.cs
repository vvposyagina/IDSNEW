using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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

namespace UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, ManagerService.IManagerServiceCallback
    {
        ManagerService.ManagerServiceClient client;
        List<string> Sources;
        List<string> Devices;
        string Filter { get; set; }
        List<string[]> lbMessages;
        string trainingFileName;
        string testFileName;
        List<string> neuralNetworkData;
        List<NNItem> ExistingNN;
        string goal = "";

        public MainWindow()
        {
            InitializeComponent();
            InstanceContext instanceContext = new InstanceContext(this);
            client = new ManagerService.ManagerServiceClient(instanceContext, "WSDualHttpBinding_IManagerService");
            lbMessages = new List<string[]>();
            neuralNetworkData = new List<string>();
            goal = "";
            ExistingNN = new List<NNItem>();

            try
            {
                client.Start();
                Sources = new List<string>();
                Devices = new List<string>();
                Filter = "";
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к сервису");
            }
        }

        private void cbNet_Checked(object sender, RoutedEventArgs e)
        {
            bAddFilter.IsEnabled = true;
            SourceWindow swindow = new SourceWindow(client);
            if (swindow.ShowDialog() == true)
            {
                Filter = swindow.Filter;
            }
        }

        private void cbNet_Unchecked(object sender, RoutedEventArgs e)
        {
            bAddFilter.IsEnabled = false;
        }

        private void cbHost_Checked(object sender, RoutedEventArgs e)
        {
            bDefineLogSource.IsEnabled = true;
        }

        private void cbHost_Unchecked(object sender, RoutedEventArgs e)
        {
            bDefineLogSource.IsEnabled = false;
        }

        private void tbTimeCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void bAddFilter_Click(object sender, RoutedEventArgs e)
        {
            SourceWindow swindow = new SourceWindow(client);
            if (swindow.ShowDialog() == true)
            {
                Filter = swindow.Filter;
            }
        }

        private void bStartScanning_Click(object sender, RoutedEventArgs e)
        {
            if (cbHost.IsChecked == true || cbNet.IsChecked == true)
            {
                if (cbApplication.IsChecked == true || cbSystem.IsChecked == true || cbSecurity.IsChecked == true)
                {
                    cbHost.IsEnabled = false;
                    cbNet.IsEnabled = false;
                    bAddFilter.IsEnabled = false;
                    bDefineLogSource.IsEnabled = false;
                    string[] str = Sources.ToArray();

                    bool mode = cbSaveToFile.IsChecked == true ? true : false;
                    bool host = cbHost.IsChecked == true ? true : false;
                    bool net = cbNet.IsChecked == true ? true : false;


                    //client.StartCollectors(null, str, net, Filter, host, mode);
                }
                else
                {
                    MessageBox.Show("Выберите журналы событий");
                }
            }
            else
            {
                MessageBox.Show("Выберите области мониторинга");
            }
        }

        private void bStopScanning_Click(object sender, RoutedEventArgs e)
        {
            cbHost.IsEnabled = true;
            cbNet.IsEnabled = true;

            if (cbHost.IsChecked == true)
                bDefineLogSource.IsEnabled = true;

            if (cbNet.IsChecked == true)
                bAddFilter.IsEnabled = true;
        }

        private void cbApplication_Checked(object sender, RoutedEventArgs e)
        {
            Sources.Add(cbApplication.Content.ToString());
        }

        private void cbSystem_Checked(object sender, RoutedEventArgs e)
        {
            Sources.Add(cbSystem.Content.ToString());
        }

        private void cbSecurity_Checked(object sender, RoutedEventArgs e)
        {
            Sources.Add(cbSecurity.Content.ToString());
        }

        private void cbSecurity_Unchecked(object sender, RoutedEventArgs e)
        {
            Sources.Remove(cbSecurity.Content.ToString());
        }

        private void cbSystem_Unchecked(object sender, RoutedEventArgs e)
        {
            Sources.Remove(cbSystem.Content.ToString());
        }

        private void cbApplication_Unchecked(object sender, RoutedEventArgs e)
        {
            Sources.Remove(cbApplication.Content.ToString());
        }

        public void HandleNetWarning(string[] message)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss "));
            strb.Append("Сетевая угроза");
            lbInstantMessage.Items.Add(strb);
            lbMessages.Add(message);
        }
        public void HandleHostWarning(string[] message)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss "));
            strb.Append("Внутрисистемная угроза");
            lbInstantMessage.Items.Add(strb);
            lbMessages.Add(message);
        }

        public void GetMessageOFF()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss "));
            strb.Append("Система обнаружения вторжений выключена");
            lbInstantMessage.Items.Add(strb);
        }

        public void GetMessageOK()
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss "));
            strb.Append("Нарушений не обнаружено");
            lbInstantMessage.Items.Add(strb);
        }

        private void bSeeDetails_Click(object sender, RoutedEventArgs e)
        {
            if (lbInstantMessage.SelectedIndex == -1)
            {
                MessageBox.Show("Отсутствуют выбранные события");
            }
            else if (lbInstantMessage.ItemStringFormat == "Нарушений не обнаружено" || lbInstantMessage.ItemStringFormat == "Система обнаружения вторжений выключена")
            {
                tbDetails.Text = lbInstantMessage.ItemStringFormat;
            }
            else
            {
                tbDetails.Text = lbMessages[lbInstantMessage.SelectedIndex].ToString();
                bSkip.IsEnabled = true;
                bNotSkip.IsEnabled = true;
            }
        }

        private void bSkip_Click(object sender, RoutedEventArgs e)
        {
            bSkip.IsEnabled = false;
            bNotSkip.IsEnabled = false;
            int num = lbInstantMessage.SelectedIndex;
            List<string> newEntry = new List<string>(lbMessages[num]);
            newEntry.Add("Нет");
            AddNewEntry(newEntry);
            lbMessages.RemoveAt(num);
            lbInstantMessage.Items.RemoveAt(num);
        }

        private void bNotSkip_Click(object sender, RoutedEventArgs e)
        {
            bSkip.IsEnabled = false;
            bNotSkip.IsEnabled = false;
            int num = lbInstantMessage.SelectedIndex;
            List<string> newEntry = new List<string>(lbMessages[num]);
            newEntry.Add("Да");
            AddNewEntry(newEntry);
            lbMessages.RemoveAt(num);
            lbInstantMessage.Items.RemoveAt(num);
        }

        private void AddNewEntry(List<string> entry)
        {
            switch (entry.Count)
            {
                case 6:
                    client.AddHostData(entry.ToArray());
                    break;
                case 7:
                    client.AddNetData(entry.ToArray());
                    break;
                default:
                    MessageBox.Show("Сообщение содержит ошибки и не может быть добавлено в базу");
                    break;
            }
        }

        private void bUpdateNNData_Click(object sender, RoutedEventArgs e)
        {
            lvExistingNN.Items.Clear();
            string[][] bdresult = client.GetNNData();
            foreach (string[] newEntry in bdresult)
            {
                NNItem item = new NNItem(newEntry);
                ExistingNN.Add(item);
                lvExistingNN.Items.Add(item);
            }
        }

        private void bUpdateNetData_Click(object sender, RoutedEventArgs e)
        {
            lvNetEntry.Items.Clear();
            string[][] bdresult = client.GetNetData();
            foreach (string[] newEntry in bdresult)
            {
                lvNetEntry.Items.Add(new NetItem(newEntry));
            }
        }

        private void bUpdateHostData_Click(object sender, RoutedEventArgs e)
        {
            lvHostEntry.Items.Clear();
            string[][] bdresult = client.GetHostData();
            foreach (string[] newEntry in bdresult)
            {
                lvHostEntry.Items.Add(new HostItem(newEntry));
            }
        }

        private void nLoadTrainingSampling_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Text documents (.txt)|*.txt";
            dlg.DefaultExt = ".txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                tbTrainingFileName.Text = dlg.FileName;
                trainingFileName = dlg.FileName;
            }
        }

        private void nLoadTestSampling_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Text documents (.txt)|*.txt";
            dlg.DefaultExt = ".txt";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                tbTestFileName.Text = dlg.FileName;
                testFileName = dlg.FileName;
            }
        }

        private void bTrain_Click(object sender, RoutedEventArgs e)
        {
            lbAboutNewNetwork.Items.Clear();
            if (testFileName != "" && trainingFileName != "")
            {
                if (tbEpochCount.Text != "" && tbNeuronCountInHiddenLayer.Text != "")
                {
                    if (rbHostNN.IsChecked == true || rbNetNN.IsChecked == true)
                    {
                        goal = rbNetNN.IsChecked == true ? "NET" : "HOST";
                        int epochCount = Convert.ToInt32(tbEpochCount.Text);
                        int neuronCountInHiddenLayer = Convert.ToInt32(tbNeuronCountInHiddenLayer.Text);
                        double[] result = client.RequestRetraining(trainingFileName, testFileName, goal, epochCount, neuronCountInHiddenLayer);
                        GetResultRetraining(result);
                    }
                    else
                    {
                        MessageBox.Show("Выберите цель построения нейросети");
                    }
                }
                else
                {
                    MessageBox.Show("Введите данные для определения конфигурации сети");
                }
            }
            else
            {
                MessageBox.Show("Загрузите файлы с обучающей и тестовой выборками");
            }
        }

        private void checkValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void GetResultRetraining(double[] results)
        {
            lbAboutNewNetwork.Items.Add("Результаты обучения");
            lbAboutNewNetwork.Items.Add(String.Format("Цель: {0}", goal));
            lbAboutNewNetwork.Items.Add(String.Format("Количество записей в обучающей выборке: {0}", results[0]));
            lbAboutNewNetwork.Items.Add(String.Format("Количество нормальных записей в тестовой выборке: {0}", results[1]));
            lbAboutNewNetwork.Items.Add(String.Format("Количество подозрительных записей в тестовой выборке: {0}", results[2]));
            lbAboutNewNetwork.Items.Add(String.Format("Количество записей в тестовой выборке: {0}", results[3]));
            lbAboutNewNetwork.Items.Add(String.Format("Количество эпох: {0}", results[4]));
            lbAboutNewNetwork.Items.Add(String.Format("Точность: {0}", results[5]));
            lbAboutNewNetwork.Items.Add(String.Format("Ошибка первого рода: {0}", results[6]));
            lbAboutNewNetwork.Items.Add(String.Format("Ошибка второго рода: {0}", results[7]));

            neuralNetworkData.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            neuralNetworkData.Add(goal);
            neuralNetworkData.Add(results[0].ToString());
            neuralNetworkData.Add(results[1].ToString());
            neuralNetworkData.Add(results[2].ToString());
            neuralNetworkData.Add(results[3].ToString());
            neuralNetworkData.Add(results[4].ToString());
            neuralNetworkData.Add(results[5].ToString());
            neuralNetworkData.Add(results[6].ToString());
            neuralNetworkData.Add(results[7].ToString());

            client.AddNNData(neuralNetworkData.ToArray());
        }

        private void bSaveInFile_Click(object sender, RoutedEventArgs e)
        {
            client.UpdateNeuralNetwork(goal);
            goal = "";
            testFileName = "";
            trainingFileName = "";
        }
    }
}
