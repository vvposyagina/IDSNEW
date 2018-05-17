using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalyzerLibrary
{
    public class LogEntry
    {
        public string Level { get; private set; }
        public string Provider { get; private set; }
        public double TimeBtwGW { get; private set; }
        public long EventID { get; private set; }
        public string Task { get; private set; }
        public string[] EventData { get; private set; }

        public string Data { get; private set; }

        public LogEntry() { }

        public LogEntry(string entry, char delimeter)
        {
            string[] entryarr = entry.Split(delimeter);
            Level = entryarr[1];
            Provider = entryarr[0];
            //TimeBtwGW = Convert.ToDouble(entryarr[1]);
            EventID = Convert.ToInt64(entryarr[2]);
            Task = entryarr[3];
            Data = entryarr[4];
            EventData = HandleMessage(entryarr[4]).Split(' ');
        }

        public string HandleMessage(string str)
        {
            string pattern = @"\d{2}.\d{2}.\d{4}";
            string replacement = " ";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(str, replacement);

            pattern = @"\d{2}:\d{2}:\d{2}";
            result = rgx.Replace(str, replacement);

            result = Regex.Replace(result, "[-.?!)(,:]", " ");

            return result;
        }

        public string[] ToStringArray()
        {
            List<string> result = new List<string>();
            result.Add(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
            result.Add(EventID.ToString());
            result.Add(Provider);
            result.Add(Data);
            return result.ToArray();
        }
    }
}