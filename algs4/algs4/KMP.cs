using algs4.stdlib;

namespace algs4.algs4
{
    public class KMP
    {
        /// <summary>
        /// The KMP automoton
        /// </summary>
        private readonly int[][] _dfa;

        /// <summary>
        /// Either the character array for the pattern
        /// </summary>
        private readonly char[] _pattern;

        /// <summary>
        /// Or the pattern string
        /// </summary>
        private readonly string _pat;

        /// <summary>
        /// Create the DFA from a string
        /// </summary>
        /// <param name="pat"></param>
        public KMP(string pat)
        {
            int r = 256;
            _pat = pat;

            // build DFA from pattern
            int m = pat.Length;
            _dfa = new int[r][];
            for (int i = 0; i < r; i++)
            {
                _dfa[i] = new int[m];
            }
            _dfa[pat[0]][0] = 1;
            for (int x = 0, j = 1; j < m; j++)
            {
                for (int c = 0; c < r; c++)
                {
                    _dfa[c][j] = _dfa[c][x]; // Copy mismatch cases. 
                }
                _dfa[pat[j]][j] = j + 1; // Set match case. 
                x = _dfa[pat[j]][x]; // Update restart state. 
            }
        }

        /// <summary>
        /// Create the DFA from a character array over R-character alphabet
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="r"></param>
        public KMP(char[] pattern, int r)
        {
            _pattern = new char[pattern.Length];
            for (int j = 0; j < pattern.Length; j++)
            {
                _pattern[j] = pattern[j];
            }

            // build DFA from pattern
            int m = pattern.Length;
            _dfa = new int[r][];
            for (int i = 0; i < r; i++)
            {
                _dfa[i] = new int[m];
            }
            _dfa[pattern[0]][0] = 1;
            for (int x = 0, j = 1; j < m; j++)
            {
                for (int c = 0; c < r; c++)
                {
                    _dfa[c][j] = _dfa[c][x]; // Copy mismatch cases. 
                }
                _dfa[pattern[j]][j] = j + 1; // Set match case. 
                x = _dfa[pattern[j]][x]; // Update restart state. 
            }
        }

        /// <summary>
        /// Return offset of first match; N if no match
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public int Search(string txt)
        {
            // simulate operation of DFA on text
            int m = _pat.Length;
            int n = txt.Length;
            int i, j;
            for (i = 0, j = 0; i < n && j < m; i++)
            {
                j = _dfa[txt[i]][j];
            }
            if (j == m)
            {
                return i - m; // found
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
            // simulate operation of DFA on text
            int m = _pattern.Length;
            int n = text.Length;
            int i, j;
            for (i = 0, j = 0; i < n && j < m; i++)
            {
                j = _dfa[text[i]][j];
            }
            if (j == m)
            {
                return i - m; // found
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

            KMP kmp1 = new KMP(pat);
            int offset1 = kmp1.Search(txt);

            KMP kmp2 = new KMP(pattern, 256);
            int offset2 = kmp2.Search(text);

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