using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DepthFirstPaths
    {
        /// <summary>
        /// marked[v] = is there an s-v path?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = last edge on s-v path
        /// </summary>
        private readonly int[] _edgeTo;

        /// <summary>
        /// Source vertex
        /// </summary>
        private readonly int _s;

        /// <summary>
        /// Computes a path between s and every other vertex in graph G.
        /// </summary>
        /// <param name="g">the graph</param>
        /// <param name="s">the source vertex</param>
        public DepthFirstPaths(Graph g, int s)
        {
            _s = s;
            _edgeTo = new int[g.V()];
            _marked = new bool[g.V()];
            DFS(g, s);
        }

        /// <summary>
        /// Depth first search from v
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(Graph g, int v)
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
        /// Is there a path between the source vertex s and vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>true if there is a path, false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _marked[v];
        }

        /// <summary>
        /// Returns a path between the source vertex s and vertex v, or
        /// null if no such path.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the sequence of vertices on a path between the source vertex s and vertex v, as an IEnumerable</returns>
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
        /// Unit tests the DepthFirstPaths data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Graph g = new Graph(input);
            int s = int.Parse(args[1]);
            DepthFirstPaths dfs = new DepthFirstPaths(g, s);

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