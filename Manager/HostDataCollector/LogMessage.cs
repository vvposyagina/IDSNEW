using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HostDataCollector
{
    public class LogMessage
    {
        public string Level { get; private set; }
        public string Provider { get; private set; }
        public DateTime EventsDate { get; private set; }
        public DateTime EntrysDate { get; private set; }
        public double TimeBtwGW { get; private set; }
        public long EventID { get; set; }
        public string Task { get; private set; }
        public string EventData { get; private set; }

        public LogMessage() { }

        public LogMessage(System.Diagnostics.EventLogEntry entry)
        {
            Level = entry.Category;
            Provider = entry.Source;
            EventsDate = entry.TimeGenerated;
            EntrysDate = entry.TimeWritten;
            TimeBtwGW = EntrysDate.Subtract(EventsDate).TotalSeconds;
            EventID = entry.InstanceId;
            Task = entry.Category;
            EventData = entry.Message;
        }

        public LogMessage(LogMessage entry)
        {
            Level = entry.Level;
            Provider = entry.Provider;
            TimeBtwGW = entry.TimeBtwGW;
            EventID = entry.EventID;
            Task = entry.Task;
            EventData = entry.EventData;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(Provider);
            str.Append(";");
            str.Append(TimeBtwGW);
            str.Append(";");
            str.Append(EventID);
            str.Append(";");
            str.Append(Task);
            str.Append(";");
            str.Append(EventData);
            str.Append(";");

            str.Replace("False", "0");
            str.Replace("True", "1");

            return str.ToString();
        }
    }
}
