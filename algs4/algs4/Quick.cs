using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Quick
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

        // quicksort the subarray from a[lo] to a[hi]
        private static void Sort<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            if (hi <= lo)
            {
                return;
            }
            int j = Partition(a, lo, hi);
            Sort(a, lo, j - 1);
            Sort(a, j + 1, hi);
            Debug.Assert(IsSorted(a, lo, hi));
        }

        /// <summary>
        /// Partition the subarray a[lo..hi] so that a[lo..j-1] &lt;= a[j] &lt;= a[j+1..hi]
        /// and return the index j.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        private static int Partition<T>(T[] a, int lo, int hi) where T : IComparable<T>
        {
            int i = lo;
            int j = hi + 1;
            T v = a[lo];
            while (true)
            {
                // find item on lo to swap
                while (Less(a[++i], v))
                {
                    if (i == hi)
                    {
                        break;
                    }
                }

                // find item on hi to swap
                while (Less(v, a[--j]))
                {
                    if (j == lo)
                    {
                        break; // redundant since a[lo] acts as sentinel
                    }
                }

                // check if pointers cross
                if (i >= j)
                {
                    break;
                }

                Exch(a, i, j);
            }

            // Put partitioning item v at a[j]
            Exch(a, lo, j);

            // now, a[lo .. j-1] <= a[j] <= a[j+1 .. hi]
            return j;
        }

        /// <summary>
        /// Rearranges the array so that a[k] contains the kth smallest key;
        /// a[0] through a[k-1] are less than (or equal to) a[k]; and
        /// a[k+1] through a[N-1] are greater than (or equal to) a[k].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a">the array</param>
        /// <param name="k">find the kth smallest</param>
        /// <returns></returns>
        public static T Select<T>(T[] a, int k) where T : IComparable<T>
        {
            if (k < 0 || k >= a.Length)
            {
                throw new IndexOutOfRangeException("Selected element out of bounds");
            }
            StdRandom.Shuffle(a);
            int lo = 0, hi = a.Length - 1;
            while (hi > lo)
            {
                int i = Partition(a, lo, hi);
                if (i > k)
                {
                    hi = i - 1;
                }
                else if (i < k)
                {
                    lo = i + 1;
                }
                else
                {
                    return a[i];
                }
            }
            return a[lo];
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
        /// Reads in a sequence of strings from standard input; quicksorts them; 
        /// and prints them to standard output in ascending order. 
        /// Shuffles the array and then prints the strings again to
        /// standard output, but this time, using the select method.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string[] a = StdIn.ReadAllStrings();
            Sort(a);
            Show(a);

            // shuffle
            StdRandom.Shuffle(a);

            // display results again using select
            StdOut.PrintLn();
            for (int i = 0; i < a.Length; i++)
            {
                string ith = Select(a, i);
                StdOut.PrintLn(ith);
            }
        }
    }
}