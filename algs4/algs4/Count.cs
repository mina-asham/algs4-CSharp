using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Count
    {
        public static void RunMain(string[] args)
        {
            Alphabet alpha = new Alphabet(args[0]);
            int r = alpha.R();
            int[] count = new int[r];
            string a = StdIn.ReadAll();
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                if (alpha.Contains(a[i]))
                {
                    count[alpha.ToIndex(a[i])]++;
                }
            }
            for (int c = 0; c < r; c++)
            {
                StdOut.PrintLn(alpha.ToChar(c) + " " + count[c]);
            }
        }
    }
}
