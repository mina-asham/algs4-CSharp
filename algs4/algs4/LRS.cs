using System.Text.RegularExpressions;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LRS
    {
        public static void RunMain(string[] args)
        {
            string text = Regex.Replace(StdIn.ReadAll(), "\\s+", " ");
            SuffixArray sa = new SuffixArray(text);

            int n = sa.Length();

            string lrs = "";
            for (int i = 1; i < n; i++)
            {
                int length = sa.LCP(i);
                if (length > lrs.Length)
                {
                    // lrs = sa.select(i).substring(0, length);
                    lrs = text.Substring(sa.Index(i), sa.Index(i) + length);
                }
            }

            StdOut.PrintLn("'" + lrs + "'");
        }
    }
}