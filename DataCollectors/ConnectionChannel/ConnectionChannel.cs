using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectionChannelLib
{
    public class ConnectionChannel
    {
        int i;
        TcpClient client;
        NetworkStream stream;
        byte[] lengthOfData = new byte[4];
        public string Address { get; set; }

        public int Port { get; set; }

        public ConnectionChannel()
        {
            Address = "127.0.0.1";
            Port = 8888;
            client = new TcpClient(Address, Port);
            StartReceivingMessages();
        }

        public ConnectionChannel(int port, string address = "127.0.0.1")
        {
            Address = address;
            Port = port;
            client = new TcpClient(Address, Port);
            StartReceivingMessages();
        }   
     
        public void StartReceivingMessages()
        {
            stream = client.GetStream();
            new Thread(() =>
            {
                while ((i = stream.Read(lengthOfData, 0, 4)) != 0)
                {
                    byte[] message = new byte[BitConverter.ToInt32(lengthOfData, 0)];
                    stream.Read(message, 0, message.Length);
                    string strMessage = Encoding.Default.GetString(message);
                    Console.WriteLine(strMessage);
                }
            }).Start(); 
        }

        public void Stop()
        {
            if (client != null && client.Connected)
            {
                stream.Close();
                client.Close();
            }
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
