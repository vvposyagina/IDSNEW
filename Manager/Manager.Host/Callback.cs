using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Host
{
    public class Callback : AnalyzerService.IAnalyzerCallback
    {
        public void GenerateNetWarning(string[] data)
        {
            Console.WriteLine("Warning!");
            //OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().HandleNetWarning(data);
        }

        public void GenerateHostWarning(string[] data)
        {
            //OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().HandleHostWarning(data);
        }

        public void GoToArchiveMode()
        {
            OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().GetMessageOFF();
        }

        public void SendOK()
        {
            //OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().GetMessageOK();
            Console.WriteLine("OK");
        }

        public void ResumeAnalyze()
        {
            //OperationContext.Current.GetCallbackChannel<IManagerServiceCallback>().GetMessageOK();
            //Console.WriteLine("OK");
        }
    }
}
