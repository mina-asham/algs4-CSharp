using algs4.stdlib;

namespace algs4.algs4
{
    public class FrequencyCounter
    {
        /// <summary>
        /// Reads in a command-line integer and sequence of words from
        /// standard input and prints out a word (whose length exceeds
        /// the threshold) that occurs most frequently to standard output.
        /// It also prints out the number of words whose length exceeds
        /// the threshold and the number of distinct such words.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int distinct = 0, words = 0;
            int minlen = int.Parse(args[0]);
            ST<string, int> st = new ST<string, int>();

            // compute frequency counts
            while (!StdIn.IsEmpty())
            {
                string key = StdIn.ReadString();
                if (key.Length < minlen)
                {
                    continue;
                }
                words++;
                if (st.Contains(key))
                {
                    st.Put(key, st.Get(key) + 1);
                }
                else
                {
                    st.Put(key, 1);
                    distinct++;
                }
            }

            // find a key with the highest frequency count
            string max = "";
            st.Put(max, 0);
            foreach (string word in st.Keys())
            {
                if (st.Get(word) > st.Get(max))
                {
                    max = word;
                }
            }

            StdOut.PrintLn(max + " " + st.Get(max));
            StdOut.PrintLn("distinct = " + distinct);
            StdOut.PrintLn("words    = " + words);
        }
    }
}