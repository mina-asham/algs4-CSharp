using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class MSD
    {
        private const int BitsPerByte = 8;

        /// <summary>
        /// Each int is 32 bits 
        /// </summary>
        private const int BitsPerInt = 32;

        /// <summary>
        /// Extended ASCII alphabet size
        /// </summary>
        private const int R = 256;

        /// <summary>
        /// Cutoff to insertion sort
        /// </summary>
        private const int Cutoff = 15;

        /// <summary>
        /// Sort array of strings
        /// </summary>
        /// <param name="a"></param>
        public static void Sort(string[] a)
        {
            int n = a.Length;
            string[] aux = new string[n];
            Sort(a, 0, n - 1, 0, aux);
        }

        /// <summary>
        /// Return dth character of s, -1 if d =.Length of string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private static int ChatAt(string s, int d)
        {
            Debug.Assert(d >= 0 && d <= s.Length);
            if (d == s.Length)
            {
                return -1;
            }
            return s[d];
        }

        /// <summary>
        /// Sort from a[lo] to a[hi], starting at the dth character
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="d"></param>
        /// <param name="aux"></param>
        private static void Sort(string[] a, int lo, int hi, int d, string[] aux)
        {
            // cutoff to insertion sort for small subarrays
            if (hi <= lo + Cutoff)
            {
                Insertion(a, lo, hi, d);
                return;
            }

            // compute frequency counts
            int[] count = new int[R + 2];
            for (int i = lo; i <= hi; i++)
            {
                int c = ChatAt(a[i], d);
                count[c + 2]++;
            }

            // transform counts to indicies
            for (int r = 0; r < R + 1; r++)
            {
                count[r + 1] += count[r];
            }

            // distribute
            for (int i = lo; i <= hi; i++)
            {
                int c = ChatAt(a[i], d);
                aux[count[c + 1]++] = a[i];
            }

            // copy back
            for (int i = lo; i <= hi; i++)
            {
                a[i] = aux[i - lo];
            }

            // recursively sort for each character
            for (int r = 0; r < R; r++)
            {
                Sort(a, lo + count[r], lo + count[r + 1] - 1, d + 1, aux);
            }
        }

        /// <summary>
        /// Insertion sort a[lo..hi], starting at dth character
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

        // MSD sort array of integers
        public static void Sort(int[] a)
        {
            int n = a.Length;
            int[] aux = new int[n];
            Sort(a, 0, n - 1, 0, aux);
        }

        /// <summary>
        /// MSD sort from a[lo] to a[hi], starting at the dth byte
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="d"></param>
        /// <param name="aux"></param>
        private static void Sort(int[] a, int lo, int hi, int d, int[] aux)
        {
            // cutoff to insertion sort for small subarrays
            if (hi <= lo + Cutoff)
            {
                Insertion(a, lo, hi);
                return;
            }

            // compute frequency counts (need R = 256)
            int[] count = new int[R + 1];
            int mask = R - 1; // 0xFF;
            int shift = BitsPerInt - BitsPerByte * d - BitsPerByte;
            for (int i = lo; i <= hi; i++)
            {
                int c = (a[i] >> shift) & mask;
                count[c + 1]++;
            }

            // transform counts to indicies
            for (int r = 0; r < R; r++)
            {
                count[r + 1] += count[r];
            }

            // distribute
            for (int i = lo; i <= hi; i++)
            {
                int c = (a[i] >> shift) & mask;
                aux[count[c]++] = a[i];
            }

            // copy back
            for (int i = lo; i <= hi; i++)
            {
                a[i] = aux[i - lo];
            }

            // no more bits
            if (d == 4)
            {
                return;
            }

            // recursively sort for each character
            if (count[0] > 0)
            {
                Sort(a, lo, lo + count[0] - 1, d + 1, aux);
            }
            for (int r = 0; r < R; r++)
            {
                if (count[r + 1] > count[r])
                {
                    Sort(a, lo + count[r], lo + count[r + 1] - 1, d + 1, aux);
                }
            }
        }

        /// <summary>
        /// Insertion sort a[lo..hi], starting at dth character
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        private static void Insertion(int[] a, int lo, int hi)
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > lo && a[j] < a[j - 1]; j--)
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
        private static void Exch(int[] a, int i, int j)
        {
            int temp = a[i];
            a[i] = a[j];
            a[j] = temp;
        }

        public static void RunMain(string[] args)
        {
            string[] a = StdIn.ReadAllStrings();
            int n = a.Length;
            Sort(a);
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }
    }
}