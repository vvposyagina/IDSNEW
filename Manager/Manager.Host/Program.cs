using HostDataCollector;
using Manager.WindowsService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var host = new ServiceHost(typeof(ManagerService)))
            //{
            //    host.Open();
            //    Console.WriteLine("Host is started...");
            //    //ManagerService client = new ManagerService();
            //    //string[] str = { "Application", "System" };
            //    //List<string> list = new List<string>();
            //    //client.Start(null, str, true, "", false, true);
            //    Console.ReadLine();
            //}

            //NetDataCollector.NetCollector nc = new NetDataCollector.NetCollector("", true);
            //nc.Start();

            ManagerService ms = new ManagerService();
            //InstanceContext instanceContext = new InstanceContext(new Callback());
            //AnalyzerService.AnalyzerClient client = new AnalyzerService.AnalyzerClient(instanceContext, "WSDualHttpBinding_IAnalyzer");
            //client.StartService();
            ms.Start();
            //ms.StartCollectors(null, null, true, "", true);
            //string[][] data = ms.GetNetData();
            ms.GetNNData();
        }
    }
}
