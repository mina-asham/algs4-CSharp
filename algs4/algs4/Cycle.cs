using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Cycle
    {
        private bool[] _marked;
        private readonly int[] _edgeTo;
        private Stack<int> _cycle;

        /// <summary>
        /// Determines whether the undirected graph G has a cycle and, if so,
        /// finds such a cycle.
        /// </summary>
        /// <param name="g">the graph</param>
        public Cycle(Graph g)
        {
            if (HasSelfLoop(g))
            {
                return;
            }
            if (HasParallelEdges(g))
            {
                return;
            }
            _marked = new bool[g.V()];
            _edgeTo = new int[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, -1, v);
                }
            }
        }

        /// <summary>
        /// Does this graph have a self loop?
        /// side effect: initialize cycle to be self loop
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool HasSelfLoop(Graph g)
        {
            for (int v = 0; v < g.V(); v++)
            {
                foreach (int w in g.Adj(v))
                {
                    if (v == w)
                    {
                        _cycle = new Stack<int>();
                        _cycle.Push(v);
                        _cycle.Push(v);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Does this graph have two parallel edges?
        /// side effect: initialize cycle to be two parallel edges
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool HasParallelEdges(Graph g)
        {
            _marked = new bool[g.V()];

            for (int v = 0; v < g.V(); v++)
            {
                // check for parallel edges incident to v
                foreach (int w in g.Adj(v))
                {
                    if (_marked[w])
                    {
                        _cycle = new Stack<int>();
                        _cycle.Push(v);
                        _cycle.Push(w);
                        _cycle.Push(v);
                        return true;
                    }
                    _marked[w] = true;
                }

                // reset so marked[v] = false for all v
                foreach (int w in g.Adj(v))
                {
                    _marked[w] = false;
                }
            }
            return false;
        }

        /// <summary>
        /// Does the graph have a cycle?
        /// </summary>
        /// <returns>true if the graph has a cycle, false otherwise</returns>
        public bool HasCycle()
        {
            return _cycle != null;
        }

        /// <summary>
        /// Returns a cycle if the graph has a cycle, and null otherwise.
        /// </summary>
        /// <returns>a cycle (as an iterable) if the graph has a cycle, and null otherwise</returns>
        public IEnumerable<int> GetCycle()
        {
            return _cycle;
        }

        private void DFS(Graph g, int u, int v)
        {
            _marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                // short circuit if cycle already found
                if (_cycle != null)
                {
                    return;
                }

                if (!_marked[w])
                {
                    _edgeTo[w] = v;
                    DFS(g, v, w);
                }

                    // check for cycle (but disregard reverse of edge leading to v)
                else if (w != u)
                {
                    _cycle = new Stack<int>();
                    for (int x = v; x != w; x = _edgeTo[x])
                    {
                        _cycle.Push(x);
                    }
                    _cycle.Push(w);
                    _cycle.Push(v);
                }
            }
        }

        /// <summary>
        /// Unit tests the Cycle data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Graph g = new Graph(input);
            Cycle finder = new Cycle(g);
            if (finder.HasCycle())
            {
                foreach (int v in finder.GetCycle())
                {
                    StdOut.Print(v + " ");
                }
                StdOut.PrintLn();
            }
            else
            {
                StdOut.PrintLn("Graph is acyclic");
            }
        }
    }
}