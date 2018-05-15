using PcapDotNet.Packets;
using PcapDotNet.Packets.Icmp;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NetDataCollector
{   
    [DataContract]
    public class CustomPacket
    {
        [DataMember]
        public int PacketLength { get; set; }

        [DataMember]
        public string DateTime { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string Destination { get; set; }

        [DataMember]
        public string Data { get; set; }

        private int ip_protocol { get; set; }

        private int ip_length { get; set; }

        private int ip_header_length { get; set; }

        private int ip_total_length { get; set; }
        
        private byte ip_ttl { get; set; }

        private byte ip_header_version { get; set; }

        private byte ip_service_type { get; set; }

        private ushort udp_check_sum { get; set; }

        private ushort udp_dst_port { get; set; }

        private ushort udp_scr_port { get; set; }

        private ushort udp_total_length { get; set; }

        private bool udp_cs_is_optional { get; set; }

        private bool udp_is_valid { get; set; }

        private ushort tcp_check_sum { get; set; }

        private ushort tcp_dst_port { get; set; }

        private ushort tcp_scr_port { get; set; }

        private ushort tcp_total_length { get; set; }

        private bool tcp_cs_is_optional { get; set; }

        private bool tcp_is_valid { get; set; }

        private int tcp_header_length { get; set; }

        private bool tcp_is_ack { get; set; }

        private bool tcp_is_cw_reduced { get; set; }

        private int tcp_length { get; set; }

        private bool tcp_is_urgent { get; set; }

        private bool tcp_is_syn { get; set; }

        private bool tcp_is_reset { get; set; }

        private bool tcp_is_push { get; set; }

        private bool tcp_is_fin { get; set; }

        private bool tcp_is_nonce_sum { get; set; }

        private bool tcp_is_ecne { get; set; }

        private ushort tcp_upointer { get; set; }

        private ushort tcp_window { get; set; }

        private bool http_is_request { get; set; }

        private bool http_is_response { get; set; }

        private int icmp_length { get; set; }

        private bool icmp_is_valid { get; set; }

        private bool icmp_check_sum { get; set; }

        public CustomPacket(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;
            TcpDatagram tcp = ip.Tcp;
            IcmpDatagram icmp = ip.Icmp;

            DateTime = packet.Timestamp.ToString("yyyy-MM-dd hh:mm:ss.fff");
           
            PacketLength = packet.Length;
            string str = ip.Protocol.ToString();
            switch(str)
            {
                case "Tcp":
                    ip_protocol = 1;
                    break;
                case "Udp":
                    ip_protocol = 2;
                    break;
                case "Icmp":
                    ip_protocol = 3;
                    break;
                case "Igmp":
                    ip_protocol = 4;
                    break;
                default:
                    ip_protocol = 0;
                    break;
            }

            Source = ip.Source.ToString();
            Destination = ip.Destination.ToString();

            ip_length = ip.Length;
            ip_header_length = ip.RealHeaderLength;
            ip_total_length = ip.TotalLength;
            ip_ttl = ip.Ttl;
            ip_service_type = ip.TypeOfService;
            ip_header_version = ip.Version;

            udp_check_sum = udp.Checksum;
            udp_dst_port = udp.DestinationPort;
            udp_cs_is_optional = udp.IsChecksumOptional;
            udp_is_valid = udp.IsValid;
            udp_scr_port = udp.SourcePort;
            udp_total_length = udp.TotalLength;

            tcp_check_sum = tcp.Checksum;
            tcp_dst_port = tcp.DestinationPort;
            tcp_header_length = tcp.HeaderLength;
            tcp_is_ack = tcp.IsAcknowledgment;
            tcp_cs_is_optional = tcp.IsChecksumOptional;
            tcp_is_cw_reduced = tcp.IsCongestionWindowReduced;
            tcp_is_ecne = tcp.IsExplicitCongestionNotificationEcho;
            tcp_is_nonce_sum = tcp.IsNonceSum;
            tcp_is_fin = tcp.IsFin;
            tcp_is_push = tcp.IsPush;
            tcp_is_reset = tcp.IsReset;
            tcp_is_syn = tcp.IsSynchronize;
            tcp_is_urgent = tcp.IsUrgent;
            tcp_is_valid = tcp.IsValid;
            tcp_length = tcp.Length;
            tcp_upointer = tcp.UrgentPointer;
            tcp_window = tcp.Window;

            icmp_length = icmp.Length;
            icmp_is_valid = icmp.IsValid;
            icmp_check_sum = icmp.IsChecksumCorrect;

            if (tcp.Http != null)
            {
                http_is_request = tcp.Http.IsRequest;
                http_is_response = tcp.Http.IsResponse;
            }
            else
            {
                http_is_request = false;
                http_is_response = false;
            }

            Data = CreateData();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.Append(DateTime);
            str.Append("|");
            str.Append(Source);
            str.Append("|");
            str.Append(Destination);
            str.Append("|");
            str.Append(PacketLength);
            str.Append("|");
            str.Append(Data);

            return str.ToString();
        }
        public string CreateData()
        {
            StringBuilder str = new StringBuilder();

            str.Append(PacketLength);
            str.Append(";");
            str.Append(ip_protocol);
            str.Append(";");
            str.Append(ip_length);
            str.Append(";");
            str.Append(ip_header_length);
            str.Append(";");
            str.Append(ip_total_length);
            str.Append(";");
            str.Append(ip_ttl);
            str.Append(";");
            str.Append(ip_header_version);
            str.Append(";");
            str.Append(udp_check_sum);
            str.Append(";");
            str.Append(udp_dst_port);
            str.Append(";");
            str.Append(udp_scr_port);
            str.Append(";");
            str.Append(udp_total_length);
            str.Append(";");
            str.Append(tcp_check_sum);
            str.Append(";");
            str.Append(tcp_dst_port);
            str.Append(";");
            str.Append(tcp_header_length);
            str.Append(";"); 
            str.Append(tcp_length);
            str.Append(";");
            str.Append(tcp_window);
            str.Append(";");
            str.Append(icmp_length);
            str.Append(";");
            str.Append(ip_service_type);
            str.Append(";");
            str.Append(udp_cs_is_optional);
            str.Append(";");
            str.Append(udp_is_valid);
            str.Append(";");
            str.Append(tcp_is_ack);
            str.Append(";");
            str.Append(tcp_cs_is_optional);
            str.Append(";");
            str.Append(tcp_is_cw_reduced);
            str.Append(";");
            str.Append(tcp_is_ecne);
            str.Append(";");
            str.Append(tcp_is_nonce_sum);
            str.Append(";");
            str.Append(tcp_is_fin);
            str.Append(";");
            str.Append(tcp_is_push);
            str.Append(";");
            str.Append(tcp_is_reset);
            str.Append(";");
            str.Append(tcp_is_syn);
            str.Append(";");
            str.Append(tcp_is_urgent);
            str.Append(";");
            str.Append(tcp_is_valid);
            str.Append(";");
            str.Append(tcp_upointer);
            str.Append(";");
            str.Append(http_is_request);
            str.Append(";");
            str.Append(http_is_response);
            str.Append(";");
            str.Append(icmp_is_valid);
            str.Append(";");
            str.Append(icmp_check_sum);
            str.Append(";");

            str.Replace("False", "0");
            str.Replace("True", "1");

            return str.ToString();
        }

    }
}
