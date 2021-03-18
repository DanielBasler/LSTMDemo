using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random();
            DataSet data = new DataSetGenerator();            

            int inputDimension = 2;
            int hiddenDimension = 3;
            int outputDimension = 1;
            int hiddenLayers = 1;            
            double learningRate = 0.005;
            double initParamsStdDev = 0.08;

            Console.WriteLine("Start...");

            INeuralNetwork nn = NetworkBuilder.MakeLstm(inputDimension,
                hiddenDimension,
                hiddenLayers,
                outputDimension,
                data.GetModelOutputUnitToUse(),
                initParamsStdDev, rng);

            int reportEveryNthEpoch = 10;
            int trainingEpochs = 650;

            Training.train<NeuralNetwork>(trainingEpochs, learningRate, nn, data, reportEveryNthEpoch, rng);

            Console.WriteLine("Training Completed.");
            Console.WriteLine("Test: 1,1");

            Matrix input = new Matrix(new double[] { 1, 1 });
            Graph g = new Graph(false);
            Matrix output = nn.Activate(input, g);

            Console.WriteLine("Test: 1,1. Output:" + output.W[0]);

            Matrix input1 = new Matrix(new double[] { 0, 1 });
            Graph g1 = new Graph(false);
            Matrix output1 = nn.Activate(input1, g1);

            Console.WriteLine("Test: 0,1. Output:" + output1.W[0]);

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}
