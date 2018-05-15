using HostDataCollector;
using Manager.Contract;
using NetDataCollector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Manager.Service
{
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ManagerService : IManagerService
    {
        public delegate void SendNewReport();
        string[] DEFAULT_SOURCES = { "Application", "System", "Security" };
        Dictionary<string, NetCollector> netDataCollector;
        Dictionary<string, HostCollector> hostDataCollector;
        public event SendNewReport ReportReceived;

        public void Start(List<string> devicesNames, string[] sources, bool net = true, string filter = "", bool host = true, bool mode = true)
        {
            netDataCollector = new Dictionary<string, NetCollector>();
            hostDataCollector = new Dictionary<string, HostCollector>();

            if (net)
                StartNetDataCollector(devicesNames, filter, mode);  

            if (host)
                StartHostDataCollector(sources, mode);
        }

        public void Stop()
        {
            StopHostDataCollector();
            StopNetDataCollector();
        }

        public void StartNetDataCollector(List<string> devicesNames, string filter, bool mode)
        {
            if (devicesNames != null)
            {
                foreach (string device in devicesNames)
                {
                    NetCollector newNetDataCollector = new NetCollector(device, filter, mode);
                    netDataCollector.Add(device, newNetDataCollector);
                    newNetDataCollector.sendToManager += GetNewPacketFromNet;
                    newNetDataCollector.Start();
                }
            }
            else
            {
                NetCollector newNetDataCollector = new NetCollector(filter, mode);
                netDataCollector.Add(newNetDataCollector.CurrentDevice.Name, newNetDataCollector);
                newNetDataCollector.sendToManager += GetNewPacketFromNet;
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
                    newHostDataCollector.sendToManager += GetNewPacketFromHost;
                    newHostDataCollector.Start();
                }
            }
            else
            {
                foreach (string source in DEFAULT_SOURCES)
                {
                    HostCollector newHostDataCollector = new HostCollector(source, mode);
                    hostDataCollector.Add(source, newHostDataCollector);
                    newHostDataCollector.sendToManager += GetNewPacketFromHost;
                    newHostDataCollector.Start();
                }
            }
        }

        public void GetNewPacketFromNet(NetCollector ndCollector)
        {
            string[] newPackets = ndCollector.ReturnLastPackets();
            SendPacketToAnalyzer(newPackets);
        }

        public void GetNewPacketFromHost(HostCollector hdCollector)
        {
            string[] newPackets = hdCollector.ReturnLastPackets();
            SendPacketToAnalyzer(newPackets);
        }

        private void SendPacketToAnalyzer(string[] data)
        {

        }

        public void StopNetDataCollector()
        {
            foreach(NetCollector ndCollector in netDataCollector.Values)
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

        public void StartService()
        {
            return;
        }

        public string[] GetDevicesList()
        {
            string[] result = NetCollector.GetDeviceDictionary().ToArray();
            return result;
        }
    }
}
