using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDataCollector
{
    public static class SuspiciousPacketGenerator
    {
        public static string GenerateSample(int length, string label, Random rand, bool withLabel)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            strb.Append('|');
            int temp;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp = rand.Next(256);
                    strb.AppendFormat("{0}", temp);
                    strb.Append('.');
                }
                strb.Remove(strb.Length - 1, 1);
                strb.Append('|');
            }

            temp = rand.Next(10000);
            strb.AppendFormat("{0}", temp);
            strb.Append('|');
            strb.AppendFormat("{0};", temp);

            temp = rand.Next(4);
            strb.AppendFormat("{0};", temp);

            for (int i = 0; i < 5; i++)
            {
                temp = rand.Next(1000);
                strb.AppendFormat("{0};", temp);
            }

            for (int i = 0; i < 6; i++)
            {
                temp = rand.Next(5000);
                strb.AppendFormat("{0};", temp);
            }

            for (int i = 0; i < 4; i++)
            {
                temp = rand.Next(100);
                strb.AppendFormat("{0};", temp);
            }

            for (int i = 17; i < length; i++)
            {
                temp = rand.Next(2);
                strb.AppendFormat("{0};", temp);
            }

            if(withLabel)
                strb.AppendFormat("{0};", label);

            return strb.ToString();
        }
    }
}
