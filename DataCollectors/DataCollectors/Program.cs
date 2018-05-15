using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDataCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Collector test = new Collector();
            test.Start();
        }
    }
}
