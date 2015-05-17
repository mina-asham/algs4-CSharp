using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class SuffixArrayX
    {
        /// <summary>
        /// Cutoff to insertion sort (any value between 0 and 12)
        /// </summary>
        private const int Cutoff = 5;

        private readonly char[] _text;

        /// <summary>
        /// index[i] = j means text.Substring(j) is ith largest suffix
        /// </summary>
        private readonly int[] _index;

        /// <summary>
        /// Number of characters in text
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// Initializes a suffix array for the given text string.
        /// </summary>
        /// <param name="text">the input string</param>
        public SuffixArrayX(string text)
        {
            _n = text.Length;
            text = text + '\0';
            _text = text.ToCharArray();
            _index = new int[_n];
            for (int i = 0; i < _n; i++)
            {
                _index[i] = i;
            }

            Sort(0, _n - 1, 0);
        }

        /// <summary>
        /// 3-way string quicksort lo..hi starting at dth character
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="d"></param>
        private void Sort(int lo, int hi, int d)
        {
            // cutoff to insertion sort for small subarrays
            if (hi <= lo + Cutoff)
            {
                Insertion(lo, hi, d);
                return;
            }

            int lt = lo, gt = hi;
            char v = _text[_index[lo] + d];
            int i = lo + 1;
            while (i <= gt)
            {
                char t = _text[_index[i] + d];
                if (t < v)
                {
                    Exch(lt++, i++);
                }
                else if (t > v)
                {
                    Exch(i, gt--);
                }
                else
                {
                    i++;
                }
            }

            // a[lo..lt-1] < v = a[lt..gt] < a[gt+1..hi]. 
            Sort(lo, lt - 1, d);
            if (v > 0)
            {
                Sort(lt, gt, d + 1);
            }
            Sort(gt + 1, hi, d);
        }

        /// <summary>
        /// Sort from a[lo] to a[hi], starting at the dth character
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="d"></param>
        private void Insertion(int lo, int hi, int d)
        {
            for (int i = lo; i <= hi; i++)
            {
                for (int j = i; j > lo && Less(_index[j], _index[j - 1], d); j--)
                {
                    Exch(j, j - 1);
                }
            }
        }

        /// <summary>
        /// Is text[i+d..N) &lt; text[j+d..N) ?
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private bool Less(int i, int j, int d)
        {
            if (i == j)
            {
                return false;
            }
            i = i + d;
            j = j + d;
            while (i < _n && j < _n)
            {
                if (_text[i] < _text[j])
                {
                    return true;
                }
                if (_text[i] > _text[j])
                {
                    return false;
                }
                i++;
                j++;
            }
            return i > j;
        }

        /// <summary>
        /// Exchange index[i] and index[j]
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Exch(int i, int j)
        {
            int swap = _index[i];
            _index[i] = _index[j];
            _index[j] = swap;
        }

        /// <summary>
        /// Returns the length of the input string.
        /// </summary>
        /// <returns>the length of the input string</returns>
        public int Length()
        {
            return _n;
        }

        /// <summary>
        /// Returns the index into the original string of the ith smallest suffix.
        /// That is, text.Substring(sa.Index(i)) is the i smallest suffix.
        /// </summary>
        /// <param name="i">an integer between 0 and N-1</param>
        /// <returns>the index into the original string of the ith smallest suffix</returns>
        public int Index(int i)
        {
            if (i < 0 || i >= _n)
            {
                throw new IndexOutOfRangeException();
            }
            return _index[i];
        }

        /// <summary>
        /// Returns the length of the longest common prefix of the ith
        /// smallest suffix and the i-1st smallest suffix.
        /// </summary>
        /// <param name="i">an integer between 1 and N-1</param>
        /// <returns>the length of the longest common prefix of the ith smallest suffix and the i-1st smallest suffix.</returns>
        public int LCP(int i)
        {
            if (i < 1 || i >= _n)
            {
                throw new IndexOutOfRangeException();
            }
            return LCP(_index[i], _index[i - 1]);
        }

        // longest common prefix of text[i..N) and text[j..N)
        private int LCP(int i, int j)
        {
            int length = 0;
            while (i < _n && j < _n)
            {
                if (_text[i] != _text[j])
                {
                    return length;
                }
                i++;
                j++;
                length++;
            }
            return length;
        }

        /// <summary>
        /// Returns the ith smallest suffix as a string.
        /// </summary>
        /// <param name="i">the index</param>
        /// <returns>the i smallest suffix as a string</returns>
        public string Select(int i)
        {
            if (i < 0 || i >= _n)
            {
                throw new IndexOutOfRangeException();
            }
            return new string(_text, _index[i], _n - _index[i]);
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
            int lo = 0, hi = _n - 1;
            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                int cmp = Compare(query, _index[mid]);
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
        /// Is query &lt; text[i..N) ?
        /// </summary>
        /// <param name="query"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private int Compare(string query, int i)
        {
            int m = query.Length;
            int j = 0;
            while (i < _n && j < m)
            {
                if (query[j] != _text[i])
                {
                    return query[j] - _text[i];
                }
                i++;
                j++;
            }
            if (i < _n)
            {
                return -1;
            }
            if (j < m)
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Unit tests the SuffixArrayx data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string s = StdIn.ReadAll().Replace("\n", " ").Trim();
            SuffixArrayX suffix = new SuffixArrayX(s);

            SuffixArray suffixReference = new SuffixArray(s);
            bool check = true;
            for (int i = 0; check && i < s.Length; i++)
            {
                if (suffixReference.Index(i) != suffix.Index(i))
                {
                    StdOut.PrintLn("suffixReference(" + i + ") = " + suffixReference.Index(i));
                    StdOut.PrintLn("suffix(" + i + ") = " + suffix.Index(i));
                    string ith = "\"" + s.Substring(suffix.Index(i), Math.Min(suffix.Index(i) + 50, s.Length)) + "\"";
                    string jth = "\"" + s.Substring(suffixReference.Index(i), Math.Min(suffixReference.Index(i) + 50, s.Length)) + "\"";
                    StdOut.PrintLn(ith);
                    StdOut.PrintLn(jth);
                    check = false;
                }
            }

            StdOut.PrintLn("  i ind LCP rnk  select");
            StdOut.PrintLn("---------------------------");

            for (int i = 0; i < s.Length; i++)
            {
                int index = suffix.Index(i);
                string ith = "\"" + s.Substring(index, Math.Min(index + 50, s.Length)) + "\"";
                int rank = suffix.Rank(s.Substring(index));
                Debug.Assert(s.Substring(index) == suffix.Select(i));
                if (i == 0)
                {
                    StdOut.PrintF("{0:000} {1:000} {2:000} {3:000}  {4}\n", i, index, "-", rank, ith);
                }
                else
                {
                    // int LCP  = suffix.LCP(suffix.Index(i), suffix.Index(i-1));
                    int lcp = suffix.LCP(i);
                    StdOut.PrintF("{0:000} {1:000} {2:000} {3:000}  {4}\n", i, index, lcp, rank, ith);
                }
            }
        }
    }
}