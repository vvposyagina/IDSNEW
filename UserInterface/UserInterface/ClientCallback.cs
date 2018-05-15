using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class ClientCallback : ManagerService.IManagerServiceCallback
    {
        public void HandleHostWarning(string[] data)
        {
            throw new NotImplementedException();
        }

        public void HandleNetWarning(string[] data)
        {
            throw new NotImplementedException();
        }

        public void GetMessageOFF()
        {
            throw new NotImplementedException();
        }

        public void GetMessageOK()
        {
            throw new NotImplementedException();
        }
    }
}
