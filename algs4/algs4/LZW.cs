using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LZW
    {
        /// <summary>
        /// Number of input chars
        /// </summary>
        private const int R = 256;

        /// <summary>
        /// Number of codewords = 2^W
        /// </summary>
        private const int L = 4096;

        public static void Compress()
        {
            String input = BinaryStdIn.ReadString();
            TST<int> st = new TST<int>();
            for (int i = 0; i < R; i++)
            {
                st.Put("" + (char)i, i);
            }
            int code = R + 1; // R is codeword for EOF

            while (input.Length > 0)
            {
                String s = st.LongestPrefixOf(input); // Find max prefix match s.
                BinaryStdOut.Write(st.Get(s)); // Print s's encoding.
                int t = s.Length;
                if (t < input.Length && code < L) // Add s to symbol table.
                {
                    st.Put(input.Substring(0, t + 1), code++);
                }
                input = input.Substring(t); // Scan past s in input.
            }
            BinaryStdOut.Write(R);
            BinaryStdOut.Close();
        }

        public static void Expand()
        {
            String[] st = new String[L];
            int i; // next available codeword value

            // initialize symbol table with all 1-character strings
            for (i = 0; i < R; i++)
            {
                st[i] = "" + (char)i;
            }
            st[i++] = ""; // (unused) lookahead for EOF

            int codeword = BinaryStdIn.ReadInt();
            if (codeword == R)
            {
                return; // expanded message is empty string
            }
            String val = st[codeword];

            while (true)
            {
                BinaryStdOut.Write(val);
                codeword = BinaryStdIn.ReadInt();
                if (codeword == R)
                {
                    break;
                }
                String s = st[codeword];
                if (i == codeword)
                {
                    s = val + val[0]; // special case hack
                }
                if (i < L)
                {
                    st[i++] = val + s[0];
                }
                val = s;
            }
            BinaryStdOut.Close();
        }

        public static void RunMain(String[] args)
        {
            if (args[0] == "-")
            {
                Compress();
            }
            else if (args[0] == "+")
            {
                Expand();
            }
            else
            {
                throw new ArgumentException("Illegal command line argument");
            }
        }
    }
}