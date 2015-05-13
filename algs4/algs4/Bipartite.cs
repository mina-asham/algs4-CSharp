using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Bipartite
    {
        /// <summary>
        /// Is the graph bipartite?
        /// </summary>
        private bool _isBipartite;

        /// <summary>
        /// color[v] gives vertices on one side of bipartition
        /// </summary>
        private readonly bool[] _color;

        /// <summary>
        /// marked[v] = true if v has been visited in DFS
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = last edge on path to v
        /// </summary>
        private readonly int[] _edgeTo;

        /// <summary>
        /// Odd-length cycle
        /// </summary>
        private Stack<int> _cycle;

        /// <summary>
        /// Determines whether an undirected graph is bipartite and finds either a
        /// bipartition or an odd-length cycle.</summary>
        /// <param name="g">the graph</param>
        public Bipartite(Graph g)
        {
            _isBipartite = true;
            _color = new bool[g.V()];
            _marked = new bool[g.V()];
            _edgeTo = new int[g.V()];

            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                }
            }
            Debug.Assert(Check(g));
        }

        private void DFS(Graph g, int v)
        {
            _marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                // short circuit if odd-length cycle found
                if (_cycle != null)
                {
                    return;
                }

                // found uncolored vertex, so recur
                if (!_marked[w])
                {
                    _edgeTo[w] = v;
                    _color[w] = !_color[v];
                    DFS(g, w);
                }

                    // if v-w create an odd-length cycle, find it
                else if (_color[w] == _color[v])
                {
                    _isBipartite = false;
                    _cycle = new Stack<int>();
                    _cycle.Push(w); // don't need this unless you want to include start vertex twice
                    for (int x = v; x != w; x = _edgeTo[x])
                    {
                        _cycle.Push(x);
                    }
                    _cycle.Push(w);
                }
            }
        }

        /// <summary>
        /// Is the graph bipartite?
        /// </summary>
        /// <returns>true if the graph is bipartite, false otherwise</returns>
        public bool IsBipartite()
        {
            return _isBipartite;
        }

        /// <summary>
        /// Returns the side of the bipartite that vertex v is on.
        /// param v the vertex
        /// </summary>
        /// <param name="v"></param>
        /// <returns>the side of the bipartition that vertex v is on; two vertices are in the same side of the bipartition if and only if they have the same color</returns>
        public bool Color(int v)
        {
            if (!_isBipartite)
            {
                throw new InvalidOperationException("Graph is not bipartite");
            }
            return _color[v];
        }

        /// <summary>
        /// Returns an odd-length cycle if the graph is not bipartite, and
        /// null otherwise.
        /// </summary>
        /// <returns>an odd-length cycle (as an iterable) if the graph is not bipartite (and hence has an odd-length cycle), and null otherwise</returns>
        public IEnumerable<int> OddCycle()
        {
            return _cycle;
        }

        private bool Check(Graph g)
        {
            // graph is bipartite
            if (_isBipartite)
            {
                for (int v = 0; v < g.V(); v++)
                {
                    foreach (int w in g.Adj(v))
                    {
                        if (_color[v] == _color[w])
                        {
                            Console.Error.Write("edge {0}-{1} with {2} and {3} in same side of bipartition\n", v, w, v, w);
                            return false;
                        }
                    }
                }
            }

            // graph has an odd-length cycle
            else
            {
                // verify cycle
                int first = -1, last = -1;
                foreach (int v in OddCycle())
                {
                    if (first == -1)
                    {
                        first = v;
                    }
                    last = v;
                }
                if (first != last)
                {
                    Console.Error.Write("cycle begins with {0} and ends with {1}\n", first, last);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Unit tests the Bipartite data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // create random bipartite graph with V vertices and E edges; then add F random edges
            int v = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            int f = int.Parse(args[2]);

            Graph g = new Graph(v);
            int[] vertices = new int[v];
            for (int i = 0; i < v; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            for (int i = 0; i < e; i++)
            {
                int vFrom = StdRandom.Uniform(v / 2);
                int wTo = StdRandom.Uniform(v / 2);
                g.AddEdge(vertices[vFrom], vertices[v / 2 + wTo]);
            }

            // add F extra edges
            Random random = new Random();
            for (int i = 0; i < f; i++)
            {
                int vFrom = (int)(random.NextDouble() * v);
                int wFrom = (int)(random.NextDouble() * v);
                g.AddEdge(vFrom, wFrom);
            }

            StdOut.PrintLn(g);

            Bipartite b = new Bipartite(g);
            if (b.IsBipartite())
            {
                StdOut.PrintLn("Graph is bipartite");
                for (int vertex = 0; vertex < g.V(); vertex++)
                {
                    StdOut.PrintLn(vertex + ": " + b.Color(vertex));
                }
            }
            else
            {
                StdOut.Print("Graph has an odd-length cycle: ");
                foreach (int x in b.OddCycle())
                {
                    StdOut.Print(x + " ");
                }
                StdOut.PrintLn();
            }
        }
    }
}