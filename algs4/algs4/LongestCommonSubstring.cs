using System;
using System.Text.RegularExpressions;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LongestCommonSubstring
    {
        public static void RunMain(string[] args)
        {
            // read in two string from two files
            In in1 = new In(args[0]);
            In in2 = new In(args[1]);
            string text1 = Regex.Replace(in1.ReadAll().Trim(), "\\s+", " ");
            string text2 = Regex.Replace(in2.ReadAll().Trim(), "\\s+", " ");
            int n1 = text1.Length;

            // concatenate two string with intervening '\1'
            string text = text1 + '\u0001' + text2;
            int n = text.Length;

            // compute suffix array of concatenated text
            SuffixArray suffix = new SuffixArray(text);

            // search for longest common substring
            string lcs = "";
            for (int i = 1; i < n; i++)
            {
                // adjacent suffixes both from first text string
                if (suffix.Index(i) < n1 && suffix.Index(i - 1) < n1)
                {
                    continue;
                }

                // adjacent suffixes both from secondt text string
                if (suffix.Index(i) > n1 && suffix.Index(i - 1) > n1)
                {
                    continue;
                }

                // check if adjacent suffixes longer common substring
                int length = suffix.LCP(i);
                if (length > lcs.Length)
                {
                    lcs = text.Substring(suffix.Index(i), suffix.Index(i) + length);
                }
            }

            // print out longest common substring
            StdOut.PrintLn(lcs.Length);
            StdOut.PrintLn("'" + lcs + "'");
        }
    }
}