using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Insertion
    {
        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j > 0 && Less(a[j], a[j - 1]); j--)
                {
                    Exch(a, j, j - 1);
                }
                Debug.Assert(IsSorted(a, 0, i));
            }
            Debug.Assert(IsSorted(a));
        }

        /// <summary>
        /// Rearranges the subarray a[lo..hi] in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        /// <param name="lo">left endpoint</param>
        /// <param name="hi">right endpoint</param>
        public static void Sort<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > 0 && Less(a[j], a[j - 1]); j--)
                {
                    Exch(a, j, j - 1);
                }
            }
            Debug.Assert(IsSorted(a, lo, hi));
        }

        /// <summary>
        /// Rearranges the array in ascending order, using a comparator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array</param>
        /// <param name="comparator">the comparator specifying the order</param>
        public static void Sort<T>(T[] a, IComparer<T> comparator) where T : IComparable<T>
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                for (int j = i; j > 0 && Less(a[j], a[j - 1], comparator); j--)
                {
                    Exch(a, j, j - 1);
                }
                Debug.Assert(IsSorted(a, 0, i, comparator));
            }
            Debug.Assert(IsSorted(a, comparator));
        }

        /// <summary>
        /// Rearranges the subarray a[lo..hi] in ascending order, using a comparator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array</param>
        /// <param name="lo">left endpoint</param>
        /// <param name="hi">right endpoint</param>
        /// <param name="comparator">the comparator specifying the order</param>
        public static void Sort<T>(T[] a, int lo, int hi, IComparer<T> comparator) where T : IComparable<T>
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > 0 && Less(a[j], a[j - 1], comparator); j--)
                {
                    Exch(a, j, j - 1);
                }
            }
            Debug.Assert(IsSorted(a, lo, hi, comparator));
        }

        /// <summary>
        /// Returns a permutation that gives the elements in the array in ascending order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array</param>
        /// <returns>a permutation p[] such that a[p[0]], a[p[1]], ..., a[p[N-1]] are in ascending order</returns>
        public static int[] IndexSort<T>(T[] a) where T : IComparable<T>
        {
            int n = a.Length;
            int[] index = new int[n];
            for (int i = 0; i < n; i++)
            {
                index[i] = i;
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = i; j > 0 && Less(a[index[j]], a[index[j - 1]]); j--)
                {
                    Exch(index, j, j - 1);
                }
            }

            return index;
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
        /// Is v &lt; w ?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        private static bool Less<T>(T v, T w, IComparer<T> comparator) where T : IComparable<T>
        {
            return (comparator.Compare(v, w) < 0);
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

        /// <summary>
        /// Exchange a[i] and a[j]  (for indirect Sort)
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private static void Exch(int[] a, int i, int j)
        {
            int swap = a[i];
            a[i] = a[j];
            a[j] = swap;
        }

        #endregion

        #region Check if array is sorted - useful for debugging

        private static bool IsSorted<T>(T[] a) where T : IComparable<T>
        {
            return IsSorted(a, 0, a.Length - 1);
        }

        /// <summary>
        /// Is the array sorted from a[lo] to a[hi]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
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

        private static bool IsSorted<T>(T[] a, IComparer<T> comparator) where T : IComparable<T>
        {
            return IsSorted(a, 0, a.Length - 1, comparator);
        }

        /// <summary>
        /// Is the array sorted from a[lo] to a[hi]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        private static bool IsSorted<T>(T[] a, int lo, int hi, IComparer<T> comparator) where T : IComparable<T>
        {
            for (int i = lo + 1; i <= hi; i++)
            {
                if (Less(a[i], a[i - 1], comparator))
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