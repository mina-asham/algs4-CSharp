using System;
using System.Collections;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Selection
    {
        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                int min = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Less(a[j], a[min]))
                    {
                        min = j;
                    }
                }
                Exch(a, i, min);
                Debug.Assert(IsSorted(a, 0, i));
            }
            Debug.Assert(IsSorted(a));
        }

        /// <summary>
        /// Rearranges the array in ascending order, using a comparator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array</param>
        /// <param name="c">the comparator specifying the order</param>
        public static void Sort<T>(T[] a, IComparer c)
        {
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                int min = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (Less(c, a[j], a[min]))
                    {
                        min = j;
                    }
                }
                Exch(a, i, min);
                Debug.Assert(IsSorted(a, c, 0, i));
            }
            Debug.Assert(IsSorted(a, c));
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
        /// <param name="c"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static bool Less<T>(IComparer c, T v, T w)
        {
            return (c.Compare(v, w) < 0);
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

        /// <summary>
        /// Is the array a[] sorted?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Is the array a[] sorted?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsSorted<T>(T[] a, IComparer c)
        {
            return IsSorted(a, c, 0, a.Length - 1);
        }

        /// <summary>
        /// Is the array sorted from a[lo] to a[hi]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="c"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        private static bool IsSorted<T>(T[] a, IComparer c, int lo, int hi)
        {
            for (int i = lo + 1; i <= hi; i++)
            {
                if (Less(c, a[i], a[i - 1]))
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
        /// Reads in a sequence of strings from standard input; selection sorts them;
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