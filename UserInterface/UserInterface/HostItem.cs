using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class HostItem
    {
         public int ID { get; set; }

        public string Date { get; set; }

        public int EventID { get; set; }

        public string Provider { get; set; }

        public string Data { get; set; }

        public string Threat { get; set; }

        public HostItem(string[] bdentry)
        {
            if(bdentry.Length == 6)
            {
                ID = Convert.ToInt32(bdentry[0]);
                Date = bdentry[1];
                EventID = Convert.ToInt32(bdentry[2]);
                Provider = bdentry[3];
                Data = bdentry[4];
                Threat = bdentry[5];
            }
        }
    }
}
