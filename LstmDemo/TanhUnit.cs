using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class TanhUnit : IForNoLinearity
    {
        private static long serialVersionUid = 1L;
        private long id;

        public TanhUnit()
        {
            id = serialVersionUid + 1;
        }

        public double Forward(double x)
        {
            return Math.Tanh(x);
        }

        public double Backward(double x)
        {
            double coshx = Math.Cosh(x);
            double denom = (Math.Cosh(2 * x) + 1);
            return 4 * coshx * coshx / (denom * denom);
        }
    }
}
