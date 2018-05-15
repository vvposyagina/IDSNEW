using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics.Models.Regression.Linear;
using Accord.Statistics.Analysis;
using Accord.IO;
using Accord.Math;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;
using Accord.Neuro;
using System.Threading;
using Accord.Neuro.Learning;
using System.Diagnostics;
using System.ServiceModel;
using AnalyzerLibrary;

namespace AnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //double[,] sampling = Utilities.ReadNetTrainingFile(@"E:\Диплом\Прога\AnalyzerTest\training.txt");

            //SimpleClassifierNN classifier = new SimpleClassifierNN(sampling, 36, 6000, 10, 1500);
            //classifier.Train();
            //classifier.SaveNetwork(@"E:\Диплом\Прога\AnalyzerTest\netSave.txt");
            //int i =0;
            //using (System.IO.StreamReader file = new System.IO.StreamReader(@"E:\Диплом\Прога\AnalyzerTest\test1.txt"))
            //{
            //    string line;
            //    while ((line = file.ReadLine()) != null && line != "")
            //    {
            //        string[] allNetEntry = line.Split('|');
            //        double[] input = Utilities.ParseToNetUnit(allNetEntry[4], ';');
            //        string desc = "";
            //        Console.WriteLine(String.Format("Result{0}: {1}", i, classifier.Classify(input).ToString()));
            //        i++;
            //    }
            //}

            //string infile = @"E:\Диплом\Прога\AnalyzerTest\SecurityTraining.txt";
            //string outfile = @"E:\Диплом\Прога\AnalyzerTest\logOutput.txt";
            //string testfile = @"E:\Диплом\Прога\AnalyzerTest\logTest.txt";
            //string saveFile = @"E:\Диплом\Прога\AnalyzerTest\logSave.txt";

            //LogClassifier logcl = new LogClassifier(saveFile, infile);
            //string[] test = Utilities.ReadTestLogsFromFile(testfile);
            //logcl.SaveClassifier(saveFile);
            //CheckHostPackets(test, logcl);
        }

        public static void CheckHostPackets(string[] packets, LogClassifier lc)
        {
            int count = 0;
            foreach (string str in packets)
            {
                LogEntry newEntry = new LogEntry(str, ',');
                string description = "";
                if (lc.Analyze(newEntry))
                    Console.WriteLine("OK");
                else
                    Console.WriteLine("NOT OK");
            }
        }
    }
}