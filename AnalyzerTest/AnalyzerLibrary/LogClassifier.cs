using Accord.MachineLearning;
using Accord.Neuro;
using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerLibrary
{
    public class LogClassifier
    {
        BagOfWords Codebook { get; set; }

        public int Samples { get; set; }

        public double Error { get; set; }

        public TimeSpan TrainingTime { get; set; }

        SimpleClassifierNN classifier;
        List<Tuple<HashSet<long>, HashSet<string>>> SignatureDictionary;
        //LogisticRegression reg;


        public LogClassifier(string fileName, int countLayers, int countEpoch)
        {
            Codebook = new BagOfWords()
            {
                MaximumOccurance = 1
            };

            int samples = 0;
            var dictionary = Utilities.ReadHostFile(fileName, ref samples);

            Samples = samples;

            if (dictionary.Item1.Length != 0 && dictionary.Item2.Length != 0)
            {
                Codebook.Learn(dictionary.Item1);
                double[][] inputs = Codebook.Transform(dictionary.Item1);
                int count = inputs.Count();

                //var learner = new IterativeReweightedLeastSquares<LogisticRegression>()
                //{
                //    Tolerance = 1e-4,  // Let's set some convergence parameters
                //    Iterations = 10,  // maximum number of iterations to perform
                //    Regularization = 0
                //};

                //reg = learner.Learn(inputs, outputs2);
                double[][] outputs = Utilities.BoolToDouble(dictionary.Item2);
                classifier = new SimpleClassifierNN(inputs, outputs, count, countLayers, countEpoch);
                var trainingResult =  classifier.Train(inputs, outputs);
                Error = trainingResult.Item1;
                TrainingTime = trainingResult.Item2;
            }
        }

        public LogClassifier(string fileName)
        {
            SignatureDictionary = Utilities.GetSignaturesDictionary(fileName);
        }

        public void SaveClassifier(string fileName)
        {
            if (classifier != null)
            {
                classifier.SaveNetwork(fileName);
            }
        }

        public LogClassifier(string networkFileName, string dictionaryFileName)
        {
            Codebook = new BagOfWords()
            {
                MaximumOccurance = 1
            };

            int samples = 0;
            var dictionary = Utilities.ReadHostFile(dictionaryFileName, ref samples);

            if (dictionary.Item1.Length != 0)
            {
                Codebook.Learn(dictionary.Item1);
            }

            classifier = new SimpleClassifierNN(networkFileName);
        }

        public bool Analyze(LogEntry entry)
        {
            double[] log = Codebook.Transform(entry.EventData);
            bool result = classifier.Classify(log);

            if (result)
                return result;

            return result;

            //bool c1 = reg.Decide(log);
            //return c1;
        }

        public bool TestAnalyze(string[] entry)
        {
            double[] log = Codebook.Transform(entry);
            bool result = classifier.Classify(log);

            if (result)
                return result;

            return result;

            //bool c1 = reg.Decide(log);
            //return c1;
        }


        public bool AnalysisOfSignatures(LogEntry entry)
        {
            bool flag = false;
            foreach(var codes in SignatureDictionary)
            {
                flag = false;
                if(codes.Item1.Contains(entry.EventID))
                {                  
                    foreach(var word in codes.Item2)
                    {
                        if(entry.Data.Contains(word))
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                        return flag;
                }
            }

            return true;
        }
    }
}