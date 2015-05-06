using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DirectedCycle
    {
        /// <summary>
        /// marked[v] = has vertex v been marked?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = previous vertex on path to v
        /// </summary>
        private readonly int[] _edgeTo;

        /// <summary>
        /// onStack[v] = is vertex on the stack?
        /// </summary>
        private readonly bool[] _onStack;

        /// <summary>
        /// directed cycle (or null if no such cycle)
        /// </summary>
        private Stack<int> _cycle;

        /// <summary>
        /// Determines whether the digraph G has a directed cycle and, if so,
        /// finds such a cycle.
        /// </summary>
        /// <param name="g">the digraph</param>
        public DirectedCycle(Digraph g)
        {
            _marked = new bool[g.V()];
            _onStack = new bool[g.V()];
            _edgeTo = new int[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                }
            }
        }

        /// <summary>
        /// Check that algorithm computes either the topological order or finds a directed cycle
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(Digraph g, int v)
        {
            _onStack[v] = true;
            _marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                // short circuit if directed cycle found
                if (_cycle != null)
                {
                    return;
                }

                //found new vertex, so recur
                if (!_marked[w])
                {
                    _edgeTo[w] = v;
                    DFS(g, w);
                }

                // trace back directed cycle
                else if (_onStack[w])
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

            _onStack[v] = false;
        }

        /// <summary>
        /// Does the digraph have a directed cycle?
        /// </summary>
        /// <returns>true if the digraph has a directed cycle, false otherwise</returns>
        public bool HasCycle()
        {
            return _cycle != null;
        }

        /// <summary>
        /// Returns a directed cycle if the digraph has a directed cycle, and null otherwise.
        /// </summary>
        /// <returns>a directed cycle (as an iterable) if the digraph has a directed cycle, and null otherwise</returns>
        public IEnumerable<int> Cycle()
        {
            return _cycle;
        }

        /// <summary>
        /// Certify that digraph is either acyclic or has a directed cycle
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            if (HasCycle())
            {
                // verify cycle
                int first = -1, last = -1;
                foreach (int v in Cycle())
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
        /// Unit tests the DirectedCycle data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);

            DirectedCycle finder = new DirectedCycle(g);
            if (finder.HasCycle())
            {
                StdOut.Print("Cycle: ");
                foreach (int v in finder.Cycle())
                {
                    StdOut.Print(v + " ");
                }
                StdOut.PrintLn();
            }
            else
            {
                StdOut.PrintLn("No cycle");
            }
        }
    }
}