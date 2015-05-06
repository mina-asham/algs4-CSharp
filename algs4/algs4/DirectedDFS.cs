using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DirectedDFS
    {
        /// <summary>
        /// marked[v] = true if v is reachable
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// number of vertices reachable from s
        /// </summary>
        private int _count;

        /// <summary>
        /// Computes the vertices in digraph G that are
        /// reachable from the source vertex s.
        /// </summary>
        /// <param name="g">the digraph</param>
        /// <param name="s">the source vertex</param>
        public DirectedDFS(Digraph g, int s)
        {
            _marked = new bool[g.V()];
            DFS(g, s);
        }

        /// <summary>
        /// Computes the vertices in digraph G that are
        /// connected to any of the source vertices sources.
        /// </summary>
        /// <param name="g">the graph</param>
        /// <param name="sources">the source vertices</param>
        public DirectedDFS(Digraph g, IEnumerable<int> sources)
        {
            _marked = new bool[g.V()];
            foreach (int v in sources)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                }
            }
        }

        private void DFS(Digraph g, int v)
        {
            _count++;
            _marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                if (!_marked[w])
                {
                    DFS(g, w);
                }
            }
        }

        /// <summary>
        /// Is there a directed path from the source vertex (or any
        /// of the source vertices) and vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>true if there is a directed path, false otherwise</returns>
        public bool Marked(int v)
        {
            return _marked[v];
        }

        /// <summary>
        /// Returns the number of vertices reachable from the source vertex
        /// (or source vertices).
        /// </summary>
        /// <returns>the number of vertices reachable from the source vertex (or source vertices)</returns>
        public int Count()
        {
            return _count;
        }

        /// <summary>
        /// Unit tests the DirectedDFS data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {

            // read in digraph from command-line argument
            In input = new In(args[0]);
            Digraph g = new Digraph(input);

            // read in sources from command-line arguments
            Bag<int> sources = new Bag<int>();
            for (int i = 1; i < args.Length; i++)
            {
                int s = int.Parse(args[i]);
                sources.Add(s);
            }

            // multiple-source reachability
            DirectedDFS dfs = new DirectedDFS(g, sources);

            // print out vertices reachable from sources
            for (int v = 0; v < g.V(); v++)
            {
                if (dfs.Marked(v))
                {
                    StdOut.Print(v + " ");
                }
            }
            StdOut.PrintLn();
        }
    }
}
