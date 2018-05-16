using Manager.Contract;
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
    public class NetCollector : Collector
    {        
        public const int PACKETS_COUNT_CONSTRAINT = 50;
        Random rand;

        public string Filter { get; set; }

        public PacketDevice CurrentDevice { get; set; }

        public string DeviceAddress { get; set; }

        private Queue<string> PacketBuffer { get; set; }

        public bool SavingInFile { get; set; }

        Thread ThreadForCapturing;
        public event ProccesBufffer queueIsFull;

        private void Initialize(string filter, bool savingfile)
        {
            Pause = false;
            FileDirectory = "E:\\Диплом\\WorkingDirectory";
            FileName = "NetCollectorLog.txt";
            DeviceAddress = CurrentDevice.Addresses[1].Address.ToString();
            DeviceAddress = DeviceAddress.Replace("Internet ", "");
            PacketBuffer = new Queue<string>();
            SavingInFile = savingfile;
            rand = new Random();

            queueIsFull = null;

            if (SavingInFile)
            {
                queueIsFull += SendBufferToFileAndManager;
            }
            else
            {
                queueIsFull += SendBufferToManager;
            }

            ThreadForCapturing = new Thread(StartCapturing);

            Filter = null;

            if (filter != "")
            {
                Filter = filter;
            }
        }

        public NetCollector(string filter = "", bool mode = false)
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;            
            if (allDevices.Count > 0)
            {
                CurrentDevice = allDevices[0];
                Initialize(filter, mode);
            }
            else
            {
                CurrentDevice = null;
            }
        }

        public NetCollector(LivePacketDevice device, string filter = "", bool mode = false)
        {
            CurrentDevice = device;
            Initialize(filter, mode);
        }

        public NetCollector(string deviceName, string filter = "", bool mode = false)
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            CurrentDevice = allDevices.FirstOrDefault(t => t.Name == deviceName);
            Initialize(filter, mode);
        }

        public static List<string> GetDevicesNames()
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            List<string> result = new List<string>();

            foreach(LivePacketDevice device in allDevices)
            {
                result.Add(device.Name);
            }

            return result;
        }

        public static List<string> GetDeviceDictionary()
        {
            IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;
            List<string> result = new List<string>();

            foreach (LivePacketDevice device in allDevices)
            {
                result.Add(String.Format("{0};{1}", device.Name, device.Description));
            }

            return result;
        }

        public void Start()
        {
            ThreadForCapturing.Start();
        }

        public void Stop()
        {
            ThreadForCapturing.Abort();
        }

        private void StartCapturing()
        {
            using (PacketCommunicator communicator = CurrentDevice.Open(65536, PacketDeviceOpenAttributes.Promiscuous, 1000))
            {
                Packet packet;
                do
                {
                    PacketCommunicatorReceiveResult result = communicator.ReceivePacket(out packet);

                    if (Filter != null)
                    {
                        communicator.SetFilter(Filter);
                    }

                    switch (result)
                    {
                        case PacketCommunicatorReceiveResult.Timeout:
                            // Timeout elapsed
                            continue;
                        case PacketCommunicatorReceiveResult.Ok:
                            PacketBuffer.Enqueue((new CustomPacket(packet)).ToString());
                            //PacketBuffer.Enqueue(SuspiciousPacketGenerator.GenerateSample(36, "0", rand, false));

                            int randomNumber = rand.Next(1000);

                            if (randomNumber % 50 == 0)
                            {
                                PacketBuffer.Enqueue(SuspiciousPacketGenerator.GenerateSample(36, "0", rand, false));
                            }
                            break;
                        default:
                            throw new InvalidOperationException("The result " + result + " should never be reached here");
                    }

                    lock (PacketBuffer)
                    {
                        if (PacketBuffer.Count >= PACKETS_COUNT_CONSTRAINT && !Pause)
                        {
                            string[] data = GetLastPackets();

                            if (queueIsFull != null)
                                queueIsFull(data);
                        }
                    }

                } while (true);
            }
        }

        public string[] GetLastPackets()
        {
            string[] result = new string[PACKETS_COUNT_CONSTRAINT];
            
            int i = 0;
            while(i < PACKETS_COUNT_CONSTRAINT)
            {
                string packet = PacketBuffer.Dequeue();
                result[i] = packet;
                i++;
            }
            
            return result;
        }
    }
}
