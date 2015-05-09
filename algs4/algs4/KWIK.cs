using System;
using System.Text.RegularExpressions;
using algs4.stdlib;

namespace algs4.algs4
{
    public class KWIK
    {
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            int context = int.Parse(args[1]);

            // read input text
            string text = Regex.Replace(input.ReadAll(), "\\s+", " ");
            int n = text.Length;

            // build suffix array
            SuffixArray sa = new SuffixArray(text);

            // find all occurrences of queries and give context
            while (StdIn.HasNextLine())
            {
                string query = StdIn.ReadLine();
                for (int i = sa.Rank(query); i < n; i++)
                {
                    int from1 = sa.Index(i);
                    int to1 = Math.Min(n, from1 + query.Length);
                    if (query != text.Substring(from1, to1))
                    {
                        break;
                    }
                    int from2 = Math.Max(0, sa.Index(i) - context);
                    int to2 = Math.Min(n, sa.Index(i) + context + query.Length);
                    StdOut.PrintLn(text.Substring(from2, to2));
                }
                StdOut.PrintLn();
            }
        }
    }
}