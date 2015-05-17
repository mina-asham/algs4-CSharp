using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Simplex
    {
        private const double Epsilon = 1.0E-10;

        /// <summary>
        /// Tableaux
        /// </summary>
        private readonly double[][] _a;

        /// <summary>
        /// Number of constraints
        /// </summary>
        private readonly int _m;

        /// <summary>
        /// Number of original variables
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// basis[i] = basic variable corresponding to row i
        /// </summary>
        private readonly int[] _basis;

        /// <summary>
        /// Sets up the simplex tableaux
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public Simplex(double[][] a, double[] b, double[] c)
        {
            _m = b.Length;
            _n = c.Length;
            _a = new double[_m + 1][];
            for (int i = 0; i < _a.Length; i++)
            {
                _a[i] = new double[_n + _m + 1];
            }
            for (int i = 0; i < _m; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    _a[i][j] = a[i][j];
                }
            }
            for (int i = 0; i < _m; i++)
            {
                _a[i][_n + i] = 1.0;
            }
            for (int j = 0; j < _n; j++)
            {
                _a[_m][j] = c[j];
            }
            for (int i = 0; i < _m; i++)
            {
                _a[i][_m + _n] = b[i];
            }

            _basis = new int[_m];
            for (int i = 0; i < _m; i++)
            {
                _basis[i] = _n + i;
            }

            Solve();

            // check optimality conditions
            Debug.Assert(Check(a, b, c));
        }

        /// <summary>
        /// Run simplex algorithm starting from initial BFS
        /// </summary>
        private void Solve()
        {
            while (true)
            {
                // find entering column q
                int q = Bland();
                if (q == -1)
                {
                    break; // optimal
                }

                // find leaving row p
                int p = MinRatioRule(q);
                if (p == -1)
                {
                    throw new ArithmeticException("Linear program is unbounded");
                }

                // pivot
                Pivot(p, q);

                // update basis
                _basis[p] = q;
            }
        }

        /// <summary>
        /// Lowest index of a non-basic column with a positive cost
        /// </summary>
        /// <returns></returns>
        private int Bland()
        {
            for (int j = 0; j < _m + _n; j++)
            {
                if (_a[_m][j] > 0)
                {
                    return j;
                }
            }
            return -1; // optimal
        }

        /// <summary>
        /// Find row p using min ratio rule (-1 if no such row)
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private int MinRatioRule(int q)
        {
            int p = -1;
            for (int i = 0; i < _m; i++)
            {
                if (_a[i][q] <= 0)
                {
                    continue;
                }
                if (p == -1)
                {
                    p = i;
                }
                else if ((_a[i][_m + _n] / _a[i][q]) < (_a[p][_m + _n] / _a[p][q]))
                {
                    p = i;
                }
            }
            return p;
        }

        /// <summary>
        /// Pivot on entry (p, q) using Gauss-Jordan elimination
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        private void Pivot(int p, int q)
        {
            // everything but row p and column q
            for (int i = 0; i <= _m; i++)
            {
                for (int j = 0; j <= _m + _n; j++)
                {
                    if (i != p && j != q)
                    {
                        _a[i][j] -= _a[p][j] * _a[i][q] / _a[p][q];
                    }
                }
            }

            // zero out column q
            for (int i = 0; i <= _m; i++)
            {
                if (i != p)
                {
                    _a[i][q] = 0.0;
                }
            }

            // scale row p
            for (int j = 0; j <= _m + _n; j++)
            {
                if (j != q)
                {
                    _a[p][j] /= _a[p][q];
                }
            }
            _a[p][q] = 1.0;
        }

        /// <summary>
        /// Return optimal objective value
        /// </summary>
        /// <returns></returns>
        public double Value()
        {
            return -_a[_m][_m + _n];
        }

        /// <summary>
        /// Return primal solution vector
        /// </summary>
        /// <returns></returns>
        public double[] Primal()
        {
            double[] x = new double[_n];
            for (int i = 0; i < _m; i++)
            {
                if (_basis[i] < _n)
                {
                    x[_basis[i]] = _a[i][_m + _n];
                }
            }
            return x;
        }

        /// <summary>
        /// Return dual solution vector
        /// </summary>
        /// <returns></returns>
        public double[] Dual()
        {
            double[] y = new double[_m];
            for (int i = 0; i < _m; i++)
            {
                y[i] = -_a[_m][_n + i];
            }
            return y;
        }

        /// <summary>
        /// Is the solution primal feasible?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool IsPrimalFeasible(double[][] a, double[] b)
        {
            double[] x = Primal();

            // check that x >= 0
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] < 0.0)
                {
                    StdOut.PrintLn("x[" + j + "] = " + x[j] + " is negative");
                    return false;
                }
            }

            // check that Ax <= b
            for (int i = 0; i < _m; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < _n; j++)
                {
                    sum += a[i][j] * x[j];
                }
                if (sum > b[i] + Epsilon)
                {
                    StdOut.PrintLn("not primal feasible");
                    StdOut.PrintLn("b[" + i + "] = " + b[i] + ", sum = " + sum);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Is the solution dual feasible?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsDualFeasible(double[][] a, double[] c)
        {
            double[] y = Dual();

            // check that y >= 0
            for (int i = 0; i < y.Length; i++)
            {
                if (y[i] < 0.0)
                {
                    StdOut.PrintLn("y[" + i + "] = " + y[i] + " is negative");
                    return false;
                }
            }

            // check that yA >= c
            for (int j = 0; j < _n; j++)
            {
                double sum = 0.0;
                for (int i = 0; i < _m; i++)
                {
                    sum += a[i][j] * y[i];
                }
                if (sum < c[j] - Epsilon)
                {
                    StdOut.PrintLn("not dual feasible");
                    StdOut.PrintLn("c[" + j + "] = " + c[j] + ", sum = " + sum);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check that optimal value = cx = yb
        /// </summary>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsOptimal(double[] b, double[] c)
        {
            double[] x = Primal();
            double[] y = Dual();
            double value = Value();

            // check that value = cx = yb
            double value1 = 0.0;
            for (int j = 0; j < x.Length; j++)
            {
                value1 += c[j] * x[j];
            }
            double value2 = 0.0;
            for (int i = 0; i < y.Length; i++)
            {
                value2 += y[i] * b[i];
            }
            if (Math.Abs(value - value1) > Epsilon || Math.Abs(value - value2) > Epsilon)
            {
                StdOut.PrintLn("value = " + value + ", cx = " + value1 + ", yb = " + value2);
                return false;
            }

            return true;
        }

        private bool Check(double[][] a, double[] b, double[] c)
        {
            return IsPrimalFeasible(a, b) && IsDualFeasible(a, c) && IsOptimal(b, c);
        }

        /// <summary>
        /// Print tableaux
        /// </summary>
        public void Show()
        {
            StdOut.PrintLn("M = " + _m);
            StdOut.PrintLn("N = " + _n);
            for (int i = 0; i <= _m; i++)
            {
                for (int j = 0; j <= _m + _n; j++)
                {
                    StdOut.PrintF("{0:0000000.00} ", _a[i][j]);
                }
                StdOut.PrintLn();
            }
            StdOut.PrintLn("value = " + Value());
            for (int i = 0; i < _m; i++)
            {
                if (_basis[i] < _n)
                {
                    StdOut.PrintLn("x_" + _basis[i] + " = " + _a[i][_m + _n]);
                }
            }
            StdOut.PrintLn();
        }

        public static void Test(double[][] a, double[] b, double[] c)
        {
            Simplex lp = new Simplex(a, b, c);
            StdOut.PrintLn("value = " + lp.Value());
            double[] x = lp.Primal();
            for (int i = 0; i < x.Length; i++)
            {
                StdOut.PrintLn("x[" + i + "] = " + x[i]);
            }
            double[] y = lp.Dual();
            for (int j = 0; j < y.Length; j++)
            {
                StdOut.PrintLn("y[" + j + "] = " + y[j]);
            }
        }

        public static void Test1()
        {
            double[][] a =
            {
                new[] { -1.0, 1.0, 0.0 },
                new[] { 1.0, 4.0, 0.0 },
                new[] { 2.0, 1.0, 0.0 },
                new[] { 3.0, -4.0, 0.0 },
                new[] { 0.0, 0.0, 1.0 },
            };
            double[] c = { 1, 1, 1 };
            double[] b = { 5, 45, 27, 24, 4 };
            Test(a, b, c);
        }

        /// <summary>
        /// x0 = 12, x1 = 28, opt = 800
        /// </summary>
        public static void Test2()
        {
            double[] c = { 13.0, 23.0 };
            double[] b = { 480.0, 160.0, 1190.0 };
            double[][] a =
            {
                new[] { 5.0, 15.0 },
                new[] { 4.0, 4.0 },
                new[] { 35.0, 20.0 },
            };
            Test(a, b, c);
        }

        /// <summary>
        /// Unbounded
        /// </summary>
        public static void Test3()
        {
            double[] c = { 2.0, 3.0, -1.0, -12.0 };
            double[] b = { 3.0, 2.0 };
            double[][] a =
            {
                new[] { -2.0, -9.0, 1.0, 9.0 },
                new[] { 1.0, 1.0, -1.0, -2.0 },
            };
            Test(a, b, c);
        }

        /// <summary>
        /// Degenerate - cycles if you choose most positive objective function coefficient
        /// </summary>
        public static void Test4()
        {
            double[] c = { 10.0, -57.0, -9.0, -24.0 };
            double[] b = { 0.0, 0.0, 1.0 };
            double[][] a =
            {
                new[] { 0.5, -5.5, -2.5, 9.0 },
                new[] { 0.5, -1.5, -0.5, 1.0 },
                new[] { 1.0, 0.0, 0.0, 0.0 },
            };
            Test(a, b, c);
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            try
            {
                Test1();
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            StdOut.PrintLn("--------------------------------");

            try
            {
                Test2();
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            StdOut.PrintLn("--------------------------------");

            try
            {
                Test3();
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            StdOut.PrintLn("--------------------------------");

            try
            {
                Test4();
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            StdOut.PrintLn("--------------------------------");

            int m = int.Parse(args[0]);
            int n = int.Parse(args[1]);
            double[] c = new double[n];
            double[] b = new double[m];
            double[][] a = new double[m][];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = new double[n];
            }
            for (int j = 0; j < n; j++)
            {
                c[j] = StdRandom.Uniform(1000);
            }
            for (int i = 0; i < m; i++)
            {
                b[i] = StdRandom.Uniform(1000);
            }
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = StdRandom.Uniform(100);
                }
            }
            Simplex lp = new Simplex(a, b, c);
            StdOut.PrintLn(lp.Value());
        }
    }
}