using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class Graph
    {
        public bool ApplyBackprop { get; set; }
        public List<IRunnable> Backprop { get; set; }

        public Graph(bool applyBackprop)
        {
            this.ApplyBackprop = applyBackprop;
            this.Backprop = new List<IRunnable>();
        }
        public void Backward()
        {
            for (int i = Backprop.Count - 1; i >= 0; i--)
            {
                Backprop[i].Run();
            }
        }

        public Matrix Nonlin(IForNoLinearity neuron, Matrix m)
        {
            Matrix returnObj = new Matrix(m.Rows, m.Cols);
            int n = m.W.Length;
            for (int i = 0; i < n; i++)
            {
                returnObj.W[i] = neuron.Forward(m.W[i]);
            }
            if (this.ApplyBackprop)
            {
                Runnable bp = new Runnable();
                bp.Run = delegate ()
                {
                    for (int i = 0; i < n; i++)
                    {
                        m.Dw[i] += neuron.Backward(m.W[i]) * returnObj.Dw[i];
                    }

                };
                Backprop.Add(bp);
            }
            return returnObj;
        }
        public Matrix Mul(Matrix m1, Matrix m2)
        {
            if (m1.Cols != m2.Rows)
            {
                throw new Exception("matrix dimension mismatch");
            }

            int m1Rows = m1.Rows;
            int m1Cols = m1.Cols;
            int m2Cols = m2.Cols;
            Matrix returnObj = new Matrix(m1Rows, m2Cols);
            int outcols = m2Cols;
            for (int i = 0; i < m1Rows; i++)
            {
                int m1Col = m1Cols * i;
                for (int j = 0; j < m2Cols; j++)
                {
                    double dot = 0;
                    for (int k = 0; k < m1Cols; k++)
                    {
                        dot += m1.W[m1Col + k] * m2.W[m2Cols * k + j];
                    }
                    returnObj.W[outcols * i + j] = dot;
                }
            }
            if (this.ApplyBackprop)
            {
                Runnable bp = new Runnable();
                bp.Run = delegate ()
                {
                    for (int i = 0; i < m1.Rows; i++)
                    {
                        int outcol = outcols * i;
                        for (int j = 0; j < m2.Cols; j++)
                        {
                            double b = returnObj.Dw[outcol + j];
                            for (int k = 0; k < m1.Cols; k++)
                            {
                                m1.Dw[m1Cols * i + k] += m2.W[m2Cols * k + j] * b;
                                m2.Dw[m2Cols * k + j] += m1.W[m1Cols * i + k] * b;
                            }
                        }
                    }

                };
                Backprop.Add(bp);
            }
            return returnObj;
        }
        public Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
            {
                throw new Exception("matrix dimension mismatch");
            }
            Matrix returnObj = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.W.Length; i++)
            {
                returnObj.W[i] = m1.W[i] + m2.W[i];
            }
            if (this.ApplyBackprop)
            {
                Runnable bp = new Runnable();
                bp.Run = delegate ()
                {
                    for (int i = 0; i < m1.W.Length; i++)
                    {
                        m1.Dw[i] += returnObj.Dw[i];
                        m2.Dw[i] += returnObj.Dw[i];
                    }
                };
                Backprop.Add(bp);
            }
            return returnObj;
        }
        public Matrix Elmul(Matrix m1, Matrix m2)
        {
            if (m1.Rows != m2.Rows || m1.Cols != m2.Cols)
            {
                throw new Exception("matrix dimension mismatch");
            }
            Matrix returnObj = new Matrix(m1.Rows, m1.Cols);
            for (int i = 0; i < m1.W.Length; i++)
            {
                returnObj.W[i] = m1.W[i] * m2.W[i];
            }
            if (this.ApplyBackprop)
            {
                Runnable bp = new Runnable();
                bp.Run = delegate ()
                {
                    for (int i = 0; i < m1.W.Length; i++)
                    {
                        m1.Dw[i] += m2.W[i] * returnObj.Dw[i];
                        m2.Dw[i] += m1.W[i] * returnObj.Dw[i];
                    }
                };
                Backprop.Add(bp);
            }
            return returnObj;
        }

    }
}
