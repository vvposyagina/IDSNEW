using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public class NetItem
    {
        public int ID { get; set; }

        public string Date { get; set; }

        public string Src { get; set; }

        public string Dst { get; set; }

        public int Length { get; set; }

        public string Data { get; set; }

        public string Threat { get; set; }

        public NetItem(string[] bdentry)
        {
            if(bdentry.Length == 7)
            {
                ID = Convert.ToInt32(bdentry[0]);
                Date = bdentry[1];
                Src = bdentry[2];
                Dst = bdentry[3];
                Length = Convert.ToInt32(bdentry[4]);
                Data = bdentry[5];
                Threat = bdentry[6];
            }
        }
    }
}
