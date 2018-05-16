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
using System.IO;

namespace AnalyzerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int ecount = 0, fcount = 0;
            double[,] sampling = Utilities.ReadNetTrainingFile(@"E:\Диплом\WorkingDirectory\training.txt", ref ecount, ref fcount);

            SimpleClassifierNN classifier = new SimpleClassifierNN(sampling, 36, 6000, 10, 1500);
            classifier.Train();
            classifier.SaveNetwork(String.Format(@"E:\Диплом\WorkingDirectory\netSave.txt", Directory.GetCurrentDirectory()));
            int i = 0;
            using (System.IO.StreamReader file = new System.IO.StreamReader(@"E:\Диплом\WorkingDirectory\test1.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null && line != "")
                {
                    string[] allNetEntry = line.Split('|');
                    double[] input = Utilities.ParseToNetUnit(allNetEntry[4], ';');
                    string desc = "";
                    Console.WriteLine(String.Format("Result{0}: {1}", i, classifier.Classify(input).ToString()));
                    i++;
                }
            }

            string infile = @"E:\Диплом\WorkingDirectory\SecurityTraining.txt";
            string outfile = @"E:\Диплом\WorkingDirectory\logOutput.txt";
            string testfile = @"E:\Диплом\WorkingDirectory\logTest.txt";
            string saveFile = @"E:\Диплом\WorkingDirectory\logSave.txt";

            LogClassifier logcl = new LogClassifier(saveFile, infile);
            var test = Utilities.ReadHostClassifyFile(testfile);
            logcl.SaveClassifier(saveFile);
            CheckHostPackets(test[0], logcl);
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