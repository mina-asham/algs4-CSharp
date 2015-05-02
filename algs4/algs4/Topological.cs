using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Topological
    {
        /// <summary>
        /// Topological order
        /// </summary>
        private readonly IEnumerable<int> _order;

        /// <summary>
        /// Determines whether the digraph G has a topological order and, if so,
        /// finds such a topological order.
        /// </summary>
        /// <param name="g">the digraph</param>
        public Topological(Digraph g)
        {
            DirectedCycle finder = new DirectedCycle(g);
            if (!finder.HasCycle())
            {
                DepthFirstOrder dfs = new DepthFirstOrder(g);
                _order = dfs.ReversePost();
            }
        }

        /// <summary>
        /// Determines whether the edge-weighted digraph G has a topological
        /// order and, if so, finds such an order.
        /// </summary>
        /// <param name="g">the edge-weighted digraph</param>
        public Topological(EdgeWeightedDigraph g)
        {
            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(g);
            if (!finder.HasCycle())
            {
                DepthFirstOrder dfs = new DepthFirstOrder(g);
                _order = dfs.ReversePost();
            }
        }

        /// <summary>
        /// Returns a topological order if the digraph has a topologial order,
        /// and null otherwise.
        /// </summary>
        /// <returns>a topological order of the vertices (as an interable) if the digraph has a topological order (or equivalently, if the digraph is a DAG), and null otherwise</returns>
        public IEnumerable<int> Order()
        {
            return _order;
        }

        /// <summary>
        /// Does the digraph have a topological order?
        /// </summary>
        /// <returns>true if the digraph has a topological order (or equivalently, if the digraph is a DAG), and false otherwise</returns>
        public bool HasOrder()
        {
            return _order != null;
        }

        /// <summary>
        /// Unit tests the Topological data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string filename = args[0];
            string delimiter = args[1];
            SymbolDigraph sg = new SymbolDigraph(filename, delimiter);
            Topological topological = new Topological(sg.G());
            foreach (int v in topological.Order())
            {
                StdOut.PrintLn(sg.Name(v));
            }
        }
    }
}
