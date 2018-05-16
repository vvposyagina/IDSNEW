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
        public delegate void ManagerReact();
        public OperationContext currentContext;
        public event ManagerReact resumeAnalyze;

        public ManagerCallback(OperationContext context)
        {
            currentContext = context;
            resumeAnalyze = null;
        }
        public void GenerateNetWarning(string[] data)
        {
            currentContext.GetCallbackChannel<IManagerServiceCallback>().HandleNetWarning(data);
        }

        public void GenerateHostWarning(string[] data)
        {
            currentContext.GetCallbackChannel<IManagerServiceCallback>().HandleHostWarning(data);
        }

        public void GoToArchiveMode()
        {
            currentContext.GetCallbackChannel<IManagerServiceCallback>().GetMessageOFF();
        }

        public void ResumeAnalyze()
        {
            if (resumeAnalyze != null)
                resumeAnalyze();
        }
    }
}
