using System.IO;
using algs4.stdlib;

namespace algs4.algs4
{
    public class FileIndex
    {
        public static void RunMain(string[] args)
        {
            // key = word, value = set of files containing that word
            ST<string, Set<FileInfo>> st = new ST<string, Set<FileInfo>>();

            // create inverted index of all files
            StdOut.PrintLn("Indexing files");
            foreach (string filename in args)
            {
                StdOut.PrintLn("  " + filename);
                FileStream file = new FileStream(filename, FileMode.Open);
                In input = new In(file);
                while (!input.IsEmpty())
                {
                    string word = input.ReadString();
                    if (!st.Contains(word))
                    {
                        st.Put(word, new Set<FileInfo>());
                    }
                    Set<FileInfo> set = st.Get(word);
                    set.Add(new FileInfo(filename));
                }
            }

            // read queries from standard input, one per line
            while (!StdIn.IsEmpty())
            {
                string query = StdIn.ReadString();
                if (st.Contains(query))
                {
                    Set<FileInfo> set = st.Get(query);
                    foreach (FileInfo file in set)
                    {
                        StdOut.PrintLn("  " + file.Name);
                    }
                }
            }
        }
    }
}