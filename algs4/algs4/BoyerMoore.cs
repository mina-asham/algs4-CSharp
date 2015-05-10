using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BoyerMoore
    {
        /// <summary>
        /// Tthe bad-character skip array
        /// </summary>
        private readonly int[] _right;

        /// <summary>
        /// Store the pattern as a character array
        /// </summary>
        private readonly char[] _pattern;

        /// <summary>
        /// Or as a string
        /// </summary>
        private readonly string _pat;

        /// <summary>
        /// Pattern provided as a string
        /// </summary>
        /// <param name="pat"></param>
        public BoyerMoore(string pat)
        {
            const int r = 256;
            _pat = pat;

            // position of rightmost occurrence of c in the pattern
            _right = new int[r];
            for (int c = 0; c < r; c++)
            {
                _right[c] = -1;
            }
            for (int j = 0; j < pat.Length; j++)
            {
                _right[pat[j]] = j;
            }
        }

        /// <summary>
        /// Pattern provided as a character array
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="r"></param>
        public BoyerMoore(char[] pattern, int r)
        {
            _pattern = new char[pattern.Length];
            for (int j = 0; j < pattern.Length; j++)
            {
                _pattern[j] = pattern[j];
            }

            // position of rightmost occurrence of c in the pattern
            _right = new int[r];
            for (int c = 0; c < r; c++)
            {
                _right[c] = -1;
            }
            for (int j = 0; j < pattern.Length; j++)
            {
                _right[pattern[j]] = j;
            }
        }

        /// <summary>
        /// Return offset of first match; N if no match
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public int Search(string txt)
        {
            int m = _pat.Length;
            int n = txt.Length;
            int skip;
            for (int i = 0; i <= n - m; i += skip)
            {
                skip = 0;
                for (int j = m - 1; j >= 0; j--)
                {
                    if (_pat[j] != txt[i + j])
                    {
                        skip = Math.Max(1, j - _right[txt[i + j]]);
                        break;
                    }
                }
                if (skip == 0)
                {
                    return i; // found
                }
            }
            return n; // not found
        }

        /// <summary>
        /// Return offset of first match; N if no match
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Search(char[] text)
        {
            int m = _pattern.Length;
            int n = text.Length;
            int skip;
            for (int i = 0; i <= n - m; i += skip)
            {
                skip = 0;
                for (int j = m - 1; j >= 0; j--)
                {
                    if (_pattern[j] != text[i + j])
                    {
                        skip = Math.Max(1, j - _right[text[i + j]]);
                        break;
                    }
                }
                if (skip == 0)
                {
                    return i; // found
                }
            }
            return n; // not found
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string pat = args[0];
            string txt = args[1];
            char[] pattern = pat.ToCharArray();
            char[] text = txt.ToCharArray();

            BoyerMoore boyermoore1 = new BoyerMoore(pat);
            BoyerMoore boyermoore2 = new BoyerMoore(pattern, 256);
            int offset1 = boyermoore1.Search(txt);
            int offset2 = boyermoore2.Search(text);

            // Print results
            StdOut.PrintLn("text:    " + txt);

            StdOut.Print("pattern: ");
            for (int i = 0; i < offset1; i++)
            {
                StdOut.Print(" ");
            }
            StdOut.PrintLn(pat);

            StdOut.Print("pattern: ");
            for (int i = 0; i < offset2; i++)
            {
                StdOut.Print(" ");
            }
            StdOut.PrintLn(pat);
        }
    }
}