using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Knuth
    {
        /// <summary>
        /// Rearranges an array of objects in uniformly random order
        /// (under the assumption that Random.NextDouble() generates independent
        /// and uniformly distributed numbers between 0 and 1).
        /// </summary>
        /// <param name="a">the array to be shuffled</param>
        public static void Shuffle<T>(T[] a)
        {
            int n = a.Length;
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                // choose index uniformly in [i, N-1]
                int r = i + (int)(random.NextDouble() * (n - i));
                T swap = a[r];
                a[r] = a[i];
                a[i] = swap;
            }
        }

        /// <summary>
        /// Reads in a sequence of strings from standard input, shuffles
        /// them, and prints out the results.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // read in the data
            string[] a = StdIn.ReadAllStrings();

            // shuffle the array
            Shuffle(a);

            // print results.
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }
    }
}