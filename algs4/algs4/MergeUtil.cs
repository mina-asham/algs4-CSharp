using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class MergeUtil
    {
        /// <summary>
        /// Stably Merge a[lo .. mid] with a[mid+1 ..hi] using aux[lo .. hi]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="aux"></param>
        /// <param name="lo"></param>
        /// <param name="mid"></param>
        /// <param name="hi"></param>
        private static void Merge<T>(T[] a, T[] aux, int lo, int mid, int hi) where T : IComparable<T>
        {
            // precondition: a[lo .. mid] and a[mid+1 .. hi] are sorted subarrays
            Debug.Assert(IsSorted(a, lo, mid));
            Debug.Assert(IsSorted(a, mid + 1, hi));

            // copy to aux[]
            for (int k = lo; k <= hi; k++)
            {
                aux[k] = a[k];
            }

            // Merge back to a[]
            int i = lo, j = mid + 1;
            for (int k = lo; k <= hi; k++)
            {
                if (i > mid)
                {
                    a[k] = aux[j++]; // this copying is unnecessary
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

            // postcondition: a[lo .. hi] is sorted
            Debug.Assert(IsSorted(a, lo, hi));
        }

        /// <summary>
        /// Mergesort a[lo..hi] using auxiliary array aux[lo..hi]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="aux"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        private static void Sort<T>(T[] a, T[] aux, int lo, int hi) where T : IComparable<T>
        {
            if (hi <= lo)
            {
                return;
            }
            int mid = lo + (hi - lo) / 2;
            Sort(a, aux, lo, mid);
            Sort(a, aux, mid + 1, hi);
            Merge(a, aux, lo, mid, hi);
        }

        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            T[] aux = new T[a.Length];
            Sort(a, aux, 0, a.Length - 1);
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
        /// Index mergesort
        /// stably Merge a[lo .. mid] with a[mid+1 .. hi] using aux[lo .. hi]
        /// </summary>
        private static void Merge<T>(T[] a, int[] index, int[] aux, int lo, int mid, int hi) where T : IComparable<T>
        {
            // copy to aux[]
            for (int k = lo; k <= hi; k++)
            {
                aux[k] = index[k];
            }

            // Merge back to a[]
            int i = lo, j = mid + 1;
            for (int k = lo; k <= hi; k++)
            {
                if (i > mid)
                {
                    index[k] = aux[j++];
                }
                else if (j > hi)
                {
                    index[k] = aux[i++];
                }
                else if (Less(a[aux[j]], a[aux[i]]))
                {
                    index[k] = aux[j++];
                }
                else
                {
                    index[k] = aux[i++];
                }
            }
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

            int[] aux = new int[n];
            Sort(a, index, aux, 0, n - 1);
            return index;
        }

        /// <summary>
        /// Mergesort a[lo..hi] using auxiliary array aux[lo..hi]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="index"></param>
        /// <param name="aux"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        private static void Sort<T>(T[] a, int[] index, int[] aux, int lo, int hi) where T : IComparable<T>
        {
            if (hi <= lo)
            {
                return;
            }
            int mid = lo + (hi - lo) / 2;
            Sort(a, index, aux, lo, mid);
            Sort(a, index, aux, mid + 1, hi);
            Merge(a, index, aux, lo, mid, hi);
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
        /// Reads in a sequence of strings from standard input; mergesorts them; 
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