using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class NeuralNetwork : INeuralNetwork
    {
        List<INetworkLayer> layers;
        public NeuralNetwork(List<INetworkLayer> layers)
        {
            this.layers = layers;
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            Matrix prev = input;
            foreach (INetworkLayer layer in layers)
            {
                prev = layer.Activate(prev, g);
            }
            return prev;
        }

        public void ResetState()
        {
            foreach (INetworkLayer layer in layers)
            {
                layer.ResetState();
            }
        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            foreach (INetworkLayer layer in layers)
            {
                result.AddRange(layer.GetParameters());
            }
            return result;
        }
    }

    public static class NetworkBuilder
    {
        public static NeuralNetwork MakeLstm(int inputDimension, int hiddenDimension, int hiddenLayers, int outputDimension, IForNoLinearity decoderUnit, double initParamsStdDev, Random rng)
        {
            List<INetworkLayer> layers = new List<INetworkLayer>();
            for (int h = 0; h < hiddenLayers; h++)
            {
                if (h == 0)
                {
                    layers.Add(new LstmLayer(inputDimension, hiddenDimension, initParamsStdDev, rng));
                }
                else
                {
                    layers.Add(new LstmLayer(hiddenDimension, hiddenDimension, initParamsStdDev, rng));
                }
            }
            layers.Add(new FeedForwardLayer(hiddenDimension, outputDimension, decoderUnit, initParamsStdDev, rng));
            return new NeuralNetwork(layers);
        }
    }

    public class Training
    {

        public static double DecayRate = 0.999;
        public static double SmoothEpsilon = 1e-8;
        public static double GradientClipValue = 5;
        public static double Regularization = 0.000001; // L2 regularization strength

        public static double train<T>(int trainingEpochs, double learningRate, INeuralNetwork network, DataSet data, int reportEveryNthEpoch, Random rng) where T : INeuralNetwork
        {
            return train<T>(trainingEpochs, learningRate, network, data, reportEveryNthEpoch, false, false, null, rng);
        }

        public static double train<T>(int trainingEpochs, double learningRate, INeuralNetwork network, DataSet data, int reportEveryNthEpoch, bool initFromSaved, bool overwriteSaved, String savePath, Random rng) where T : INeuralNetwork
        {

            double result = 1.0;
            for (int epoch = 0; epoch < trainingEpochs; epoch++)
            {

                String show = "epoch[" + (epoch + 1) + "/" + trainingEpochs + "]";

                double reportedLossTrain = Pass(learningRate, network, data.Training, true, data.LossTraining, data.LossReporting);
                result = reportedLossTrain;
                if (Double.IsNaN(reportedLossTrain) || Double.IsInfinity(reportedLossTrain))
                {
                    throw new Exception("WARNING: invalid value for training loss. Try lowering learning rate.");
                }
                double reportedLossValidation = 0;
                double reportedLossTesting = 0;
                if (data.Validation != null)
                {
                    reportedLossValidation = Pass(learningRate, network, data.Validation, false, data.LossTraining, data.LossReporting);
                    result = reportedLossValidation;
                }
                if (data.Testing != null)
                {
                    reportedLossTesting = Pass(learningRate, network, data.Testing, false, data.LossTraining, data.LossReporting);
                    result = reportedLossTesting;
                }
                show += "\ttrain loss = " + String.Format("{0:N5}", reportedLossTrain);
                if (data.Validation != null)
                {
                    show += "\tvalid loss = " + String.Format("{0:N5}", reportedLossValidation);
                }
                if (data.Testing != null)
                {
                    show += "\ttest loss  = " + String.Format("{0:N5}", reportedLossTesting);
                }


                Console.WriteLine(show);
            }
            return result;
        }

        public static double Pass(double learningRate, INeuralNetwork network, List<DataSequence> sequences,
            bool applyTraining, ILoss lossTraining, ILoss lossReporting)
        {
            double numerLoss = 0;
            double denomLoss = 0;

            foreach (DataSequence seq in sequences)
            {
                network.ResetState();
                Graph g = new Graph(applyTraining);
                foreach (DataStep step in seq.Steps)
                {
                    Matrix output = network.Activate(step.Input, g);
                    if (step.TargetOutput != null)
                    {
                        double loss = lossReporting.Measure(output, step.TargetOutput);
                        if (Double.IsNaN(loss) || Double.IsInfinity(loss))
                        {
                            return loss;
                        }
                        numerLoss += loss;
                        denomLoss++;
                        if (applyTraining)
                        {
                            lossTraining.Backward(output, step.TargetOutput);
                        }
                    }
                }
                List<DataSequence> thisSequence = new List<DataSequence>();
                thisSequence.Add(seq);
                if (applyTraining)
                {
                    g.Backward(); //backprop dw values
                    UpdateModelParams(network, learningRate); //update params
                }
            }
            return numerLoss / denomLoss;
        }



        public static void UpdateModelParams(INeuralNetwork network, double stepSize)
        {
            foreach (Matrix m in network.GetParameters())
            {
                for (int i = 0; i < m.W.Length; i++)
                {

                    // rmsprop adaptive learning rate
                    double mdwi = m.Dw[i];
                    m.StepCache[i] = m.StepCache[i] * DecayRate + (1 - DecayRate) * mdwi * mdwi;

                    // gradient clip
                    if (mdwi > GradientClipValue)
                    {
                        mdwi = GradientClipValue;
                    }
                    if (mdwi < -GradientClipValue)
                    {
                        mdwi = -GradientClipValue;
                    }

                    // update (and regularize)
                    m.W[i] += -stepSize * mdwi / Math.Sqrt(m.StepCache[i] + SmoothEpsilon) - Regularization * m.W[i];
                    m.Dw[i] = 0;
                }
            }
        }

    }
}
