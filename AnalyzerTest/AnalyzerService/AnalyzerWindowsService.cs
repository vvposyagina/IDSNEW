using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerService
{
    public class AnalyzerServiceWindows : ServiceBase
    {
        public ServiceHost serviceHost = null;

        public AnalyzerServiceWindows()
        {
            ServiceName = "AnalyzerServiceWindows";
        }

        public static void Main()
        {
            ServiceBase.Run(new AnalyzerServiceWindows());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            serviceHost = new ServiceHost(typeof(Analyzer));

            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
