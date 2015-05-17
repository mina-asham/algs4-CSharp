using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Quick3Way
    {
        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            StdRandom.Shuffle(a);
            Sort(a, 0, a.Length - 1);
            Debug.Assert(IsSorted(a));
        }

        /// <summary>
        /// Quicksort the subarray a[lo .. hi] using 3-way partitioning
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        private static void Sort<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            if (hi <= lo)
            {
                return;
            }
            int lt = lo, gt = hi;
            T v = a[lo];
            int i = lo;
            while (i <= gt)
            {
                int cmp = a[i].CompareTo(v);
                if (cmp < 0)
                {
                    Exch(a, lt++, i++);
                }
                else if (cmp > 0)
                {
                    Exch(a, i, gt--);
                }
                else
                {
                    i++;
                }
            }

            // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi]. 
            Sort(a, lo, lt - 1);
            Sort(a, gt + 1, hi);
            Debug.Assert(IsSorted(a, lo, hi));
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
            return (v.CompareTo(w) < 0);
        }

        /// <summary>
        /// Exchange a[i] and a[j]
        /// </summary>
        /// <typeparam name="T"></typeparam>
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

        #region Check if array is sorted - useful for debugging

        private static bool IsSorted<T>(T[] a) where T : IComparable<T>
        {
            return IsSorted(a, 0, a.Length - 1);
        }

        private static bool IsSorted<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            for (int i = lo + 1; i <= hi; i++)
            {
                if (Less(a[i], a[i - 1]))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

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
        /// Reads in a sequence of strings from standard input; 3-way
        /// quicksorts them; and prints them to standard output in ascending order. 
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