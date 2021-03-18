using System;

namespace LstmDemo
{
    public class Matrix
    {
        public int Rows;
        public int Cols;
        public double[] W;
        public double[] Dw;
        public double[] StepCache;

        public Matrix(int dim)
        {
            this.Rows = dim;
            this.Cols = 1;
            this.W = new double[Rows * Cols];
            this.Dw = new double[Rows * Cols];
            this.StepCache = new double[Rows * Cols];
        }
        public Matrix(int rows, int cols)
        {
            this.Rows = rows;
            this.Cols = cols;
            this.W = new double[rows * cols];
            this.Dw = new double[rows * cols];
            this.StepCache = new double[rows * cols];
        }
        public Matrix(double[] vector)
        {
            this.Rows = vector.Length;
            this.Cols = 1;
            this.W = vector;
            this.Dw = new double[vector.Length];
            this.StepCache = new double[vector.Length];
        }
        public static Matrix Random(int rows, int cols, double initParamsStdDev, Random rng)
        {
            Matrix result = new Matrix(rows, cols);
            for (int i = 0; i < result.W.Length; i++)
            {
                result.W[i] = rng.NextDouble() * initParamsStdDev;
            }
            return result;
        }
        public static Matrix Uniform(int rows, int cols, double s)
        {
            Matrix result = new Matrix(rows, cols);
            for (int i = 0; i < result.W.Length; i++)
            {
                result.W[i] = s;
            }
            return result;
        }
        public static Matrix Ones(int rows, int cols)
        {
            return Uniform(rows, cols, 1.0);
        }

    }
}
