using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BreadthFirstPaths
    {
        private const int Infinity = int.MaxValue;

        /// <summary>
        /// marked[v] = is there an s-v path
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = previous edge on shortest s-v path
        /// </summary>
        private readonly int[] _edgeTo;

        /// <summary>
        /// distTo[v] = number of edges shortest s-v path
        /// </summary>
        private readonly int[] _distTo;

        /// <summary>
        /// Computes the shortest path between the source vertex s
        /// and every other vertex in the graph G.
        /// </summary>
        /// <param name="g">the graph</param>
        /// <param name="s">the source vertex</param>
        public BreadthFirstPaths(Graph g, int s)
        {
            _marked = new bool[g.V()];
            _distTo = new int[g.V()];
            _edgeTo = new int[g.V()];
            BFS(g, s);

            Debug.Assert(Check(g, s));
        }

        /// <summary>
        /// Computes the shortest path between any one of the source vertices in sources
        /// and every other vertex in graph G.
        /// </summary>
        /// <param name="g">the graph</param>
        /// <param name="sources">the source vertices</param>
        public BreadthFirstPaths(Graph g, IEnumerable<int> sources)
        {
            _marked = new bool[g.V()];
            _distTo = new int[g.V()];
            _edgeTo = new int[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = Infinity;
            }
            BFS(g, sources);
        }

        /// <summary>
        /// Breadth-first search from a single source
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        private void BFS(Graph g, int s)
        {
            Queue<int> q = new Queue<int>();
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = Infinity;
            }
            _distTo[s] = 0;
            _marked[s] = true;
            q.Enqueue(s);

            while (!q.IsEmpty())
            {
                int v = q.Dequeue();
                foreach (int w in g.Adj(v))
                {
                    if (!_marked[w])
                    {
                        _edgeTo[w] = v;
                        _distTo[w] = _distTo[v] + 1;
                        _marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
        }

        /// <summary>
        /// Breadth-first search from multiple sources
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sources"></param>
        private void BFS(Graph g, IEnumerable<int> sources)
        {
            Queue<int> q = new Queue<int>();
            foreach (int s in sources)
            {
                _marked[s] = true;
                _distTo[s] = 0;
                q.Enqueue(s);
            }
            while (!q.IsEmpty())
            {
                int v = q.Dequeue();
                foreach (int w in g.Adj(v))
                {
                    if (!_marked[w])
                    {
                        _edgeTo[w] = v;
                        _distTo[w] = _distTo[v] + 1;
                        _marked[w] = true;
                        q.Enqueue(w);
                    }
                }
            }
        }

        /// <summary>
        /// Is there a path between the source vertex s (or sources) and vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>true if there is a path, and false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _marked[v];
        }

        /// <summary>
        /// Returns the number of edges in a shortest path between the source vertex s
        /// (or sources) and vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the number of edges in a shortest path</returns>
        public int DistTo(int v)
        {
            return _distTo[v];
        }

        /// <summary>
        /// Returns a shortest path between the source vertex s (or sources)
        /// and v, or null if no such path.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the sequence of vertices on a shortest path, as an IEnumerable</returns>
        public IEnumerable<int> PathTo(int v)
        {
            if (!HasPathTo(v))
            {
                return null;
            }
            Stack<int> path = new Stack<int>();
            int x;
            for (x = v; _distTo[x] != 0; x = _edgeTo[x])
            {
                path.Push(x);
            }
            path.Push(x);
            return path;
        }

        /// <summary>
        /// Check optimality conditions for single source
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool Check(Graph g, int s)
        {
            // check that the distance of s = 0
            if (_distTo[s] != 0)
            {
                StdOut.PrintLn("distance of source " + s + " to itself = " + _distTo[s]);
                return false;
            }

            // check that for each edge v-w dist[w] <= dist[v] + 1
            // provided v is reachable from s
            for (int v = 0; v < g.V(); v++)
            {
                foreach (int w in g.Adj(v))
                {
                    if (HasPathTo(v) != HasPathTo(w))
                    {
                        StdOut.PrintLn("edge " + v + "-" + w);
                        StdOut.PrintLn("hasPathTo(" + v + ") = " + HasPathTo(v));
                        StdOut.PrintLn("hasPathTo(" + w + ") = " + HasPathTo(w));
                        return false;
                    }
                    if (HasPathTo(v) && (_distTo[w] > _distTo[v] + 1))
                    {
                        StdOut.PrintLn("edge " + v + "-" + w);
                        StdOut.PrintLn("distTo[" + v + "] = " + _distTo[v]);
                        StdOut.PrintLn("distTo[" + w + "] = " + _distTo[w]);
                        return false;
                    }
                }
            }

            // check that v = edgeTo[w] satisfies distTo[w] + distTo[v] + 1
            // provided v is reachable from s
            for (int w = 0; w < g.V(); w++)
            {
                if (!HasPathTo(w) || w == s)
                {
                    continue;
                }
                int v = _edgeTo[w];
                if (_distTo[w] != _distTo[v] + 1)
                {
                    StdOut.PrintLn("shortest path edge " + v + "-" + w);
                    StdOut.PrintLn("distTo[" + v + "] = " + _distTo[v]);
                    StdOut.PrintLn("distTo[" + w + "] = " + _distTo[w]);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Unit tests the BreadthFirstPaths data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Graph g = new Graph(input);

            int s = int.Parse(args[1]);
            BreadthFirstPaths bfs = new BreadthFirstPaths(g, s);

            for (int v = 0; v < g.V(); v++)
            {
                if (bfs.HasPathTo(v))
                {
                    StdOut.PrintF("{0} to {1} ({2}):  ", s, v, bfs.DistTo(v));
                    foreach (int x in bfs.PathTo(v))
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
                    StdOut.PrintF("{0} to {1} (-):  not connected\n", s, v);
                }
            }
        }
    }
}