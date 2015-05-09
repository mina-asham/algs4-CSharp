using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BipartiteMatching
    {
        public static void RunMain(string[] args)
        {
            // read in bipartite network with 2N vertices and E edges
            // we assume the vertices on one side of the bipartition
            // are named 0 to N-1 and on the other side are N to 2N-1.
            int n = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            int s = 2 * n, t = 2 * n + 1;
            FlowNetwork g = new FlowNetwork(2 * n + 2);
            for (int i = 0; i < e; i++)
            {
                int v = StdRandom.Uniform(n);
                int w = StdRandom.Uniform(n) + n;
                g.AddEdge(new FlowEdge(v, w, double.PositiveInfinity));
                StdOut.PrintLn(v + "-" + w);
            }
            for (int i = 0; i < n; i++)
            {
                g.AddEdge(new FlowEdge(s, i, 1.0));
                g.AddEdge(new FlowEdge(i + n, t, 1.0));
            }

            // compute maximum flow and minimum cut
            FordFulkerson maxflow = new FordFulkerson(g, s, t);
            StdOut.PrintLn();
            StdOut.PrintLn("Size of maximum matching = " + (int)maxflow.Value());
            for (int v = 0; v < n; v++)
            {
                foreach (FlowEdge edge in g.Adj(v))
                {
                    if (edge.From() == v && edge.Flow() > 0)
                    {
                        StdOut.PrintLn(edge.From() + "-" + edge.To());
                    }
                }
            }
        }
    }
}