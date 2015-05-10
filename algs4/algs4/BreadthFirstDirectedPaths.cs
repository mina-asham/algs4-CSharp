using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BreadthFirstDirectedPaths
    {
        private const int Infinity = int.MaxValue;

        /// <summary>
        /// marked[v] = is there an s->v path?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = last edge on shortest s->v path
        /// </summary>
        private readonly int[] _edgeTo;

        /// <summary>
        /// distTo[v] = length of shortest s->v path
        /// </summary>
        private readonly int[] _distTo;

        /// <summary>
        /// Computes the shortest path from s and every other vertex in graph G.
        /// </summary>
        /// <param name="g">the digraph</param>
        /// <param name="s">the source vertex</param>
        public BreadthFirstDirectedPaths(Digraph g, int s)
        {
            _marked = new bool[g.V()];
            _distTo = new int[g.V()];
            _edgeTo = new int[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = Infinity;
            }
            BFS(g, s);
        }

        /// <summary>
        /// Computes the shortest path from any one of the source vertices in sources
        /// to every other vertex in graph G.
        /// </summary>
        /// <param name="g">the digraph</param>
        /// <param name="sources">the source vertices</param>
        public BreadthFirstDirectedPaths(Digraph g, IEnumerable<int> sources)
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
        /// BFS from single source
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        private void BFS(Digraph g, int s)
        {
            Queue<int> q = new Queue<int>();
            _marked[s] = true;
            _distTo[s] = 0;
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
        /// BFS from multiple sources
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sources"></param>
        private void BFS(Digraph g, IEnumerable<int> sources)
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
        /// Is there a directed path from the source s (or sources) to vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>true if there is a directed path, false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _marked[v];
        }

        /// <summary>
        /// Returns the number of edges in a shortest path from the source s
        /// (or sources) to vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the number of edges in a shortest path</returns>
        public int DistTo(int v)
        {
            return _distTo[v];
        }

        /// <summary>
        /// Returns a shortest path from s (or sources) to v, or
        /// null if no such path.
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
        /// Unit tests the BreadthFirstDirectedPaths data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);

            int s = int.Parse(args[1]);
            BreadthFirstDirectedPaths bfs = new BreadthFirstDirectedPaths(g, s);

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
                            StdOut.Print("->" + x);
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