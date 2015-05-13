using algs4.stdlib;

namespace algs4.algs4
{
    public static class ThreeSum
    {
        /// <summary>
        /// Prints to standard output the (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0.
        /// </summary>
        /// <param name="a">the array of integers</param>
        public static void PrintAll(int[] a)
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        if (a[i] + a[j] + a[k] == 0)
                        {
                            StdOut.PrintLn(a[i] + " " + a[j] + " " + a[k]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the number of triples (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0.
        /// </summary>
        /// <param name="a">the array of integers</param>
        /// <returns>the number of triples (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0</returns>
        public static int Count(int[] a)
        {
            int n = a.Length;
            int cnt = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    for (int k = j + 1; k < n; k++)
                    {
                        if (a[i] + a[j] + a[k] == 0)
                        {
                            cnt++;
                        }
                    }
                }
            }
            return cnt;
        }

        /// <summary>
        /// Reads in a sequence of integers from a file, specified as a command-line argument;
        /// counts the number of triples sum to exactly zero; prints out the time to perform
        /// the computation.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            int[] a = input.ReadAllInts();

            Stopwatch timer = new Stopwatch();
            int cnt = Count(a);
            StdOut.PrintLn("elapsed time = " + timer.ElapsedTime());
            StdOut.PrintLn(cnt);
        }
    }
}