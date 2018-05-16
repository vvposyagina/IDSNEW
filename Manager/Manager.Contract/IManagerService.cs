using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Manager.Contract
{
    [ServiceContractAttribute(SessionMode = SessionMode.Required, CallbackContract = typeof(IManagerServiceCallback))]
    public interface IManagerService
    {
        [OperationContract(IsInitiating = true, IsOneWay = true, IsTerminating = false)]
        void Start();

        [OperationContract(IsInitiating=false, IsOneWay = true, IsTerminating=false)]
        void StartCollectors(string[] devicesNames, string[] sources, bool net = true, string filter = "", bool host = true, bool mode = true);

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = true)]
        void Stop();

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = false)]
        void Pause();

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = false)]
        void StartNetDataCollector(string[] devicesNames, string filter, bool mode);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void StopNetDataCollector();

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        string[] GetDevicesList();

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void StartHostDataCollector(string[] sources, bool mode);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void StopHostDataCollector();

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        double[] RequestRetraining(string trainingFileName, string testFileName, string goal, int epochCount, int neuronCountInHiddenLayer);

        [OperationContract(IsInitiating = false, IsOneWay = true, IsTerminating = false)]
        void UpdateNeuralNetwork();
      
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        string[][] GetNetData();        

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        string[][] GetHostData();   

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        string[][] GetNNData();          

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void AddNetData(string[] data);      

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void AddHostData(string[] data);

        [OperationContract(IsInitiating = false, IsTerminating = false)]
        void AddNNData(string[] data);
      
    } 
}
