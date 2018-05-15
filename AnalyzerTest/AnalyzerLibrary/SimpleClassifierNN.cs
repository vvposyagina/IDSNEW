using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnalyzerLibrary
{
    public class SimpleClassifierNN
    {
        ActivationNetwork Network { get; set; }
        public double[,] SourceMatrix { get; set; }
        public double LearningRate { get; set; }
        public double SigmoidAlphaValue { get; set; }
        public double ThresholdValue { get; set; }
        public int NeuronsInFirstLayer { get; set; }
        public int NeuronsInHiddenLayer { get; set; }
        public int EpochCount { get; set; }
        public bool UseRegularization { get; set; }
        public bool UseNguyenWidrow { get; set; }
        public bool UseSameWeights { get; set; }
        public int Samples { get; set; }
        public int FeaturesAmount { get; set; }

        TimeSpan elapsed = TimeSpan.Zero;

        public SimpleClassifierNN(double[,] sourceMatrix, int featuresAmount, int samples, int countNeuronsHiddinLayer, int epochCount)
        {
            SourceMatrix = sourceMatrix;
            FeaturesAmount = featuresAmount;
            ThresholdValue = 0.5;
            LearningRate = 0.5;
            SigmoidAlphaValue = 0.5;
            EpochCount = epochCount;
            Samples = samples;
            NeuronsInFirstLayer = featuresAmount;
            NeuronsInHiddenLayer = countNeuronsHiddinLayer;
            UseRegularization = true;
            UseNguyenWidrow = true;
            UseSameWeights = false;
        }

        public SimpleClassifierNN(string fileName)
        {
            Network = (ActivationNetwork)Accord.Neuro.Network.Load(fileName);
        }

        public SimpleClassifierNN(double[][] inputs, double[][] outputs, int samples, int countNeuronsHiddinLayer, int epochCount)
        {
            ThresholdValue = 0;
            LearningRate = 0.3;
            SigmoidAlphaValue = 0.3;
            EpochCount = epochCount;
            Samples = samples;
            NeuronsInFirstLayer = inputs[0].Count();
            NeuronsInHiddenLayer = countNeuronsHiddinLayer;
        }

        public Tuple<double, TimeSpan> Train()
        {
            List<int> listColumns = new List<int>();
            for (int i = 0; i < FeaturesAmount; i++)
                listColumns.Add(i);

            double[][] inputs = SourceMatrix.GetColumns(listColumns.ToArray()).ToJagged();
            double[][] outputs = SourceMatrix.GetColumn(FeaturesAmount).Transpose().ToJagged();

            Network = new ActivationNetwork(new BipolarSigmoidFunction(SigmoidAlphaValue), NeuronsInFirstLayer, NeuronsInHiddenLayer, 1);

            BackPropagationLearning teacher = new BackPropagationLearning(Network);
            teacher.LearningRate = LearningRate;

            Network.Randomize();

            var sw = Stopwatch.StartNew();
            double error = double.PositiveInfinity;
            double previous;

            for (int i = 1; i <= EpochCount; i++)
            {
                previous = error;


                error = teacher.RunEpoch(inputs, outputs);

                if (error == 0)
                    break;

                //Console.WriteLine(String.Format("Epoch={0} Error={1}", i, error));
            }
            sw.Stop();

            return Tuple.Create(error, sw.Elapsed);
        }

        public Tuple<double, TimeSpan> Train(double[][] inputs, double[][] outputs)
        {
            Network = new ActivationNetwork(new BipolarSigmoidFunction(SigmoidAlphaValue), NeuronsInFirstLayer, NeuronsInHiddenLayer, 1);

            BackPropagationLearning teacher = new BackPropagationLearning(Network);
            teacher.LearningRate = LearningRate;

            Network.Randomize();

            var sw = Stopwatch.StartNew();
            double error = double.PositiveInfinity;
            double previous = double.PositiveInfinity;

            for (int i = 1; i <= EpochCount; i++)
            {
                error = teacher.RunEpoch(inputs, outputs);

                if (error == 0 || previous < error)
                    break;

                previous = error;

               // Console.WriteLine(String.Format("Epoch={0} Error={1}", i, error));
            }

            sw.Stop();

            return Tuple.Create(error, sw.Elapsed);
        }

        public void SaveNetwork(string fileName)
        {
            if (Network != null)
            {
                Network.Save(fileName);
            }
        }

        public bool Classify(double[] sample)
        {
            double[] result = Network.Compute(sample);

            //Console.WriteLine("Result = " + result[0]);

            bool total = result[0] > ThresholdValue ? true : false;

            return total;
        }
    }
}