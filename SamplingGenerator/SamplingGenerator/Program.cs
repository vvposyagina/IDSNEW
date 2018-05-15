using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingGenerator
{
    class Program
    {
        public static string GenerateSample(int length, string label, Random rand)
        {
            StringBuilder strb = new StringBuilder();
            strb.Append(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            strb.Append('|');
            int temp;

            for (int i = 0; i < 2; i++ )
            {
                for(int j = 0; j < 4; j++)
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

            //strb.AppendFormat("{0};", label);

            return strb.ToString();
        }

        static void Main(string[] args)
        {
            string FileDirectory = @"E:\Диплом\Прога\SamplingGenerator";
            DirectoryInfo directory = new DirectoryInfo(FileDirectory);
            Random rand = new Random(unchecked((int)(DateTime.Now.Ticks)));

            if (!directory.Exists)
            {
                directory.Create();
            }

            string FileName = "asmpling.txt";

            string path = String.Format(@"{0}//{1}", FileDirectory, FileName);

            int n = 100, length = 36;

            using (StreamWriter writer = File.AppendText(path))
            {
                for (int i = 0; i < n; i++)
                {
                    string newSample = GenerateSample(length, "0", rand);
                    writer.WriteLine(newSample.ToString());
                }
            }
            
        }
    }
}
