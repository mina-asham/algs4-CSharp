using algs4.stdlib;

namespace algs4.algs4
{
    public static class CPM
    {
        /// <summary>
        ///  Reads the precedence constraints from standard input
        ///  and prints a feasible schedule to standard output.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // number of jobs
            int n = StdIn.ReadInt();

            // source and sink
            int source = 2 * n;
            int sink = 2 * n + 1;

            // build network
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(2 * n + 2);
            for (int i = 0; i < n; i++)
            {
                double duration = StdIn.ReadDouble();
                g.AddEdge(new DirectedEdge(source, i, 0.0));
                g.AddEdge(new DirectedEdge(i + n, sink, 0.0));
                g.AddEdge(new DirectedEdge(i, i + n, duration));

                // precedence constraints
                int m = StdIn.ReadInt();
                for (int j = 0; j < m; j++)
                {
                    int precedent = StdIn.ReadInt();
                    g.AddEdge(new DirectedEdge(n + i, precedent, 0.0));
                }
            }

            // compute longest path
            AcyclicLP lp = new AcyclicLP(g, source);

            // print results
            StdOut.PrintLn(" job   start  finish");
            StdOut.PrintLn("--------------------");
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintF("{0:0000} {1:0000000.0} {2:0000000.0}\n", i, lp.DistTo(i), lp.DistTo(i + n));
            }
            StdOut.PrintF("Finish time: {0:0000000.0}\n", lp.DistTo(sink));
        }
    }
}