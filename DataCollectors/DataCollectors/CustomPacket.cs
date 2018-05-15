using PcapDotNet.Packets;
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
        public int ip_lenght { get; set; }

        [DataMember]
        public int ip_header_lenght { get; set; }

        [DataMember]
        public int ip_total_lenght { get; set; }

        [DataMember]
        public byte ip_ttl { get; set; }

        [DataMember]
        public byte ip_header_version { get; set; }

        [DataMember]
        public byte ip_service_type { get; set; }

        [DataMember]
        public ushort udp_check_sum { get; set; }

        [DataMember]
        public ushort udp_dst_port { get; set; }

        [DataMember]
        public ushort udp_scr_port { get; set; }

        [DataMember]
        public ushort udp_total_length { get; set; }

        [DataMember]
        public bool udp_cs_is_optional { get; set; }

        [DataMember]
        public bool udp_is_valid { get; set; }

        [DataMember]
        public ushort tcp_check_sum { get; set; }

        [DataMember]
        public ushort tcp_dst_port { get; set; }

        [DataMember]
        public ushort tcp_scr_port { get; set; }

        [DataMember]
        public ushort tcp_total_length { get; set; }

        [DataMember]
        public bool tcp_cs_is_optional { get; set; }

        [DataMember]
        public bool tcp_is_valid { get; set; }

        [DataMember]
        public int tcp_header_lenght { get; set; }

        [DataMember]
        public bool tcp_is_ack { get; set; }

        [DataMember]
        public bool tcp_is_cw_reduced { get; set; }
      
        [DataMember]
        public int tcp_lenght { get; set; }

        [DataMember]
        public bool tcp_is_urgent { get; set; }

        [DataMember]
        public bool tcp_is_syn { get; set; }

        [DataMember]
        public bool tcp_is_reset { get; set; }

        [DataMember]
        public bool tcp_is_push { get; set; }

        [DataMember]
        public bool tcp_is_fin { get; set; }

        [DataMember]
        public bool tcp_is_nonce_sum { get; set; }

        [DataMember]
        public bool tcp_is_ecne { get; set; }

        [DataMember]
        public ushort tcp_upointer { get; set; }

        [DataMember]
        public ushort tcp_window { get; set; }

        [DataMember]
        public bool http_is_request { get; set; }

        [DataMember]
        public bool http_is_response{ get; set; }

        //[DataMember]
        //public int http_header_bytes { get; set; }

        public CustomPacket(Packet packet)
        {
            IpV4Datagram ip = packet.Ethernet.IpV4;
            UdpDatagram udp = ip.Udp;
            TcpDatagram tcp = ip.Tcp;            

            ip_lenght = ip.Length;
            ip_header_lenght = ip.RealHeaderLength;
            ip_total_lenght = ip.TotalLength;
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
            tcp_header_lenght = tcp.HeaderLength;
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
            tcp_lenght = tcp.Length;
            tcp_upointer = tcp.UrgentPointer;
            tcp_window = tcp.Window;

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
        }

            public override string ToString()
            {
                StringBuilder str = new StringBuilder();
                
                str.Append(ip_lenght);
                str.Append(";");
                str.Append(ip_header_lenght);
                str.Append(";");
                str.Append(ip_total_lenght);
                str.Append(";");
                str.Append(ip_ttl);
                str.Append(";");
                str.Append(ip_service_type);
                str.Append(";");
                str.Append(ip_header_version);
                str.Append(";");
                str.Append(udp_check_sum );
                str.Append(";");
                str.Append(udp_dst_port);
                str.Append(";");
                str.Append(udp_cs_is_optional);
                str.Append(";");
                str.Append(udp_is_valid);
                str.Append(";");
                str.Append(udp_scr_port);
                str.Append(";");
                str.Append(udp_total_length);
                str.Append(";");
                str.Append(tcp_check_sum);
                str.Append(";");
                str.Append(tcp_dst_port);
                str.Append(";");
                str.Append(tcp_header_lenght);
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
                str.Append(tcp_lenght);
                str.Append(";");
                str.Append(tcp_upointer);
                str.Append(";");
                str.Append(tcp_window);
                str.Append(";");
                str.Append(http_is_request);
                str.Append(";");
                str.Append(http_is_response);
                str.Append(";");
                //str.Append(http_header_bytes);
                //str.Append(";");

                str.Replace("False", "0");
                str.Replace("True", "1");

                return str.ToString();
            }
            
     }    
}
