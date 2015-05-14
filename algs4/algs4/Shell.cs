using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Shell
    {
        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            int n = a.Length;

            // 3x+1 increment sequence:  1, 4, 13, 40, 121, 364, 1093, ... 
            int h = 1;
            while (h < n / 3)
            {
                h = 3 * h + 1;
            }

            while (h >= 1)
            {
                // h-sort the array
                for (int i = h; i < n; i++)
                {
                    for (int j = i; j >= h && Less(a[j], a[j - h]); j -= h)
                    {
                        Exch(a, j, j - h);
                    }
                }
                Debug.Assert(IsHsorted(a, h));
                h /= 3;
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
        /// Is the array h-sorted?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        private static bool IsHsorted<T>(T[] a, int h) where T : IComparable<T>
        {
            for (int i = h; i < a.Length; i++)
            {
                if (Less(a[i], a[i - h]))
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

        #endregion

        /// <summary>
        /// Reads in a sequence of strings from standard input; Shellsorts them; 
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