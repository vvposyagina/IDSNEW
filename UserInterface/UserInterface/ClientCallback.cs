using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class ClientCallback : ManagerService.IManagerServiceCallback
    {
        public delegate void GetData(string[] data);
        public event GetData GetNetWarning = null;
        public event GetData GetHostWarning = null;

        public void HandleHostWarning(string[] data)
        {
            if (GetHostWarning != null)
                GetHostWarning(data);
        }

        public void HandleNetWarning(string[] data)
        {
            if (GetNetWarning != null)
                GetNetWarning(data);
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
