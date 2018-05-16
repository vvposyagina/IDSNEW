using HostDataCollector;
using Manager.Contract;
using Manager.DataAccess;
using NetDataCollector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Manager.WindowsService
{
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ManagerService : IManagerService
    {
        string[] DEFAULT_SOURCES = { "Application", "System", "Security" };
        Dictionary<string, NetCollector> netDataCollector;
        Dictionary<string, HostCollector> hostDataCollector;
        DBAccess dbaccess;
        string address = @"E:\Диплом\Новая\IDSNEW\UserInterface\UserInterface\bin\Debug\SuspiciousEvents.db";
        ManagerCallback managercb;

        AnalyzerService.AnalyzerClient client;

        public void Start()
        {
            managercb = new ManagerCallback(OperationContext.Current);
            managercb.resumeAnalyze += RestartCollectors;
            InstanceContext instanceContext = new InstanceContext(managercb);
            client = new AnalyzerService.AnalyzerClient(instanceContext, "WSDualHttpBinding_IAnalyzer");
            client.StartService();
            dbaccess = new DBAccess(address);

            netDataCollector = new Dictionary<string, NetCollector>();
            hostDataCollector = new Dictionary<string, HostCollector>();
        }

        public void Stop()
        {
            StopHostDataCollector();
            StopNetDataCollector();
        }     

        public void StartCollectors(string[] devicesNames, string[] sources, bool net = true, string filter = "", bool host = true, bool mode = true)
        {
            if (net)
                StartNetDataCollector(devicesNames, filter, mode);

            if (host)
                StartHostDataCollector(sources, mode);
        }

        public void StartNetDataCollector(string[] devicesNames, string filter, bool mode)
        {
            if (devicesNames != null)
            {
                foreach (string device in devicesNames)
                {
                    NetCollector newNetDataCollector = new NetCollector(device, filter, mode);
                    netDataCollector.Add(device, newNetDataCollector);
                    newNetDataCollector.sendToManager += SendNetPacketToAnalyzer;
                    newNetDataCollector.Start();
                }
            }
            else
            {
                NetCollector newNetDataCollector = new NetCollector(filter, mode);
                netDataCollector.Add(newNetDataCollector.CurrentDevice.Name, newNetDataCollector);
                newNetDataCollector.sendToManager += SendNetPacketToAnalyzer;
                newNetDataCollector.Start();
            }
        }

        public void StartHostDataCollector(string[] sources, bool mode)
        {
            if (sources != null)
            {
                foreach (string source in sources)
                {
                    HostCollector newHostDataCollector = new HostCollector(source, mode);
                    hostDataCollector.Add(source, newHostDataCollector);
                    newHostDataCollector.sendToManager += SendHostPacketToAnalyzer;
                    newHostDataCollector.Start();
                }
            }
            else
            {
                foreach (string source in DEFAULT_SOURCES)
                {
                    HostCollector newHostDataCollector = new HostCollector(source, mode);
                    hostDataCollector.Add(source, newHostDataCollector);
                    newHostDataCollector.sendToManager += SendHostPacketToAnalyzer;
                    newHostDataCollector.Start();
                }
            }
        }
       
        private void SendNetPacketToAnalyzer(string[] data)
        {
           client.CheckNetPackets(data);
        }

        private void SendHostPacketToAnalyzer(string[] data)
        {
            client.CheckHostPackets(data);
        }

        public void StopNetDataCollector()
        {
            foreach (NetCollector ndCollector in netDataCollector.Values)
            {
                ndCollector.Stop();
            }

            netDataCollector.Clear();
        }

        public void StopHostDataCollector()
        {
            foreach (HostCollector hdCollector in hostDataCollector.Values)
            {
                hdCollector.Stop();
            }

            hostDataCollector.Clear();
        }

        public void Pause(List<string> nCollectors, List<string> hCollectors)
        {
            foreach (string ndCollector in nCollectors)
            {
                if (netDataCollector.ContainsKey(ndCollector))
                    netDataCollector[ndCollector].Stop();
            }

            foreach (string hdCollector in hCollectors)
            {
                if (hostDataCollector.ContainsKey(hdCollector))
                    hostDataCollector[hdCollector].Stop();
            }
        }

        public string[] GetDevicesList()
        {
            string[] result = NetCollector.GetDeviceDictionary().ToArray();
            return result;
        }

        public string[][] GetNetData()
        {
            string[][] data = dbaccess.GetAllNetEntrys();
            return data;
        }

        public string[][] GetHostData()
        {
            string[][] data = dbaccess.GetAllHostEntrys();
            return data;
        }

        public string[][] GetNNData()
        {
            string[][] data = dbaccess.GetAllNNEntrys();
            return data;
        }

        public void AddNetData(string[] data)
        {
            dbaccess.AddNetLog(data);            
        }

        public void AddHostData(string[] data)
        {
            dbaccess.AddHostLog(data); 
        }

        public void AddNNData(string[] data)
        {
            dbaccess.AddNNEntry(data); 
        }

        public double[] RequestRetraining(string trainingFileName, string testFileName, string goal, int epochCount, int neuronCountInHiddenLayer)
        {
            return client.CreateNewNN(trainingFileName, testFileName, goal, epochCount, neuronCountInHiddenLayer);
        }

        public void UpdateNeuralNetwork()
        {
            PauseHostDataCollector();
            PauseNetDataCollector();
            client.ChangeNN();
            RestartHostDataCollector();
            RestartNetDataCollector();
        }

        private void PauseHostDataCollector()
        {

        }

        private void PauseNetDataCollector()
        {

        }

        private void RestartHostDataCollector()
        {

        }

        private void RestartNetDataCollector()
        {

        }

        public void RestartCollectors()
        {

        }
    }
}
