using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class ThreeSumFast
    {
        /// <summary>
        /// returns true if the sorted array a[] contains any duplicated integers
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static bool ContainsDuplicates(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] == a[i - 1])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Prints to standard output the (i, j, k) with i &lt; j &lt; k such that a[i] + a[j] + a[k] == 0.
        /// </summary>
        /// <param name="a">the array of integers</param>
        public static void PrintAll(int[] a)
        {
            int n = a.Length;
            Array.Sort(a);
            if (ContainsDuplicates(a))
            {
                throw new ArgumentException("array contains duplicate integers");
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    int k = Array.BinarySearch(a, -(a[i] + a[j]));
                    if (k > j)
                    {
                        StdOut.PrintLn(a[i] + " " + a[j] + " " + a[k]);
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
            Array.Sort(a);
            if (ContainsDuplicates(a))
            {
                throw new ArgumentException("array contains duplicate integers");
            }
            int cnt = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    int k = Array.BinarySearch(a, -(a[i] + a[j]));
                    if (k > j)
                    {
                        cnt++;
                    }
                }
            }
            return cnt;
        }

        /// <summary>
        /// Reads in a sequence of distinct integers from a file, specified as a command-line argument;
        /// counts the number of triples sum to exactly zero; prints out the time to perform
        /// the computation.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            int[] a = input.ReadAllInts();
            int cnt = Count(a);
            StdOut.PrintLn(cnt);
        }
    }
}
