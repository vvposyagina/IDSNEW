using ConnectionChannelLib;
using PcapDotNet.Core;
using PcapDotNet.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDataCollector
{
    public class Collector
    {
        delegate bool ProccesBufffer();
        const int COUNT_CONSTRAINT = 100;
        object locker = new object();

        public string FileDirectory { get; set; }

        public string FileName { get; set; }

        public PacketDevice CurrentDevice { get; set; }

        public string DeviceAddress { get; set; }

        public Queue<Packet> PacketBuffer { get; set; }

        public ConnectionChannel ManagerChannel { get; set; }

        public bool SensorMode { get; set; }

        Thread threadForFileOutput;

        event ProccesBufffer queueIsFull;

        private void Initialize(bool mode)
        {
            DeviceAddress = CurrentDevice.Addresses[1].Address.ToString();
            DeviceAddress = DeviceAddress.Replace("Internet ", "");
            PacketBuffer = new Queue<Packet>();
            SensorMode = mode;
            queueIsFull = null;

            if (SensorMode)
                queueIsFull += SendBufferToManager;
            else
                queueIsFull += SendBufferToFile;


            threadForFileOutput = new Thread(StartCapturing);
        }

        public Collector(bool mode = false)
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            FileDirectory = "E:\\Диплом\\Прога\\DataCollectors\\DataCollectors";
            FileName = "test.txt";
            //Manager = new ConnectionChannel();
            //Manager.StartReceivingMessages();
            if (allDevices.Count > 0)
            {
                CurrentDevice = allDevices[0];
                Initialize(mode);               
            }
            else
            {
                CurrentDevice = null;
            }
        }

        public Collector(LivePacketDevice device, bool mode = false)
        {
            CurrentDevice = device;
            Initialize(mode);
        }

        public Collector(string deviceAddress, bool mode = false)
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            CurrentDevice = allDevices.FirstOrDefault(t => t.Addresses[1].Address.ToString() == String.Format("Internet {0}", deviceAddress));
            Initialize(mode);
        }
        public void Start()
        {
            if(!SensorMode)
                threadForFileOutput.Start();
        }

        public void Stop()
        {
            if (!SensorMode)
                threadForFileOutput.Abort();
        }

        public void StartCapturing()
        {
            using (PacketCommunicator communicator = CurrentDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                Packet packet;
                do
                {
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);
                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            continue;
                        case PacketCommunicatorReceiveResult.Ok:
                            PacketBuffer.Enqueue(packet);
                            break;
                        default:
                            throw new InvalidOperationException("The result " + result + " shoudl never be reached here");
                    }

                    if (PacketBuffer.Count == 1000)
                    {
                        if (queueIsFull != null)
                            queueIsFull();                        
                    }
                } while (true);
            }
        }

        public bool SendBufferToManager()
        {
            return true;
        }

        public bool SendBufferToFile()
        {
            bool result = false;
            DirectoryInfo directory = new DirectoryInfo(FileDirectory);

            if (!directory.Exists)
            {
                directory.Create();
            }

            string path = String.Format(@"{0}//{1}", FileDirectory, FileName);

            //if (File.Exists(path))
            //{
            //    File.Delete(path);
            //    File.Create(path);
            //}

            using (StreamWriter writer = File.AppendText(path))
            {
                lock (locker)
                {
                    foreach (Packet packet in PacketBuffer)
                    {
                        CustomPacket newPacket = new CustomPacket(packet);
                        writer.WriteLine(newPacket.ToString());
                    }
                    PacketBuffer.Clear();
                    result = true;
                }               
            }
            return result;
        }

        public void CreatNewFilter()
        {
            //using (BerkeleyPacketFilter filter = communicator.CreateFilter("ip and tcp"))
            //{
            //    // Set the filter
            //    communicator.SetFilter(filter);
            //}
        }

        //public string HandlePacket(Packet packet)
        //{

        //}
    }
}
