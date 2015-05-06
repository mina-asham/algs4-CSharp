using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class TransitiveClosure
    {
        // tc[v] = reachable from v
        private readonly DirectedDFS[] _tc;

        /// <summary>
        /// Computes the transitive closure of the digraph G.
        /// </summary>
        /// <param name="g">the digraph</param>
        public TransitiveClosure(Digraph g)
        {
            _tc = new DirectedDFS[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _tc[v] = new DirectedDFS(g, v);
            }
        }

        /// <summary>
        /// Is there a directed path from vertex v to vertex w Input the digraph?
        /// </summary>
        /// <param name="v">the source vertex</param>
        /// <param name="w">the target vertex</param>
        /// <returns>true if there is a directed path from v to w, false otherwise</returns>
        public bool Reachable(int v, int w)
        {
            return _tc[v].Marked(w);
        }

        /// <summary>
        /// Unit tests the TransitiveClosure data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);

            TransitiveClosure tc = new TransitiveClosure(g);

            // Print header
            StdOut.Print("     ");
            for (int v = 0; v < g.V(); v++)
            {
                StdOut.PrintF("{0:000}", v);
            }
            StdOut.PrintLn();
            StdOut.PrintLn("--------------------------------------------");

            // Print transitive closure
            for (int v = 0; v < g.V(); v++)
            {
                StdOut.PrintF("{0:000}: ", v);
                for (int w = 0; w < g.V(); w++)
                {
                    if (tc.Reachable(v, w))
                    {
                        StdOut.PrintF("  T");
                    }
                    else
                    {
                        StdOut.PrintF("   ");
                    }
                }
                StdOut.PrintLn();
            }
        }
    }
}
