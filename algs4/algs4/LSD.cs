using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LSD
    {
        private const int BitsPerByte = 8;

        /// <summary>
        /// LSD radix sort
        /// </summary>
        /// <param name="a"></param>
        /// <param name="w"></param>
        public static void Sort(string[] a, int w)
        {
            int n = a.Length;
            const int r = 256; // extend ASCII alphabet size
            string[] aux = new string[n];

            for (int d = w - 1; d >= 0; d--)
            {
                // sort by key-indexed counting on dth character

                // compute frequency counts
                int[] count = new int[r + 1];
                for (int i = 0; i < n; i++)
                {
                    count[a[i][d] + 1]++;
                }

                // compute cumulates
                for (int c = 0; c < r; c++)
                {
                    count[c + 1] += count[c];
                }

                // move data
                for (int i = 0; i < n; i++)
                {
                    aux[count[a[i][d]]++] = a[i];
                }

                // copy back
                for (int i = 0; i < n; i++)
                {
                    a[i] = aux[i];
                }
            }
        }

        /// <summary>
        /// LSD sort an array of integers, treating each int as 4 bytes
        /// assumes integers are nonnegative
        /// [ 2-3x faster than Arrays.sort() ]
        /// </summary>
        /// <param name="a"></param>
        public static void Sort(int[] a)
        {
            const int bits = 32; // each int is 32 bits 
            int w = bits / BitsPerByte; // each int is 4 bytes
            int r = 1 << BitsPerByte; // each bytes is between 0 and 255
            int mask = r - 1; // 0xFF

            int n = a.Length;
            int[] aux = new int[n];

            for (int d = 0; d < w; d++)
            {
                // compute frequency counts
                int[] count = new int[r + 1];
                for (int i = 0; i < n; i++)
                {
                    int c = (a[i] >> BitsPerByte * d) & mask;
                    count[c + 1]++;
                }

                // compute cumulates
                for (int c = 0; c < r; c++)
                {
                    count[c + 1] += count[c];
                }

                // for most significant byte, 0x80-0xFF comes before 0x00-0x7F
                if (d == w - 1)
                {
                    int shift1 = count[r] - count[r / 2];
                    int shift2 = count[r / 2];
                    for (int c = 0; c < r / 2; c++)
                    {
                        count[c] += shift1;
                    }
                    for (int c = r / 2; c < r; c++)
                    {
                        count[c] -= shift2;
                    }
                }

                // move data
                for (int i = 0; i < n; i++)
                {
                    int c = (a[i] >> BitsPerByte * d) & mask;
                    aux[count[c]++] = a[i];
                }

                // copy back
                for (int i = 0; i < n; i++)
                {
                    a[i] = aux[i];
                }
            }
        }

        public static void RunMain(string[] args)
        {
            string[] a = StdIn.ReadAllStrings();
            int n = a.Length;

            // check that strings have fixed length
            int w = a[0].Length;
            for (int i = 0; i < n; i++)
            {
                Debug.Assert(a[i].Length == w, "Strings must have fixed length");
            }

            // sort the strings
            Sort(a, w);

            // print results
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn(a[i]);
            }
        }
    }
}