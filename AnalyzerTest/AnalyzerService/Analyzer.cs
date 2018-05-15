using AnalyzerLibrary;
using AnalyzerServ.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyzerService
{
    [ServiceBehaviorAttribute(InstanceContextMode = InstanceContextMode.PerSession)]
    public class Analyzer : IAnalyzer
    {
        const char DELIMETER = ';';
        SimpleClassifierNN netClassifier;
        LogClassifier hostClassifier;
        SimpleClassifierNN temporaryNetClassifier;
        LogClassifier temporaryHostClassifier;
        string networkFile = @"E:\Диплом\Прога\AnalyzerTest\netSave.txt";
        string logClassifierFile = @"E:\Диплом\Прога\AnalyzerTest\logSave.txt";
        string dictionaryFile = @"E:\Диплом\Прога\AnalyzerTest\SecurityTraining.txt";
        string temporaryGoal;
        Queue<string[]> netQueue;
        Queue<string[]> hostQueue;
        object lockerNet = new object();
        object lockerHost = new object();
        Thread ThreadForNetAnalyzing;
        Thread ThreadForHostAnalyzing;

        public void StartService()
        {
            netQueue = new Queue<string[]>();
            hostQueue = new Queue<string[]>();
            temporaryGoal = "";
            netClassifier = new SimpleClassifierNN(networkFile);
            hostClassifier = new LogClassifier(logClassifierFile, dictionaryFile);
            ThreadForNetAnalyzing = new Thread(NetAnalyze);
            ThreadForHostAnalyzing = new Thread(HostAnalyze);
            ThreadForHostAnalyzing.Start();
            ThreadForNetAnalyzing.Start();
        }

        public void CheckNetPackets(string[] packets)
        {
            lock(lockerNet)
            {
                netQueue.Enqueue(packets);
            }
        }

        public void NetAnalyze()
        {
            while(true)
            {
                if(netQueue.Count > 0)
                {
                    string[] packets;
                    lock (lockerNet)
                    {
                        packets = netQueue.Dequeue();
                    }

                    foreach (string packet in packets)
                    {
                        string[] packetInfoArray = packet.Split('|');
                        double[] input = Utilities.ParseToNetUnit(packetInfoArray[4], DELIMETER);
                        if (!netClassifier.Classify(input))
                            OperationContext.Current.GetCallbackChannel<IAnalyzerCallback>().GenerateNetWarning(packetInfoArray);
                    }
                }
            }
        }

        public void CheckHostPackets(string[] packets)
        {
            lock (lockerHost)
            {
                hostQueue.Enqueue(packets);
            }
        }

        public void HostAnalyze()
        {
            while (true)
            {
                if (hostQueue.Count > 0)
                {
                    string[] packets;
                    lock (lockerHost)
                    {
                        packets = hostQueue.Dequeue();
                    }

                    foreach (string str in packets)
                    {
                        LogEntry newEntry = new LogEntry(str, DELIMETER);

                        if (!hostClassifier.Analyze(newEntry))
                            OperationContext.Current.GetCallbackChannel<IAnalyzerCallback>().GenerateHostWarning(newEntry.ToStringArray());
                    }
                }
            }
        }

        public void Stop()
        {
            netClassifier = null;
            hostClassifier = null;
            OperationContext.Current.GetCallbackChannel<IAnalyzerCallback>().GoToArchiveMode();
        }

        public double[] CreateNewNN(string trainingFileName, string testFileName, string goal, int epochCount, int neuronCountInHiddenLayer)
        {
            List<double> results = new List<double>();
            int trainingSamples = 0, countFeatures = 0, testSamples = 0, testGood = 0, testBad = 0, TP = 0, TN = 0, FP = 0, FN = 0;
            temporaryGoal = goal;

            if (goal == "NET")
            {
                temporaryNetClassifier = null;
                var trainingData = Utilities.ReadNetTrainingFile(trainingFileName, ref trainingSamples, ref countFeatures);
                temporaryNetClassifier = new SimpleClassifierNN(trainingData, countFeatures, trainingSamples, neuronCountInHiddenLayer, epochCount);
                var trainingResult = temporaryNetClassifier.Train();
                var testData = Utilities.ReadNetTestFile(testFileName, ref testSamples);

                foreach (var unitTest in testData)
                {
                    bool classifyResult = temporaryNetClassifier.Classify(unitTest.Item1);

                    if (unitTest.Item2)
                    {
                        testGood++;

                        if (unitTest.Item2 != classifyResult)
                            FN++;
                        else
                            TP++;
                    }
                    else
                    {
                        testBad++;

                        if (unitTest.Item2 != classifyResult)
                            FP++;
                        else
                            TN++;
                    }
                }
            }

            if(goal == "HOST")
            {
                temporaryHostClassifier = null;
                temporaryHostClassifier = new LogClassifier(trainingFileName, neuronCountInHiddenLayer, epochCount);
                var testData = Utilities.ReadHostFile(testFileName, ref testSamples);

                for(int i = 0; i < testData.Item1.Length; i++)
                {
                    bool classifyResult = temporaryHostClassifier.TestAnalyze(testData.Item1[i]);

                    if (testData.Item2[i])
                    {
                        testGood++;

                        if (testData.Item2[i] != classifyResult)
                            FN++;
                        else
                            TP++;
                    }
                    else
                    {
                        testBad++;

                        if (testData.Item2[i] != classifyResult)
                            FP++;
                        else
                            TN++;
                    }
                }
            }

            double precision = (double)TP / (double)(TP + FP);
            double recall = (double)TP / (double)(TP + FN);
            double accuracy = 2 * ((double)(precision * recall) / (double)(precision + recall));
            double firstMistake = (double)FP / (double)testSamples;
            double secondMistake = (double)FN / (double)testSamples;

            results.Add(trainingSamples);
            results.Add(testGood);
            results.Add(testBad);
            results.Add(testSamples);
            results.Add(epochCount);
            results.Add(accuracy);
            results.Add(firstMistake);
            results.Add(secondMistake);

            return results.ToArray();
         }

        public void  ChangeNN()
        {
            if (netClassifier != null && temporaryGoal == "NET")
            {
                ThreadForNetAnalyzing.Sleep(1000);
                netClassifier = temporaryNetClassifier;
                temporaryNetClassifier = null;
            }

            if (hostClassifier != null && temporaryGoal == "HOST")
            {
                hostClassifier = temporaryHostClassifier;
                temporaryHostClassifier = null;
            }

            OperationContext.Current.GetCallbackChannel<IAnalyzerCallback>().ResumeAnalyze();
        }
    }
}