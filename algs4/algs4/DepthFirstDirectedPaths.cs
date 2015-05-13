using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DepthFirstDirectedPaths
    {
        /// <summary>
        /// marked[v] = true if v is reachable from s
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = last edge on path from s to v
        /// </summary>
        private readonly int[] _edgeTo;

        /// <summary>
        /// Source vertex
        /// </summary>
        private readonly int _s;

        /// <summary>
        /// Computes a directed path from s to every other vertex in digraph G.
        /// </summary>
        /// <param name="g">the digraph</param>
        /// <param name="s">the source vertex</param>
        public DepthFirstDirectedPaths(Digraph g, int s)
        {
            _marked = new bool[g.V()];
            _edgeTo = new int[g.V()];
            _s = s;
            DFS(g, s);
        }

        private void DFS(Digraph g, int v)
        {
            _marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                if (!_marked[w])
                {
                    _edgeTo[w] = v;
                    DFS(g, w);
                }
            }
        }

        /// <summary>
        /// Is there a directed path from the source vertex s to vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>true if there is a directed path from the source vertex s to vertex v, false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _marked[v];
        }

        /// <summary>
        /// Returns a directed path from the source vertex s to vertex v, or
        /// null if no such path.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the sequence of vertices on a directed path from the source vertex s to vertex v, as an IEnumerable</returns>
        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v))
            {
                return null;
            }
            Stack<int> path = new Stack<int>();
            for (int x = v; x != _s; x = _edgeTo[x])
            {
                path.Push(x);
            }
            path.Push(_s);
            return path;
        }

        /// <summary>
        /// Unit tests the DepthFirstDirectedPaths data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);

            int s = int.Parse(args[1]);
            DepthFirstDirectedPaths dfs = new DepthFirstDirectedPaths(g, s);

            for (int v = 0; v < g.V(); v++)
            {
                if (dfs.HasPathTo(v))
                {
                    StdOut.PrintF("{0} to {1}:  ", s, v);
                    foreach (int x in dfs.PathTo(v))
                    {
                        if (x == s)
                        {
                            StdOut.Print(x);
                        }
                        else
                        {
                            StdOut.Print("-" + x);
                        }
                    }
                    StdOut.PrintLn();
                }

                else
                {
                    StdOut.PrintF("{0} to {1}:  not connected\n", s, v);
                }
            }
        }
    }
}