using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterface
{
    public static class Utilities
    {
        public static bool CheckFilter(string filter)
        {
            return true;
        }
      
        public static Dictionary<string, string> ParseDevicesList(string[] list)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach(string item in list)
            {
                string[] pair = item.Split(';');
                result.Add(pair[0], pair[1]);
            }

            return result;
        }
        
        public static string ParseDeviceTitle(string title)
        {
            string result = title.Substring(title.IndexOf('(') + 1);
            result = result.Remove(result.IndexOf(')'));
            return result;
        }

        public static string GetNetReport(string[] data)
        {
            StringBuilder info = new StringBuilder();
            
            info.Append(String.Format("DateTime: {0}\r\n", data[0]));
            info.Append(String.Format("Source: {0}\r\n", data[1]));
            info.Append(String.Format("Destination: {0}\r\n", data[2]));
            info.Append(String.Format("Length: {0}\r\n", data[3]));
            info.Append(String.Format("Data:\r\n"));
            info.Append(ParseNetData(data[4]));

            return info.ToString();
        }

        public static string GetHostReport(string[] data)
        {
            StringBuilder info = new StringBuilder();
            info.Append(String.Format("DateTime: {0}\r\n", data[0]));
            info.Append(String.Format("EventID: {0}\r\n", data[1]));
            info.Append(String.Format("Provider: {0}\r\n", data[2]));
            info.Append(String.Format("Data:\r\n"));
            info.Append(ParseNetData(data[3]));

            return info.ToString();
        }

        public static string ParseNetData(string data)
        {
            StringBuilder str = new StringBuilder();

            str.Append(String.Format("PacketLength: {0}\r\n", data[0]));

            str.Append(String.Format("ip_protocol: {0}\r\n", data[1]));

            str.Append(String.Format("ip_length: {0}\r\n", data[2]));

            str.Append(String.Format("ip_header_length: {0}\r\n", data[3]));

            str.Append(String.Format("ip_total_length: {0}\r\n", data[4]));

            str.Append(String.Format("ip_ttl: {0}\r\n", data[5]));

            str.Append(String.Format("ip_header_version: {0}\r\n", data[6]));

            str.Append(String.Format("udp_check_sum: {0}\r\n", data[7]));

            str.Append(String.Format("udp_dst_port: {0}\r\n", data[8]));

            str.Append(String.Format("udp_scr_port: {0}\r\n", data[9]));

            str.Append(String.Format("udp_total_length: {0}\r\n", data[10]));

            str.Append(String.Format("tcp_check_sum: {0}\r\n", data[11]));

            str.Append(String.Format("tcp_dst_port: {0}\r\n", data[12]));

            str.Append(String.Format("tcp_header_length: {0}\r\n", data[13]));

            str.Append(String.Format("tcp_length: {0}\r\n", data[14]));

            str.Append(String.Format("tcp_window: {0}\r\n", data[15]));

            str.Append(String.Format("icmp_length: {0}\r\n", data[16]));

            str.Append(String.Format("ip_service_type: {0}\r\n", data[17]));

            str.Append(String.Format("udp_cs_is_optional: {0}\r\n", data[18]));

            str.Append(String.Format("udp_is_valid: {0}\r\n", data[19]));

            str.Append(String.Format("tcp_is_ack: {0}\r\n", data[20]));

            str.Append(String.Format("tcp_cs_is_optional: {0}\r\n", data[21]));

            str.Append(String.Format("tcp_is_cw_reduced: {0}\r\n", data[22]));

            str.Append(String.Format("tcp_is_ecne: {0}\r\n", data[23]));

            str.Append(String.Format("tcp_is_nonce_sum: {0}\r\n", data[24]));

            str.Append(String.Format("tcp_is_fin: {0}\r\n", data[25]));

            str.Append(String.Format("tcp_is_push: {0}\r\n", data[26]));

            str.Append(String.Format("tcp_is_reset: {0}\r\n", data[27]));

            str.Append(String.Format("tcp_is_syn: {0}\r\n", data[28]));

            str.Append(String.Format("tcp_is_urgent: {0}\r\n", data[29]));

            str.Append(String.Format("tcp_is_valid: {0}\r\n", data[30]));

            str.Append(String.Format("tcp_upointer: {0}\r\n", data[31]));

            str.Append(String.Format("http_is_request: {0}\r\n", data[32]));

            str.Append(String.Format("http_is_response: {0}\r\n", data[33]));

            str.Append(String.Format("icmp_is_valid: {0}\r\n", data[34]));

            str.Append(String.Format("icmp_check_sum: {0}\r\n", data[35]));      

            return str.ToString();
        }

    }
}
