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

        public static bool CheckSource(string filter)
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
    }
}
