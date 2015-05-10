using algs4.stdlib;

namespace algs4.algs4
{
    public static class DegreesOfSeparation
    {
        /// <summary>
        /// Reads in a social network from a file, and then repeatedly reads in
        /// individuals from standard input and prints out their degrees of
        /// separation.
        /// Takes three command-line arguments: the name of a file,
        /// a delimiter, and the name of the distinguished individual.
        /// Each line in the file contains the name of a vertex, followed by a
        /// list of the names of the vertices adjacent to that vertex,
        /// separated by the delimiter.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string filename = args[0];
            string delimiter = args[1];
            string source = args[2];

            // StdOut.PrintLn("Source: " + source);

            SymbolGraph sg = new SymbolGraph(filename, delimiter);
            Graph g = sg.G();
            if (!sg.Contains(source))
            {
                StdOut.PrintLn(source + " not in database.");
                return;
            }

            int s = sg.Index(source);
            BreadthFirstPaths bfs = new BreadthFirstPaths(g, s);

            while (!StdIn.IsEmpty())
            {
                string sink = StdIn.ReadLine();
                if (sg.Contains(sink))
                {
                    int t = sg.Index(sink);
                    if (bfs.HasPathTo(t))
                    {
                        foreach (int v in bfs.PathTo(t))
                        {
                            StdOut.PrintLn("   " + sg.Name(v));
                        }
                    }
                    else
                    {
                        StdOut.PrintLn("Not connected");
                    }
                }
                else
                {
                    StdOut.PrintLn("   Not in database.");
                }
            }
        }
    }
}