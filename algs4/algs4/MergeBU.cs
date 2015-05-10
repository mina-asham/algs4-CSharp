using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class MergeBU
    {
        /// <summary>
        /// Stably merge a[lo..mid] with a[mid+1..hi] using aux[lo..hi]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="aux"></param>
        /// <param name="lo"></param>
        /// <param name="mid"></param>
        /// <param name="hi"></param>
        private static void Merge<T>(T[] a, T[] aux, int lo, int mid, int hi) where T : IComparable<T>
        {
            // copy to aux[]
            for (int k = lo; k <= hi; k++)
            {
                aux[k] = a[k];
            }

            // merge back to a[]
            int i = lo, j = mid + 1;
            for (int k = lo; k <= hi; k++)
            {
                if (i > mid)
                {
                    a[k] = aux[j++]; // this copying is unneccessary
                }
                else if (j > hi)
                {
                    a[k] = aux[i++];
                }
                else if (Less(aux[j], aux[i]))
                {
                    a[k] = aux[j++];
                }
                else
                {
                    a[k] = aux[i++];
                }
            }
        }

        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            int n = a.Length;
            T[] aux = new T[n];
            for (int i = 1; i < n; i = i + i)
            {
                for (int j = 0; j < n - i; j += i + i)
                {
                    int lo = j;
                    int m = j + i - 1;
                    int hi = Math.Min(j + i + i - 1, n - 1);
                    Merge(a, aux, lo, m, hi);
                }
            }

            Debug.Assert(IsSorted(a));
        }

        #region Helper sorting functions

        /// <summary>
        /// Is v &lt; w ?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static bool Less<T>(T v, T w) where T : IComparable<T>
        {
            return v.CompareTo(w) < 0;
        }

        #endregion

        /// <summary>
        /// Check if array is sorted - useful for debugging
        /// </summary>
        /// <typeparam name="T"></typeparam>
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
        /// Reads in a sequence of strings from standard input; bottom-up
        /// mergesorts them; and prints them to standard output in ascending order. 
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