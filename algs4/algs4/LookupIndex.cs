using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LookupIndex
    {
        public static void RunMain(string[] args)
        {
            string filename = args[0];
            string separator = args[1];
            In input = new In(filename);

            ST<string, Queue<string>> st = new ST<string, Queue<string>>();
            ST<string, Queue<string>> ts = new ST<string, Queue<string>>();

            while (input.HasNextLine())
            {
                string line = input.ReadLine();
                string[] fields = line.Split(new[] { separator }, StringSplitOptions.None);
                string key = fields[0];
                for (int i = 1; i < fields.Length; i++)
                {
                    string val = fields[i];
                    if (!st.Contains(key))
                    {
                        st.Put(key, new Queue<string>());
                    }
                    if (!ts.Contains(val))
                    {
                        ts.Put(val, new Queue<string>());
                    }
                    st.Get(key).Enqueue(val);
                    ts.Get(val).Enqueue(key);
                }
            }

            StdOut.PrintLn("Done indexing");

            // read queries from standard input, one per line
            while (!StdIn.IsEmpty())
            {
                string query = StdIn.ReadLine();
                if (st.Contains(query))
                {
                    foreach (string vals in st.Get(query))
                    {
                        StdOut.PrintLn("  " + vals);
                    }
                }
                if (ts.Contains(query))
                {
                    foreach (string keys in ts.Get(query))
                    {
                        StdOut.PrintLn("  " + keys);
                    }
                }
            }
        }
    }
}