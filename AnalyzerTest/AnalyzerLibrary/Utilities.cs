using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalyzerLibrary
{
    public static class Utilities
    {
        const char DELIMETER = ';';
        const char STRDELIMETER = '@';

        //Функции для работы с сетевыми данными
        public static double[,] ReadNetTrainingFile(string fileName, ref int samples, ref int countFeatures)
        {
            if (System.IO.File.Exists(fileName))
            {
                List<double[]> dataset = new List<double[]>();
                samples = 0;

                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null && line != "")
                    {
                        string[] array = line.Split('|');
                        dataset.Add(ParseToNetUnit(array[4], DELIMETER));
                        samples++;
                    }
                }
                try
                {
                    var arr = new double[dataset.Count(), dataset[0].Count()];
                    var source = dataset.ToArray();
                    countFeatures = dataset[0].Length - 1;

                    for (int i = 0; i < dataset.Count; i++)
                    {
                        for (int j = 0; j < dataset[0].Count(); j++)
                        {
                            arr[i, j] = source[i][j];
                        }
                    }

                    return arr;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static List<Tuple<double[], bool>> ReadNetTestFile(string fileName, ref int samples)
        {
            if (System.IO.File.Exists(fileName))
            {
                List<Tuple<double[], bool>> dataset = new List<Tuple<double[], bool>>();
                samples = 0;

                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null && line != "")
                    {
                        string[] array = line.Split('|');
                        double[] darray = ParseToNetUnit(array[4], DELIMETER);
                        bool label = darray[darray.Length - 1] > 0.5 ? true : false;
                        System.Array.Resize(ref darray, darray.Length - 1);
                        dataset.Add(Tuple.Create(darray, label));
                        samples++;
                    }
                }
            
                return dataset;                
            }
            return null;
        }

        public static double[][] ReadNetClassifyFile(string fileName, ref int samples)
        {
            if (System.IO.File.Exists(fileName))
            {
                List<double[]> dataset = new List<double[]>();
                samples = 0;

                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    while ((line = file.ReadLine()) != null && line != "")
                    {
                        string[] array = line.Split('|');
                        dataset.Add(ParseToNetUnit(array[4], DELIMETER));
                        samples++;
                    }
                }

                return dataset.ToArray();
            }
            return null;
        }

        public static double[] ParseToNetUnit(string source, char delimeter)
        {
            List<double> list = new List<double>();

            source = source.Substring(0, source.Length - 1);
            string[] temp = source.Split(delimeter);

            foreach (string str in temp)
            {
                list.Add(Convert.ToDouble(str));
            }

            double last = list.Last();
            double[] result = Accord.Math.Matrix.Normalize(list.ToArray(), false);
            result[list.Count - 1] = last;
            return result;
        }

        //Функции для работы с системными данными
        public static Tuple<string[][], bool[]> ReadHostFile(string fileName, ref int samples)
        {
            if (System.IO.File.Exists(fileName))
            {
                List<string[]> list = new List<string[]>();
                List<bool> boolList = new List<bool>();
                samples = 0;

                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    if ((line = file.ReadToEnd()) != null && line != "")
                    {
                        string[] allMessages = line.Split('@');

                        foreach (string str in allMessages)
                        {
                            if (str != "")
                            {
                                bool label = str[0] == '1' ? true : false;
                                string message = str.Remove(0, 1);
                                string[] data = message.Split(' ');
                                list.Add(data);
                                boolList.Add(label);
                                samples++;
                            }
                        }
                    }
                }

                if (list.Count() != 0 && boolList.Count != 0 && list.Count() == boolList.Count())
                    return Tuple.Create(list.ToArray(), boolList.ToArray());
                else
                    return null;
            }
            return null;
        }

        public static List<string[]> ReadHostClassifyFile(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                List<string[]> dataset = new List<string[]>();

                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    string line;
                    if ((line = file.ReadToEnd()) != null && line != "")
                    {
                        string[] allMessages = line.Split('@');

                        foreach (string str in allMessages)
                        {
                            if (str != "")
                            {                               
                                string[] data = str.Split(' ');
                                dataset.Add(data);
                            }
                        }
                    }
                }

                if (dataset.Count() != 0)
                    return dataset;
                else
                    return null;
            }
            return null;
        }

        public static double[][] BoolToDouble(bool[] vector)
        {
            List<double[]> results = new List<double[]>();
            
            for(int i = 0; i < vector.Length; i++)
            {
                double[] array = new double[1];
                array[0] = vector[i] ? 1 : 0;
                results.Add(array);
            }

            return results.ToArray();
        }
    }
}