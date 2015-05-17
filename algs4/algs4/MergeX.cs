using System;
using System.Collections;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class MergeX
    {
        /// <summary>
        /// Cutoff to insertion Sort
        /// </summary>
        private const int Cutoff = 7;

        private static void Merge<T>(T[] src, T[] dst, int lo, int mid, int hi) where T : IComparable<T>
        {
            // precondition: src[lo .. mid] and src[mid+1 .. hi] are sorted subarrays
            Debug.Assert(IsSorted(src, lo, mid));
            Debug.Assert(IsSorted(src, mid + 1, hi));

            int i = lo, j = mid + 1;
            for (int k = lo; k <= hi; k++)
            {
                if (i > mid)
                {
                    dst[k] = src[j++];
                }
                else if (j > hi)
                {
                    dst[k] = src[i++];
                }
                else if (Less(src[j], src[i]))
                {
                    dst[k] = src[j++]; // to ensure stability
                }
                else
                {
                    dst[k] = src[i++];
                }
            }

            // postcondition: dst[lo .. hi] is sorted subarray
            Debug.Assert(IsSorted(dst, lo, hi));
        }

        private static void Sort<T>(T[] src, T[] dst, int lo, int hi) where T : IComparable<T>
        {
            // if (hi <= lo) return;
            if (hi <= lo + Cutoff)
            {
                InsertionSort(dst, lo, hi);
                return;
            }
            int mid = lo + (hi - lo) / 2;
            Sort(dst, src, lo, mid);
            Sort(dst, src, mid + 1, hi);

            if (!Less(src[mid + 1], src[mid]))
            {
                Array.Copy(src, lo, dst, lo, hi - lo + 1);
                return;
            }

            Merge(src, dst, lo, mid, hi);
        }

        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            T[] aux = (T[])a.Clone();
            Sort(aux, a, 0, a.Length - 1);
            Debug.Assert(IsSorted(a));
        }

        // Sort from a[lo] to a[hi] using insertion Sort
        private static void InsertionSort<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > lo && Less(a[j], a[j - 1]); j--)
                {
                    Exch(a, j, j - 1);
                }
            }
        }

        #region Utility methods

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
        /// Is a[i] &lt; a[j]?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Less<T>(T a, T b) where T : IComparable<T>
        {
            return (a.CompareTo(b) < 0);
        }

        /// <summary>
        /// Is a[i] &lt; a[j]?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        private static bool Less<T>(T a, T b, IComparer comparator)
        {
            return (comparator.Compare(a, b) < 0);
        }

        #endregion

        #region Version that takes IComparer as argument

        /// <summary>
        /// Rearranges the array in ascending order, using the provided order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        /// <param name="comparator"></param>
        public static void Sort<T>(T[] a, IComparer comparator)
        {
            T[] aux = (T[])a.Clone();
            Sort(aux, a, 0, a.Length - 1, comparator);
            Debug.Assert(IsSorted(a, comparator));
        }

        private static void Merge<T>(T[] src, T[] dst, int lo, int mid, int hi, IComparer comparator)
        {
            // precondition: src[lo .. mid] and src[mid+1 .. hi] are sorted subarrays
            Debug.Assert(IsSorted(src, lo, mid, comparator));
            Debug.Assert(IsSorted(src, mid + 1, hi, comparator));

            int i = lo, j = mid + 1;
            for (int k = lo; k <= hi; k++)
            {
                if (i > mid)
                {
                    dst[k] = src[j++];
                }
                else if (j > hi)
                {
                    dst[k] = src[i++];
                }
                else if (Less(src[j], src[i], comparator))
                {
                    dst[k] = src[j++];
                }
                else
                {
                    dst[k] = src[i++];
                }
            }

            // postcondition: dst[lo .. hi] is sorted subarray
            Debug.Assert(IsSorted(dst, lo, hi, comparator));
        }

        private static void Sort<T>(T[] src, T[] dst, int lo, int hi, IComparer comparator)
        {
            // if (hi <= lo) return;
            if (hi <= lo + Cutoff)
            {
                InsertionSort(dst, lo, hi, comparator);
                return;
            }
            int mid = lo + (hi - lo) / 2;
            Sort(dst, src, lo, mid, comparator);
            Sort(dst, src, mid + 1, hi, comparator);

            // using System.arraycopy() is a bit faster than the above loop
            if (!Less(src[mid + 1], src[mid], comparator))
            {
                Array.Copy(src, lo, dst, lo, hi - lo + 1);
                return;
            }

            Merge(src, dst, lo, mid, hi, comparator);
        }

        /// <summary>
        /// Sort from a[lo] to a[hi] using insertion Sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="comparator"></param>
        private static void InsertionSort<T>(T[] a, int lo, int hi, IComparer comparator)
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > lo && Less(a[j], a[j - 1], comparator); j--)
                {
                    Exch(a, j, j - 1);
                }
            }
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

        private static bool IsSorted<T>(T[] a, IComparer comparator)
        {
            return IsSorted(a, 0, a.Length - 1, comparator);
        }

        private static bool IsSorted<T>(T[] a, int lo, int hi, IComparer comparator)
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
        /// <param name="a"></param>
        private static void Show<T>(T[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }

        /// <summary>
        /// Reads in a sequence of strings from standard input; mergesorts them
        /// (using an optimized version of mergesort); 
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