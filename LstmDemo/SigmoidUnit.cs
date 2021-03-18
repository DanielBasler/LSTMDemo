using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class SigmoidUnit : IForNoLinearity
    {
        private static long serialVersionUid = 1L;
        private long id;

        public SigmoidUnit()
        {
            id = serialVersionUid + 1;
        }

        public double Forward(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public double Backward(double x)
        {
            double act = Forward(x);
            return act * (1 - act);
        }
    }
}
