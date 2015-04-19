using System;

namespace algs4.stdlib
{
    public static class StdStats
    {
        /// <summary>
        /// Returns the maximum value in the array a[], -infinity if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Max(double[] a)
        {
            double max = double.NegativeInfinity;
            for (int i = 0; i < a.Length; i++)
            {
                if (double.IsNaN(a[i]))
                {
                    return double.NaN;
                }
                if (a[i] > max)
                {
                    max = a[i];
                }
            }
            return max;
        }

        /// <summary>
        /// Returns the maximum value in the subarray a[lo..hi], -infinity if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double Max(double[] a, int lo, int hi)
        {
            if (lo < 0 || hi >= a.Length || lo > hi)
            {
                throw new IndexOutOfRangeException("Subarray indices out of bounds");
            }
            double max = double.NegativeInfinity;
            for (int i = lo; i <= hi; i++)
            {
                if (double.IsNaN(a[i]))
                {
                    return double.NaN;
                }
                if (a[i] > max)
                {
                    max = a[i];
                }
            }
            return max;
        }

        /// <summary>
        /// Returns the maximum value in the array a[], int.MinValue if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Max(int[] a)
        {
            int max = int.MinValue;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] > max)
                {
                    max = a[i];
                }
            }
            return max;
        }

        /// <summary>
        /// Returns the minimum value in the array a[], +infinity if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Min(double[] a)
        {
            double min = double.PositiveInfinity;
            for (int i = 0; i < a.Length; i++)
            {
                if (double.IsNaN(a[i]))
                {
                    return double.NaN;
                }
                if (a[i] < min)
                {
                    min = a[i];
                }
            }
            return min;
        }

        /// <summary>
        /// Returns the minimum value in the subarray a[lo..hi], +infinity if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double Min(double[] a, int lo, int hi)
        {
            if (lo < 0 || hi >= a.Length || lo > hi)
            {
                throw new IndexOutOfRangeException("Subarray indices out of bounds");
            }
            double min = double.PositiveInfinity;
            for (int i = lo; i <= hi; i++)
            {
                if (double.IsNaN(a[i]))
                {
                    return double.NaN;
                }
                if (a[i] < min)
                {
                    min = a[i];
                }
            }
            return min;
        }

        /// <summary>
        /// Returns the minimum value in the array a[], int.MaxValue if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Min(int[] a)
        {
            int min = int.MaxValue;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] < min)
                {
                    min = a[i];
                }
            }
            return min;
        }

        /// <summary>
        /// Returns the average value in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Mean(double[] a)
        {
            if (a.Length == 0)
            {
                return double.NaN;
            }
            double sum = Sum(a);
            return sum / a.Length;
        }

        /// <summary>
        /// Returns the average value in the subarray a[lo..hi], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double Mean(double[] a, int lo, int hi)
        {
            int length = hi - lo + 1;
            if (lo < 0 || hi >= a.Length || lo > hi)
            {
                throw new IndexOutOfRangeException("Subarray indices out of bounds");
            }
            if (length == 0)
            {
                return double.NaN;
            }
            double sum = Sum(a, lo, hi);
            return sum / length;
        }

        /// <summary>
        /// Returns the average value in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Mean(int[] a)
        {
            if (a.Length == 0)
            {
                return double.NaN;
            }
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum = sum + a[i];
            }
            return sum / a.Length;
        }

        /// <summary>
        /// Returns the sample variance in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Var(double[] a)
        {
            if (a.Length == 0)
            {
                return double.NaN;
            }
            double avg = Mean(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / (a.Length - 1);
        }

        /// <summary>
        /// Returns the sample variance in the subarray a[lo..hi], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double Var(double[] a, int lo, int hi)
        {
            int length = hi - lo + 1;
            if (lo < 0 || hi >= a.Length || lo > hi)
            {
                throw new IndexOutOfRangeException("Subarray indices out of bounds");
            }
            if (length == 0)
            {
                return double.NaN;
            }
            double avg = Mean(a, lo, hi);
            double sum = 0.0;
            for (int i = lo; i <= hi; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / (length - 1);
        }

        /// <summary>
        /// Returns the sample variance in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Var(int[] a)
        {
            if (a.Length == 0)
            {
                return double.NaN;
            }
            double avg = Mean(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / (a.Length - 1);
        }

        /// <summary>
        /// Returns the population variance in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double VarP(double[] a)
        {
            if (a.Length == 0)
            {
                return double.NaN;
            }
            double avg = Mean(a);
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / a.Length;
        }

        /// <summary>
        /// Returns the population variance in the subarray a[lo..hi], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double VarP(double[] a, int lo, int hi)
        {
            int length = hi - lo + 1;
            if (lo < 0 || hi >= a.Length || lo > hi)
            {
                throw new IndexOutOfRangeException("Subarray indices out of bounds");
            }
            if (length == 0)
            {
                return double.NaN;
            }
            double avg = Mean(a, lo, hi);
            double sum = 0.0;
            for (int i = lo; i <= hi; i++)
            {
                sum += (a[i] - avg) * (a[i] - avg);
            }
            return sum / length;
        }


        /// <summary>
        /// Returns the sample standard deviation in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double StdDev(double[] a)
        {
            return Math.Sqrt(Var(a));
        }

        /// <summary>
        /// Returns the sample standard deviation in the subarray a[lo..hi], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double StdDev(double[] a, int lo, int hi)
        {
            return Math.Sqrt(Var(a, lo, hi));
        }

        /// <summary>
        /// Returns the sample standard deviation in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double StdDev(int[] a)
        {
            return Math.Sqrt(Var(a));
        }

        /// <summary>
        /// Returns the population standard deviation in the array a[], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double StdDevP(double[] a)
        {
            return Math.Sqrt(VarP(a));
        }

        /// <summary>
        /// Returns the population standard deviation in the subarray a[lo..hi], NaN if no such value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double StdDevP(double[] a, int lo, int hi)
        {
            return Math.Sqrt(VarP(a, lo, hi));
        }

        /// <summary>
        /// Returns the sum of all values in the array a[].
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Sum(double[] a)
        {
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of all values in the subarray a[lo..hi].
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public static double Sum(double[] a, int lo, int hi)
        {
            if (lo < 0 || hi >= a.Length || lo > hi)
            {
                throw new IndexOutOfRangeException("Subarray indices out of bounds");
            }
            double sum = 0.0;
            for (int i = lo; i <= hi; i++)
            {
                sum += a[i];
            }
            return sum;
        }

        /// <summary>
        /// Returns the sum of all values in the array a[].
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Sum(int[] a)
        {
            int sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
            }
            return sum;
        }

        /// <summary>
        /// Plots the points (i, a[i]) to standard draw.
        /// </summary>
        /// <param name="a"></param>
        public static void PlotPoints(double[] a)
        {
            int n = a.Length;
            StdDraw.SetXscale(0, n - 1);
            StdDraw.SetPenRadius(1.0 / (3.0 * n));
            for (int i = 0; i < n; i++)
            {
                StdDraw.Point(i, a[i]);
            }
        }

        /// <summary>
        /// Plots line segments connecting points (i, a[i]) to standard draw.
        /// </summary>
        /// <param name="a"></param>
        public static void PlotLines(double[] a)
        {
            int n = a.Length;
            StdDraw.SetXscale(0, n - 1);
            StdDraw.SetPenRadius();
            for (int i = 1; i < n; i++)
            {
                StdDraw.Line(i - 1, a[i - 1], i, a[i]);
            }
        }

        /// <summary>
        /// Plots bars from (0, a[i]) to (i, a[i]) to standard draw.
        /// </summary>
        /// <param name="a"></param>
        public static void PlotBars(double[] a)
        {
            int n = a.Length;
            StdDraw.SetXscale(0, n - 1);
            for (int i = 0; i < n; i++)
            {
                StdDraw.FilledRectangle(i, a[i] / 2, .25, a[i] / 2);
            }
        }


        /// <summary>
        /// Test client.
        /// Convert command-line arguments to array of doubles and call various methods.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            double[] a = StdArrayIO.ReadDouble1D();
            StdOut.PrintF("       min {0:0000000000.000}\n", Min(a));
            StdOut.PrintF("      mean {0:0000000000.000}\n", Mean(a));
            StdOut.PrintF("       max {0:0000000000.000}\n", Max(a));
            StdOut.PrintF("       sum {0:0000000000.000}\n", Sum(a));
            StdOut.PrintF("    StdDev {0:0000000000.000}\n", StdDev(a));
            StdOut.PrintF("       var {0:0000000000.000}\n", Var(a));
            StdOut.PrintF("   stddevp {0:0000000000.000}\n", StdDevP(a));
            StdOut.PrintF("      VarP {0:0000000000.000}\n", VarP(a));
        }

        public class StdDraw
        {
            public static void SetXscale(int i, int i1)
            {
                throw new NotImplementedException();
            }

            public static void SetPenRadius()
            {
                throw new NotImplementedException();
            }

            public static void Line(int i, double d, int i1, double d1)
            {
                throw new NotImplementedException();
            }

            public static void FilledRectangle(int i, double d, double d1, double d2)
            {
                throw new NotImplementedException();
            }

            public static void SetPenRadius(double d)
            {
                throw new NotImplementedException();
            }

            public static void Point(int i, double d)
            {
                throw new NotImplementedException();
            }
        }
    }
}
