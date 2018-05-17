using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Contract
{
    public class Collector
    {
        public delegate void ProccesBufffer(string[] data);
        public string FileDirectory { get; set; }

        public string FileName { get; set; }

        protected Random rand;

        protected void SendBufferToManager(string[] data)
        {
            if (sendToManager != null)
                sendToManager(data);
        }

        public event ProccesBufffer sendToManager;

        protected bool Pause {get; set;}

        protected void SendBufferToFile(string[] data)
        {
            DirectoryInfo directory = new DirectoryInfo(FileDirectory);

            if (!directory.Exists)
            {
                directory.Create();
            }

            string path = String.Format(@"{0}//{1}", FileDirectory, FileName);

            using (StreamWriter writer = File.AppendText(path))
            {
                foreach (string str in data)
                {
                    writer.WriteLine(str.ToString());
                }
            }
        }
        protected void SendBufferToFileAndManager(string[] data)
        {
            SendBufferToManager(data);
            SendBufferToFile(data);
        }

        public void SetPause()
        {
            Pause = true;
        }

        public void ResetPause()
        {
            Pause = false;
        }
    }
}
