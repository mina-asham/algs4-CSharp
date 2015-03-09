using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algs4.algs4
{
    public class StaticSETofInts
    {
        private readonly int[] _a;

        /// <summary>
        /// Initializes a set of integers specified by the integer array.
        /// </summary>
        /// <param name="keys">the array of integers</param>
        public StaticSETofInts(int[] keys)
        {
            // defensive copy
            _a = new int[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                _a[i] = keys[i];
            }

            // sort the integers
            Array.Sort(_a);

            // check for duplicates
            for (int i = 1; i < _a.Length; i++)
            {
                if (_a[i] == _a[i - 1])
                {
                    throw new ArgumentException("Argument arrays contains duplicate keys.");
                }
            }
        }

        /// <summary>
        /// Is the key in this set of integers?
        /// </summary>
        /// <param name="key">the search key</param>
        /// <returns>if the set of integers contains the key; false otherwise</returns>
        public bool Contains(int key)
        {
            return Rank(key) != -1;
        }

        /// <summary>
        /// Returns either the index of the search key in the sorted array
        /// (if the key is in the set) or -1 (if the key is not in the set).</summary>
        /// <param name="key">the search key</param>
        /// <returns>the number of keys in this set less than the key (if the key is in the set)</returns>
        public int Rank(int key)
        {
            int lo = 0;
            int hi = _a.Length - 1;
            while (lo <= hi)
            {
                // Key is in a[lo..hi] or not present.
                int mid = lo + (hi - lo) / 2;
                if (key < _a[mid])
                {
                    hi = mid - 1;
                }
                else if (key > _a[mid])
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
    }
}
