using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class FeedForwardLayer : INetworkLayer
    {
        readonly Matrix w;
        readonly Matrix b;
        readonly IForNoLinearity f;
        public FeedForwardLayer(int inputDimension, int outputDimension, IForNoLinearity f, double initParamsStdDev, Random rng)
        {
            w = Matrix.Random(outputDimension, inputDimension, initParamsStdDev, rng);
            b = new Matrix(outputDimension);
            this.f = f;
        }

        public Matrix Activate(Matrix input, Graph g)
        {
            Matrix sum = g.Add(g.Mul(w, input), b);
            Matrix returnObj = g.Nonlin(f, sum);
            return returnObj;
        }

        public void ResetState()
        {

        }

        public List<Matrix> GetParameters()
        {
            List<Matrix> result = new List<Matrix>();
            result.Add(w);
            result.Add(b);
            return result;
        }
    }
}
