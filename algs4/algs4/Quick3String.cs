using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Quick3String
    {
        /// <summary>
        /// Cutoff to insertion Sort
        /// </summary>
        private const int Cutoff = 15;

        /// <summary>
        /// Sort the array a[] of strings
        /// </summary>
        /// <param name="a"></param>
        public static void Sort(string[] a)
        {
            StdRandom.Shuffle(a);
            Sort(a, 0, a.Length - 1, 0);
            Debug.Assert(IsSorted(a));
        }

        /// <summary>
        /// Return the dth character of s, -1 if d = length of s
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static int CharAt(string s, int d)
        {
            Debug.Assert(d >= 0 && d <= s.Length);
            if (d == s.Length)
            {
                return -1;
            }
            return s[d];
        }

        /// <summary>
        /// 3-way string quicksort a[lo..hi] starting at dth character
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="d"></param>
        private static void Sort(string[] a, int lo, int hi, int d)
        {
            // cutoff to insertion Sort for small subarrays
            if (hi <= lo + Cutoff)
            {
                Insertion(a, lo, hi, d);
                return;
            }

            int lt = lo, gt = hi;
            int v = CharAt(a[lo], d);
            int i = lo + 1;
            while (i <= gt)
            {
                int t = CharAt(a[i], d);
                if (t < v)
                {
                    Exch(a, lt++, i++);
                }
                else if (t > v)
                {
                    Exch(a, i, gt--);
                }
                else
                {
                    i++;
                }
            }

            // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi]. 
            Sort(a, lo, lt - 1, d);
            if (v >= 0)
            {
                Sort(a, lt, gt, d + 1);
            }
            Sort(a, gt + 1, hi, d);
        }

        /// <summary>
        /// Sort from a[lo] to a[hi], starting at the dth character
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="d"></param>
        private static void Insertion(string[] a, int lo, int hi, int d)
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > lo && Less(a[j], a[j - 1], d); j--)
                {
                    Exch(a, j, j - 1);
                }
            }
        }

        /// <summary>
        /// Exchange a[i] and a[j]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private static void Exch(string[] a, int i, int j)
        {
            string temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }

        /// <summary>
        /// Is v less than w, starting at character d
        /// </summary>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static bool Less(string v, string w, int d)
        {
            Debug.Assert(v.Substring(0, d) == w.Substring(0, d));
            for (int i = d; i < Math.Min(v.Length, w.Length); i++)
            {
                if (v[i] < w[i])
                {
                    return true;
                }
                if (v[i] > w[i])
                {
                    return false;
                }
            }
            return v.Length < w.Length;
        }

        /// <summary>
        /// Is the array sorted
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static bool IsSorted(string[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (string.Compare(a[i], a[i - 1], StringComparison.Ordinal) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static void RunMain(string[] args)
        {
            // read in the strings from standard input
            string[] a = StdIn.ReadAllStrings();
            int n = a.Length;

            // Sort the strings
            Sort(a);

            // Print the results
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }
    }
}