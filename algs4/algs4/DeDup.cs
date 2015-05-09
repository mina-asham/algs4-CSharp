using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DeDup
    {
        public static void RunMain(string[] args)
        {
            Set<string> set = new Set<string>();

            // read in strings and add to set
            while (!StdIn.IsEmpty())
            {
                string key = StdIn.ReadString();
                if (!set.Contains(key))
                {
                    set.Add(key);
                    StdOut.PrintLn(key);
                }
            }
        }
    }
}