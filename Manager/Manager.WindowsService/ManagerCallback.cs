using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.WindowsService
{
    public class ManagerCallback : AnalyzerService.IAnalyzerCallback
    {
        public void GenerateNetWarning(string[] data)
        {
            OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().HandleNetWarning(data);
            //Console.WriteLine("Warning!");
        }

        public void GenerateHostWarning(string[] data)
        {
            OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().HandleHostWarning(data);
        }

        public void GoToArchiveMode()
        {
            OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().GetMessageOFF();
        }

        public void SendOK()
        {
            OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().GetMessageOK();
        }

        public void ResumeAnalyze()
        {
            
        }
    }
}
