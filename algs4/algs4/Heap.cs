using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Heap
    {
        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <param name="pq">the array to be sorted</param>
        public static void Sort<T>(T[] pq) where T : IComparable<T>
        {
            int n = pq.Length;
            for (int k = n / 2; k >= 1; k--)
            {
                Sink(pq, k, n);
            }
            while (n > 1)
            {
                Exch(pq, 1, n--);
                Sink(pq, 1, n);
            }

            Debug.Assert(IsSorted(pq));
        }

        /// <summary>
        /// Helper functions to restore the heap invariant.
        /// </summary>
        /// <param name="pq"></param>
        /// <param name="k"></param>
        /// <param name="n"></param>
        private static void Sink<T>(T[] pq, int k, int n) where T : IComparable<T>
        {
            while (2 * k <= n)
            {
                int j = 2 * k;
                if (j < n && Less(pq, j, j + 1))
                {
                    j++;
                }
                if (!Less(pq, k, j))
                {
                    break;
                }
                Exch(pq, k, j);
                k = j;
            }
        }

        #region Helper functions for comparisons and swaps. Indices are "off-by-one" to support 1-based indexing.

        private static bool Less<T>(T[] pq, int i, int j) where T : IComparable<T>
        {
            return pq[i - 1].CompareTo(pq[j - 1]) < 0;
        }

        private static void Exch<T>(T[] pq, int i, int j)
        {
            T swap = pq[i - 1];
            pq[i - 1] = pq[j - 1];
            pq[j - 1] = swap;
        }

        // is v < w ?
        private static bool Less<T>(T v, T w) where T : IComparable<T>
        {
            return (v.CompareTo(w) < 0);
        }

        #endregion

        /// <summary>
        /// Check if array is sorted - useful for debugging
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static bool IsSorted<T>(T[] a) where T : IComparable<T>
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (Less(a[i], a[i - 1]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Print array to standard output
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        private static void Show<T>(T[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }

        /// <summary>
        /// Reads in a sequence of strings from standard input; heapsorts them;
        /// and prints them to standard output in ascending order.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string[] a = StdIn.ReadAllStrings();
            Sort(a);
            Show(a);
        }
    }
}