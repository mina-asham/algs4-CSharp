using algs4.stdlib;

namespace algs4.algs4
{
    public static class DoublingTest
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
            stdlib.Stopwatch timer = new stdlib.Stopwatch();
            ThreeSum.Count(a);
            return timer.ElapsedTime();
        }

        /// <summary>
        /// Prints table of running times to call ThreeSum.count()
        /// for arrays of size 250, 500, 1000, 2000, and so forth.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            for (int n = 250; n < 1000000; n += n)
            {
                double time = TimeTrial(n);
                StdOut.PrintF("{0:0000000} {1:00000.0}\n", n, time);
            }
        }
    }
}