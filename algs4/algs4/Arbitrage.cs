using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class Arbitrage
    {
        /// <summary>
        /// Reads the currency exchange table from standard input and
        /// prints an arbitrage opportunity to standard output (if one exists).
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // V currencies
            int v = StdIn.ReadInt();
            string[] name = new string[v];

            // create complete network
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(v);
            for (int vertex = 0; vertex < v; vertex++)
            {
                name[vertex] = StdIn.ReadString();
                for (int w = 0; w < v; w++)
                {
                    double rate = StdIn.ReadDouble();
                    DirectedEdge e = new DirectedEdge(vertex, w, -Math.Log(rate));
                    g.AddEdge(e);
                }
            }

            // find negative cycle
            BellmanFordSP spt = new BellmanFordSP(g, 0);
            if (spt.HasNegativeCycle())
            {
                double stake = 1000.0;
                foreach (DirectedEdge e in spt.NegativeCycle())
                {
                    StdOut.PrintF("{0:0000000000.00000} {1} ", stake, name[e.From()]);
                    stake *= Math.Exp(-e.Weight());
                    StdOut.PrintF("= {0:0000000000.00000} {1}\n", stake, name[e.To()]);
                }
            }
            else
            {
                StdOut.PrintLn("No arbitrage opportunity");
            }
        }
    }
}