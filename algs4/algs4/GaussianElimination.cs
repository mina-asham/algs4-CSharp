using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class GaussianElimination
    {
        private const double Epsilon = 1e-10;

        // Gaussian elimination with partial pivoting
        public static double[] LSolve(double[][] a, double[] b)
        {
            int n = b.Length;

            for (int p = 0; p < n; p++)
            {
                // find pivot row and swap
                int max = p;
                for (int i = p + 1; i < n; i++)
                {
                    if (Math.Abs(a[i][p]) > Math.Abs(a[max][p]))
                    {
                        max = i;
                    }
                }
                double[] temp = a[p];
                a[p] = a[max];
                a[max] = temp;
                double t = b[p];
                b[p] = b[max];
                b[max] = t;

                // singular or nearly singular
                if (Math.Abs(a[p][p]) <= Epsilon)
                {
                    throw new ArithmeticException("Matrix is singular or nearly singular");
                }

                // pivot within A and b
                for (int i = p + 1; i < n; i++)
                {
                    double alpha = a[i][p] / a[p][p];
                    b[i] -= alpha * b[p];
                    for (int j = p; j < n; j++)
                    {
                        a[i][j] -= alpha * a[p][j];
                    }
                }
            }

            // back substitution
            double[] x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0.0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += a[i][j] * x[j];
                }
                x[i] = (b[i] - sum) / a[i][i];
            }
            return x;
        }

        /// <summary>
        /// Sample client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            const int n = 3;
            double[][] a =
            {
                new[] { 0.0, 1.0, 1.0 },
                new[] { 2.0, 4.0, -2.0 },
                new[] { 0.0, 3.0, 15.0 }
            };
            double[] b = { 4, 2, 36 };
            double[] x = LSolve(a, b);

            // print results
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn(x[i]);
            }
        }
    }
}