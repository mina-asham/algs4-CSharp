using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LookupCSV
    {
        public static void RunMain(string[] args)
        {
            int keyField = int.Parse(args[1]);
            int valField = int.Parse(args[2]);

            // symbol table
            ST<string, string> st = new ST<string, string>();

            // read input the data from csv file
            In input = new In(args[0]);
            while (input.HasNextLine())
            {
                string line = input.ReadLine();
                string[] tokens = line.Split(',');
                string key = tokens[keyField];
                string val = tokens[valField];
                st.Put(key, val);
            }

            while (!StdIn.IsEmpty())
            {
                string s = StdIn.ReadString();
                if (st.Contains(s))
                {
                    StdOut.PrintLn(st.Get(s));
                }
                else
                {
                    StdOut.PrintLn("Not found");
                }
            }
        }
    }
}