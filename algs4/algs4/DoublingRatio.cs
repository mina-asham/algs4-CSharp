using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class DoublingRatio
    {
        /// <summary>
        /// Returns the amount of time to call ThreeSum.count() with N
        /// random 6-digit integers.
        /// </summary>
        /// <param name="n">the number of integers</param>
        /// <returns>amount of time (in seconds) to call ThreeSum.count() with N random 6-digit integers</returns>
        public static double TimeTrial(int n)
        {
            const int max = 1000000;
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = StdRandom.Uniform(-max, max);
            }
            Stopwatch timer = new Stopwatch();
            ThreeSum.Count(a);
            return timer.ElapsedTime();
        }

        /// <summary>
        /// Prints table of running times to call ThreeSum.count()
        /// for arrays of size 250, 500, 1000, 2000, and so forth, along
        /// with ratios of running times between successive array sizes.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            double prev = TimeTrial(125);
            for (int n = 250; n < 1000000; n += n)
            {
                double time = TimeTrial(n);
                StdOut.PrintF("{0:000000} {1:0000000.0} {2:00000.0}\n", n, time, time / prev);
                prev = time;
            }
        }
    }
}