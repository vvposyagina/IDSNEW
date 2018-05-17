using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostDataCollector
{
    public static class SuspiciousLogGenerator
    {
        static string rulesFile = @"E:\Диплом\WorkingDirectory\RulesFile.txt";
        static Random rand = new Random();
        static long[] badCodes = {4624, 4672, 4723, 4717, 4738, 4688};

        private static List<Tuple<HashSet<long>, HashSet<string>>> GetSignaturesDictionary(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                List<Tuple<HashSet<long>, HashSet<string>>> data = new List<Tuple<HashSet<long>, HashSet<string>>>();

                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    int i = 0;
                    while ((line = file.ReadLine()) != null && line != "")
                    {
                        string[] temp = line.Split(':');
                        string[] codes = temp[0].Split(',');
                        string[] words = temp[1].Split(',');
                        HashSet<long> codesSet = new HashSet<long>();
                        HashSet<string> wordsSet = new HashSet<string>();

                        foreach (string str in codes)
                        {
                            codesSet.Add(Convert.ToInt64(str));
                        }

                        foreach (string str in words)
                        {
                            wordsSet.Add(str);
                        }

                        data.Add(Tuple.Create(codesSet, wordsSet));
                        i++;
                    }
                }

                if (data.Count() != 0)
                    return data;
                else
                    return null;
            }
            return null;
        }
        public static LogMessage GenerateSample(LogMessage entry)
        {
            var dictionary = GetSignaturesDictionary(rulesFile);

            LogMessage newEntry = new LogMessage(entry);

            foreach (var codes in dictionary)
            {
                if (codes.Item1.Contains(newEntry.EventID))
                {
                    foreach (var word in codes.Item2)
                    {
                        if (newEntry.EventData.Contains(word))
                        {
                            newEntry.EventData.Replace(word, GenerateRandomString(7));
                        }
                    }
                }
            }
            newEntry.EventID = badCodes[rand.Next(6)];

            return newEntry;
        }

        public static string GenerateRandomString(int length)
        {
            StringBuilder str = new StringBuilder();
            for(int i = 0; i < length; i++)
            {
                int temp = rand.Next(26) + 65;
                str.Append(Convert.ToChar(temp));
            }
            return str.ToString();
        }
    }
}
