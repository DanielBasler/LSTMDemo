using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class DataSet
    {
        public int InputDimension { get; set; }
        public int OutputDimension { get; set; }
        public ILoss LossTraining { get; set; }
        public ILoss LossReporting { get; set; }
        public List<DataSequence> Training { get; set; }
        public List<DataSequence> Validation { get; set; }
        public List<DataSequence> Testing { get; set; }

        internal IForNoLinearity GetModelOutputUnitToUse()
        {
            return new SigmoidUnit();
        }
    }

    public class DataSequence
    {
        public List<DataStep> Steps { get; set; }               

        public DataSequence(List<DataStep> steps)
        {
            this.Steps = steps;
        }
    }

    public class DataStep
    {
        public Matrix Input = null;
        public Matrix TargetOutput = null;
        public DataStep(double[] input, double[] targetOutput)
        {
            this.Input = new Matrix(input);
            if (targetOutput != null)
            {
                this.TargetOutput = new Matrix(targetOutput);
            }
        }

    }
}
