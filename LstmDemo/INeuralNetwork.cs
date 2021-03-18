using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public interface INeuralNetwork
    {
        Matrix Activate(Matrix input, Graph g);
        void ResetState();
        List<Matrix> GetParameters();
    }
}
