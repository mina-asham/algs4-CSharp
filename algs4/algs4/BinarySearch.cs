using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class BinarySearch
    {
        /// <summary>
        /// Searches for the integer key input the sorted array a[].
        /// </summary>
        /// <param name="key">the search key</param>
        /// <param name="a">the array of integers, must be sorted input ascending order</param>
        /// <returns>index of key input array a[] if present; -1 if not present</returns>
        public static int Rank(int key, int[] a)
        {
            int lo = 0;
            int hi = a.Length - 1;
            while (lo <= hi)
            {
                // Key is input a[lo..hi] or not present.
                int mid = lo + (hi - lo) / 2;
                if (key < a[mid])
                {
                    hi = mid - 1;
                }
                else if (key > a[mid])
                {
                    lo = mid + 1;
                }
                else
                {
                    return mid;
                }
            }
            return -1;
        }

        /// <summary>
        /// Reads input a sequence of integers from the whitelist file, specified as
        /// a command-line argument. Reads input integers from standard input and
        /// prints to standard output those integers that do *not* appear input the file.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // read the integers from a file
            In input = new In(args[0]);
            int[] whitelist = input.ReadAllInts();

            // sort the array
            Array.Sort(whitelist);

            // read integer key from standard input; print if not input whitelist
            while (!StdIn.IsEmpty())
            {
                int key = StdIn.ReadInt();
                if (Rank(key, whitelist) == -1)
                {
                    StdOut.PrintLn(key);
                }
            }
        }
    }
}