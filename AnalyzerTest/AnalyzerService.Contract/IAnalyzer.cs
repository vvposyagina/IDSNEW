using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerServ.Contract
{
    [ServiceContractAttribute(SessionMode = SessionMode.Required, CallbackContract = typeof(IAnalyzerCallback))]
    public interface IAnalyzer
    {
        [OperationContract(IsInitiating = true, IsOneWay = true, IsTerminating = false)]
        void StartService();

        [OperationContract(IsInitiating = true, IsOneWay = true, IsTerminating = false)]
        void CheckNetPackets(string[] packets);

        [OperationContract(IsInitiating = true, IsOneWay = true, IsTerminating = false)]
        void CheckHostPackets(string[] packets);

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = true)]
        void Stop();

        [OperationContract(IsInitiating = false, IsOneWay = false, IsTerminating = false)]
        double[] CreateNewNN(string trainingFileName, string testFileName, string goal, int epochCount, int neuronCountInHiddenLayer);

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = false)]
        void ChangeNN();
    }
}
