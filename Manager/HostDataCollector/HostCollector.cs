using Manager.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HostDataCollector
{
    public class HostCollector : Collector
    {
        public const int PACKETS_COUNT_CONSTRAINT = 1;
        object locker = new object();

        EventLog sourceLog;

        public string Source { get; set; }

        public Queue<LogMessage> MessagesBuffer { get; set; }

        public bool SavingInFile { get; set; }

        public event ProccesBufffer queueIsFull;

        public void Initialize(string source, bool savingfile)
        {
            FileDirectory = "E:\\Диплом\\Новая\\IDSNEW\\UserInterface\\UserInterface\\bin\\Debug";
            FileName = "HostCollectorLog.txt";
            MessagesBuffer = new Queue<LogMessage>();
            Source = source;
            SavingInFile = savingfile;
            queueIsFull = null;

            if (SavingInFile)
            {
                queueIsFull += SendBufferToFileAndManager;
            }
            else
            {
                queueIsFull += SendBufferToManager;
            }          

            if (EventLog.Exists(source))
            {
                sourceLog = new EventLog(source);
                sourceLog.EnableRaisingEvents = true;
            }
        }

        public HostCollector(string source, bool mode = true)
        {
            Initialize(source, mode);
        }

        public void Start()
        {
            sourceLog.EntryWritten += new EntryWrittenEventHandler(GetNewEntry);
        }

        public void Stop()
        {
            sourceLog.EntryWritten -= new EntryWrittenEventHandler(GetNewEntry);
        } 
        public void GetNewEntry(Object source, EntryWrittenEventArgs e)
        {            
            LogMessage newMessage = new LogMessage(e.Entry);
            MessagesBuffer.Enqueue(newMessage);

            if(MessagesBuffer.Count == PACKETS_COUNT_CONSTRAINT)
            {
                string[] data = GetLastPackets();
                if (queueIsFull != null)
                    queueIsFull(data);
            }
        }

        public string[] GetLastPackets()
        {
            string[] result = new string[PACKETS_COUNT_CONSTRAINT];
            lock (locker)
            {
                int i = 0;
                while (i < PACKETS_COUNT_CONSTRAINT)
                {
                    LogMessage message = MessagesBuffer.Dequeue();
                    result[i] = message.ToString();
                    i++;
                }
            }
            return result;
        }
    }
}
