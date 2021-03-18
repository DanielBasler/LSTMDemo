using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class LstmLayer : INetworkLayer
    {
        int inputDimension;
        int outputDimension;

        readonly Matrix wix;
        readonly Matrix wih;
        readonly Matrix inputBias;
        readonly Matrix wfx;
        readonly Matrix wfh;
        readonly Matrix forgetBias;
        readonly Matrix wox;
        readonly Matrix woh;
        readonly Matrix outputBias;
        readonly Matrix wcx;
        readonly Matrix wch;
        readonly Matrix cellWriteBias;

        Matrix _hiddenContext;
        Matrix _cellContext;

        readonly IForNoLinearity _inputGateActivation = new SigmoidUnit();
        readonly IForNoLinearity _forgetGateActivation = new SigmoidUnit();
        readonly IForNoLinearity _outputGateActivation = new SigmoidUnit();
        readonly IForNoLinearity _cellInputActivation = new TanhUnit();
        readonly IForNoLinearity _cellOutputActivation = new TanhUnit();

        public LstmLayer(int inputDimension, int outputDimension, double initParamsStdDev, Random rng)
        {
            this.inputDimension = inputDimension;
            this.outputDimension = outputDimension;
            wix = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            wih = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            inputBias = new Matrix(outputDimension);
            wfx = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            wfh = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);            
            forgetBias = Matrix.Ones(outputDimension, 1);
            wox = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            woh = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            outputBias = new Matrix(outputDimension);
            wcx = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            wch = Matrix.Random(outputDimension, outputDimension, initParamsStdDev, rng);
            cellWriteBias = new Matrix(outputDimension);
        }

        public Matrix Activate(Matrix input, Graph g)
        {

            //input gate
            Matrix sum0 = g.Mul(wix, input);
            Matrix sum1 = g.Mul(wih, _hiddenContext);
            Matrix inputGate = g.Nonlin(_inputGateActivation, g.Add(g.Add(sum0, sum1), inputBias));

            //forget gate
            Matrix sum2 = g.Mul(wfx, input);
            Matrix sum3 = g.Mul(wfh, _hiddenContext);
            Matrix forgetGate = g.Nonlin(_forgetGateActivation, g.Add(g.Add(sum2, sum3), forgetBias));

            //output gate
            Matrix sum4 = g.Mul(wox, input);
            Matrix sum5 = g.Mul(woh, _hiddenContext);
            Matrix outputGate = g.Nonlin(_outputGateActivation, g.Add(g.Add(sum4, sum5), outputBias));

            //write operation on cells
            Matrix sum6 = g.Mul(wcx, input);
            Matrix sum7 = g.Mul(wch, _hiddenContext);
            Matrix cellInput = g.Nonlin(_cellInputActivation, g.Add(g.Add(sum6, sum7), cellWriteBias));

            //compute new cell activation
            Matrix retainCell = g.Elmul(forgetGate, _cellContext);
            Matrix writeCell = g.Elmul(inputGate, cellInput);
            Matrix cellAct = g.Add(retainCell, writeCell);

            //compute hidden state as gated, saturated cell activations
            Matrix output = g.Elmul(outputGate, g.Nonlin(_cellOutputActivation, cellAct));

            //rollover activations for next iteration
            _hiddenContext = output;
            _cellContext = cellAct;

            return output;
        }

        public void ResetState()
        {
            _hiddenContext = new Matrix(outputDimension);
            _cellContext = new Matrix(outputDimension);
        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            result.Add(wix);
            result.Add(wih);
            result.Add(inputBias);
            result.Add(wfx);
            result.Add(wfh);
            result.Add(forgetBias);
            result.Add(wox);
            result.Add(woh);
            result.Add(outputBias);
            result.Add(wcx);
            result.Add(wch);
            result.Add(cellWriteBias);
            return result;
        }
    }
}
