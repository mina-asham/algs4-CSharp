using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using algs4.stdlib;

namespace algs4.algs4
{
    public class SuffixArray
    {
        private readonly Suffix[] _suffixes;

        /// <summary>
        /// Initializes a suffix array for the given text string.
        /// </summary>
        /// <param name="text">the input string</param>
        public SuffixArray(string text)
        {
            int n = text.Length;
            _suffixes = new Suffix[n];
            for (int i = 0; i < n; i++)
            {
                _suffixes[i] = new Suffix(text, i);
            }
            Array.Sort(_suffixes);
        }

        private class Suffix : IComparable<Suffix>
        {
            public string Text { get; set; }
            public int Index { get; set; }

            public Suffix(string text, int index)
            {
                Text = text;
                Index = index;
            }

            public int Length()
            {
                return Text.Length - Index;
            }

            public char CharAt(int i)
            {
                return Text[Index + i];
            }

            public int CompareTo(Suffix that)
            {
                // optimization
                if (this == that)
                {
                    return 0;
                }
                int n = Math.Min(Length(), that.Length());
                for (int i = 0; i < n; i++)
                {
                    if (CharAt(i) < that.CharAt(i))
                    {
                        return -1;
                    }
                    if (CharAt(i) > that.CharAt(i))
                    {
                        return +1;
                    }
                }
                return Length() - that.Length();
            }

            public override string ToString()
            {
                return Text.Substring(Index);
            }
        }

        /// <summary>
        /// Returns the length of the input string.
        /// </summary>
        /// <returns>the length of the input string</returns>
        public int Length()
        {
            return _suffixes.Length;
        }

        /// <summary>
        /// Returns the index into the original string of the ireadonlyth smallest suffix.
        /// That is, text.substring(sa.index(i)) is the ith smallest suffix.
        /// </summary>
        /// <param name="i">an integer between 0 and N-1</param>
        /// <returns>the index into the original string of the ith smallest suffix</returns>
        public int Index(int i)
        {
            if (i < 0 || i >= _suffixes.Length)
            {
                throw new IndexOutOfRangeException();
            }
            return _suffixes[i].Index;
        }

        /// <summary>
        /// Returns the length of the longest common prefix of the ith
        /// smallest suffix and the i-1st smallest suffix.
        /// </summary>
        /// <param name="i">an integer between 1 and N-1</param>
        /// <returns>the length of the longest common prefix of the ith smallest suffix and the i-1st smallest suffix.</returns>
        public int LCP(int i)
        {
            if (i < 1 || i >= _suffixes.Length)
            {
                throw new IndexOutOfRangeException();
            }
            return LCP(_suffixes[i], _suffixes[i - 1]);
        }

        /// <summary>
        /// Longest common prefix of s and t
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static int LCP(Suffix s, Suffix t)
        {
            int n = Math.Min(s.Length(), t.Length());
            for (int i = 0; i < n; i++)
            {
                if (s.CharAt(i) != t.CharAt(i))
                {
                    return i;
                }
            }
            return n;
        }

        /// <summary>
        /// Returns the ith smallest suffix as a string.
        /// </summary>
        /// <param name="i">the index</param>
        /// <returns>the i smallest suffix as a string</returns>
        public string Select(int i)
        {
            if (i < 0 || i >= _suffixes.Length)
            {
                throw new IndexOutOfRangeException();
            }
            return _suffixes[i].ToString();
        }

        /// <summary>
        /// Returns the number of suffixes strictly less than the query string.
        /// We note that rank(select(i)) equals i for each i
        /// between 0 and N-1.
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>the number of suffixes strictly less than query</returns>
        public int Rank(string query)
        {
            int lo = 0, hi = _suffixes.Length - 1;
            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                int cmp = Compare(query, _suffixes[mid]);
                if (cmp < 0)
                {
                    hi = mid - 1;
                }
                else if (cmp > 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    return mid;
                }
            }
            return lo;
        }

        /// <summary>
        /// Compare query string to suffix
        /// </summary>
        /// <param name="query"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        private static int Compare(string query, Suffix suffix)
        {
            int n = Math.Min(query.Length, suffix.Length());
            for (int i = 0; i < n; i++)
            {
                if (query[i] < suffix.CharAt(i))
                {
                    return -1;
                }
                if (query[i] > suffix.CharAt(i))
                {
                    return +1;
                }
            }
            return query.Length - suffix.Length();
        }

        /// <summary>
        /// Unit tests the SuffixArray data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string s = Regex.Replace(StdIn.ReadAll(), "\\s+", " ").Trim();
            SuffixArray suffix = new SuffixArray(s);

            StdOut.PrintLn("  i ind LCP rnk select");
            StdOut.PrintLn("---------------------------");

            for (int i = 0; i < s.Length; i++)
            {
                int index = suffix.Index(i);
                string ith = "\"" + s.Substring(index, Math.Min(index + 50, s.Length)) + "\"";

                Debug.Assert(s.Substring(index).Equals(suffix.Select(i)));

                int rank = suffix.Rank(s.Substring(index));
                if (i == 0)
                {
                    StdOut.PrintF("{0:000} {1:000} {2:000} {3:000} {4}\n", i, index, "-", rank, ith);
                }
                else
                {
                    int lcp = suffix.LCP(i);
                    StdOut.PrintF("{0:000} {1:000} {2:000} {3:000} {4}\n", i, index, lcp, rank, ith);
                }
            }
        }
    }
}