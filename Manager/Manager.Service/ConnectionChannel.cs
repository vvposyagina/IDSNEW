using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Service
{
    public delegate void QueueChanged();
    public class ConnectionChannel
    {
        public event QueueChanged newMessageAdded = null;
        public TcpListener listener;
        public NetworkStream stream;
        public TcpClient client;
        public List<Thread> allThreads;
        public byte[] lengthOfData = new byte[4];
        public Queue<string> messageBuffer;
        int i;

        public ConnectionChannel()
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            messageBuffer = new Queue<string>();
        }
        public ConnectionChannel(int port, string address = "127.0.0.1")
        {
            listener = new TcpListener(IPAddress.Parse(address), port);
            messageBuffer = new Queue<string>();            
        }
        public void Start()
        {
            allThreads = new List<Thread>();
            listener.Start();
            Thread nt = new Thread(() =>
            {
                client = listener.AcceptTcpClient();

                if (client.Connected)
                {
                    StartReceivingMessages();
                }
            });

            nt.Start();
            allThreads.Add(nt);
        }
        public void Restart()
        {
            Stop();
            Start();
        }
        private void StartReceivingMessages()
        {
            stream = client.GetStream();

            Thread nt = new Thread(() =>
            {
                try
                {
                    while ((i = stream.Read(lengthOfData, 0, 4)) != 0)
                    {
                        byte[] message = new byte[BitConverter.ToInt32(lengthOfData, 0)];
                        stream.Read(message, 0, message.Length);
                        string strMessage = Encoding.Default.GetString(message);
                        messageBuffer.Enqueue(strMessage);

                        if (newMessageAdded != null)
                            newMessageAdded();
                    }
                }
                catch
                {
                    Restart();
                }
            });         

            nt.Start();
            allThreads.Add(nt);
        }
        public void Stop()
        {
            if (client != null && client.Connected)
            {
                stream.Close();
                client.Close();
            }

            allThreads.Clear();
            listener.Stop();
        }

        public void SendMessage(string message)
        {
            stream = client.GetStream();
            byte[] data = Encoding.Default.GetBytes(message);
            int length = data.Length;
            byte[] dl = new byte[4];
            dl = BitConverter.GetBytes(length);
            stream.Write(dl, 0, 4);
            stream.Write(data, 0, data.Length);
        }     
    }
}
