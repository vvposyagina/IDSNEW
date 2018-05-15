using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Manager.WindowsService
{
    public class ManagerServiceWindows : ServiceBase
    {
        public ServiceHost serviceHost = null;

        public ManagerServiceWindows()
        {
            ServiceName = "ManagerWindowsService";
        }

        public static void Main()
        {
            ServiceBase.Run(new ManagerServiceWindows());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            serviceHost = new ServiceHost(typeof(ManagerService));

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
