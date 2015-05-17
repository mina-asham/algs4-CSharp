using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class QuickX
    {
        /// <summary>
        /// Cutoff to insertion Sort, must be &gt;= 1
        /// </summary>
        private const int Cutoff = 8;

        /// <summary>
        /// Rearranges the array in ascending order, using the natural order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array to be sorted</param>
        public static void Sort<T>(T[] a) where T : IComparable<T>
        {
            Sort(a, 0, a.Length - 1);
            Debug.Assert(IsSorted(a));
        }

        private static void Sort<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            int n = hi - lo + 1;

            // cutoff to insertion Sort
            if (n <= Cutoff)
            {
                InsertionSort(a, lo, hi);
                return;
            }

            // use median-of-3 as partitioning element
            else if (n <= 40)
            {
                int m = Median3(a, lo, lo + n / 2, hi);
                Exch(a, m, lo);
            }

            // use Tukey ninther as partitioning element
            else
            {
                int eps = n / 8;
                int mid = lo + n / 2;
                int m1 = Median3(a, lo, lo + eps, lo + eps + eps);
                int m2 = Median3(a, mid - eps, mid, mid + eps);
                int m3 = Median3(a, hi - eps - eps, hi - eps, hi);
                int ninther = Median3(a, m1, m2, m3);
                Exch(a, ninther, lo);
            }

            // Bentley-McIlroy 3-way partitioning
            int i = lo, j = hi + 1;
            int p = lo, q = hi + 1;
            T v = a[lo];
            while (true)
            {
                while (Less(a[++i], v))
                {
                    if (i == hi)
                    {
                        break;
                    }
                }
                while (Less(v, a[--j]))
                {
                    if (j == lo)
                    {
                        break;
                    }
                }

                // pointers cross
                if (i == j && Eq(a[i], v))
                {
                    Exch(a, ++p, i);
                }
                if (i >= j)
                {
                    break;
                }

                Exch(a, i, j);
                if (Eq(a[i], v))
                {
                    Exch(a, ++p, i);
                }
                if (Eq(a[j], v))
                {
                    Exch(a, --q, j);
                }
            }

            i = j + 1;
            for (int k = lo; k <= p; k++)
            {
                Exch(a, k, j--);
            }
            for (int k = hi; k >= q; k--)
            {
                Exch(a, k, i++);
            }

            Sort(a, lo, j);
            Sort(a, i, hi);
        }

        /// <summary>
        /// Sort from a[lo] to a[hi] using insertion Sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
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

        /// <summary>
        /// Return the index of the median element among a[i], a[j], and a[k]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private static int Median3<T>(T[] a, int i, int j, int k) where T : IComparable<T>
        {
            return (Less(a[i], a[j]) ?
                (Less(a[j], a[k]) ? j : Less(a[i], a[k]) ? k : i) :
                (Less(a[k], a[j]) ? j : Less(a[k], a[i]) ? k : i));
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
        /// Does v == w ?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static bool Eq<T>(T v, T w) where T : IComparable<T>
        {
            return (v.CompareTo(w) == 0);
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
        /// Reads in a sequence of strings from standard input; quicksorts them
        /// (using an optimized version of quicksort); 
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