using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class DataSetGenerator : DataSet
    {
        public DataSetGenerator()
        {
            InputDimension = 2;
            OutputDimension = 1;
            LossTraining = new LossSumOfSquares();
            LossReporting = new LossSumOfSquares();
            Training = GetTrainingData();
            Validation = GetTrainingData();
            Testing = GetTrainingData();
        }

        public static List<DataSequence> GetTrainingData()
        {

            List<DataSequence> result = new List<DataSequence>();
            result.Add(new DataSequence(new List<DataStep>() { new DataStep(new double[] { 1, 0 }, new double[] { 1 }) }));
            result.Add(new DataSequence(new List<DataStep>() { new DataStep(new double[] { 0, 1 }, new double[] { 1 }) }));
            result.Add(new DataSequence(new List<DataStep>() { new DataStep(new double[] { 0, 0 }, new double[] { 0 }) }));
            result.Add(new DataSequence(new List<DataStep>() { new DataStep(new double[] { 1, 1 }, new double[] { 0 }) }));

            return result;
            
        }       
    }
}
