using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Host
{
    public class Callback : AnalyzerService.IAnalyzerCallback
    {
        public void GenerateNetWarning(string description)
        {
            Console.WriteLine(description);
        }

        public void GenerateHostWarning(string description)
        {
            Console.WriteLine(description);
        }

        public void GoToArchiveMode(string description)
        {
            Console.WriteLine(description);
        }

        public void SendOK()
        {
            Console.WriteLine("OK");
        }
    }
}
