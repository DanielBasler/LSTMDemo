using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public interface IForNoLinearity
    {
        double Forward(double x);
        double Backward(double x);
    }
}
