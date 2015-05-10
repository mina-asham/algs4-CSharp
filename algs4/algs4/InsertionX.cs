using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class InsertionX
    {
        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            int n = a.Length;

            // put smallest element in position to serve as sentinel
            for (int i = n - 1; i > 0; i--)
            {
                if (Less(a[i], a[i - 1]))
                {
                    Exch(a, i, i - 1);
                }
            }

            // insertion sort with half-exchanges
            for (int i = 2; i < n; i++)
            {
                T v = a[i];
                int j = i;
                while (Less(v, a[j - 1]))
                {
                    a[j] = a[j - 1];
                    j--;
                }
                a[j] = v;
            }

            Debug.Assert(IsSorted(a));
        }

        #region Helper sorting functions

        /// <summary>
        /// Is v &lt; w ?
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static bool Less<T>(T v, T w) where T : IComparable<T>
        {
            return v.CompareTo(w) < 0;
        }

        /// <summary>
        /// Exchange a[i] and a[j]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private static void Exch<T>(T[] a, int i, int j)
        {
            T swap = a[i];
            a[i] = a[j];
            a[j] = swap;
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
        /// <param name="a"></param>
        private static void Show<T>(T[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }

        /// <summary>
        /// Reads in a sequence of strings from standard input; insertion sorts them;
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