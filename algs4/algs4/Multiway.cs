using algs4.stdlib;

namespace algs4.algs4
{
    public static class Multiway
    {
        /// <summary>
        /// Merge together the sorted input streams and write the sorted result to standard output
        /// </summary>
        /// <param name="streams"></param>
        private static void Merge(In[] streams)
        {
            int n = streams.Length;
            IndexMinPQ<string> pq = new IndexMinPQ<string>(n);
            for (int i = 0; i < n; i++)
            {
                if (!streams[i].IsEmpty())
                {
                    pq.Insert(i, streams[i].ReadString());
                }
            }

            // Extract and print min and read next from its stream. 
            while (!pq.IsEmpty())
            {
                StdOut.Print(pq.MinKey() + " ");
                int i = pq.DelMin();
                if (!streams[i].IsEmpty())
                {
                    pq.Insert(i, streams[i].ReadString());
                }
            }
            StdOut.PrintLn();
        }

        /// <summary>
        /// Reads sorted text files specified as command-line arguments;
        /// merges them together into a sorted output; and writes
        /// the results to standard output.
        /// Note: this client does not check that the input files are sorted.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int n = args.Length;
            In[] streams = new In[n];
            for (int i = 0; i < n; i++)
            {
                streams[i] = new In(args[i]);
            }
            Merge(streams);
        }
    }
}