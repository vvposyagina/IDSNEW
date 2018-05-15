using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Contract
{
    public interface IManagerServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void HandleHostWarning(string[] data);

        [OperationContract(IsOneWay = true)]
        void HandleNetWarning(string[] data);

        [OperationContract(IsOneWay = true)]
        void GetMessageOFF();

        [OperationContract(IsOneWay = true)]
        void GetMessageOK();       
    }
}
