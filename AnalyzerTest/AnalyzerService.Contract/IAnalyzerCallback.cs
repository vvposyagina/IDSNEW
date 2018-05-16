using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerServ.Contract
{
    public interface IAnalyzerCallback
    {
        [OperationContract(IsOneWay = true)]
        void GenerateNetWarning(string[] data);

        [OperationContract(IsOneWay = true)]
        void GenerateHostWarning(string[] data);

        [OperationContract(IsOneWay = true)]
        void GoToArchiveMode();      

        [OperationContract(IsOneWay = true)]
        void ResumeAnalyze();
    }
}
