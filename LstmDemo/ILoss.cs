using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public interface ILoss
    {
        void Backward(Matrix actualOutput, Matrix targetOutput);
        double Measure(Matrix actualOutput, Matrix targetOutput);
    }
}
