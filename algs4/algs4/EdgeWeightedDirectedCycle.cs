using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class EdgeWeightedDirectedCycle
    {
        /// <summary>
        /// marked[v] = has vertex v been marked?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// edgeTo[v] = previous edge on path to v
        /// </summary>
        private readonly DirectedEdge[] _edgeTo;

        /// <summary>
        /// onStack[v] = is vertex on the stack?
        /// </summary>
        private readonly bool[] _onStack;

        /// <summary>
        /// Directed cycle (or null if no such cycle)
        /// </summary>
        private Stack<DirectedEdge> _cycle;

        /// <summary>
        /// Determines whether the edge-weighted digraph G has a directed cycle and,
        /// if so, finds such a cycle.
        /// </summary>
        /// <param name="g">the edge-weighted digraph</param>
        public EdgeWeightedDirectedCycle(EdgeWeightedDigraph g)
        {
            _marked = new bool[g.V()];
            _onStack = new bool[g.V()];
            _edgeTo = new DirectedEdge[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                }
            }

            // check that digraph has a cycle
            Debug.Assert(Check());
        }

        /// <summary>
        /// Check that algorithm computes either the topological order or finds a directed cycle
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(EdgeWeightedDigraph g, int v)
        {
            _onStack[v] = true;
            _marked[v] = true;
            foreach (DirectedEdge e in g.Adj(v))
            {
                int w = e.To();

                // short circuit if directed cycle found
                if (_cycle != null)
                {
                    return;
                }

                //found new vertex, so recur
                if (!_marked[w])
                {
                    _edgeTo[w] = e;
                    DFS(g, w);
                }

                // trace back directed cycle
                else if (_onStack[w])
                {
                    _cycle = new Stack<DirectedEdge>();
                    DirectedEdge edge = e;
                    while (edge.From() != w)
                    {
                        _cycle.Push(edge);
                        edge = _edgeTo[edge.From()];
                    }
                    _cycle.Push(edge);
                }
            }

            _onStack[v] = false;
        }

        /// <summary>
        /// Does the edge-weighted digraph have a directed cycle?
        /// </summary>
        /// <returns>true if the edge-weighted digraph has a directed cycle, false otherwise</returns>
        public bool HasCycle()
        {
            return _cycle != null;
        }

        /// <summary>
        /// Returns a directed cycle if the edge-weighted digraph has a directed cycle,
        /// and null otherwise.
        /// </summary>
        /// <returns>a directed cycle (as an iterable) if the edge-weighted digraph has a directed cycle, and null otherwise</returns>
        public IEnumerable<DirectedEdge> Cycle()
        {
            return _cycle;
        }

        /// <summary>
        /// Certify that digraph is either acyclic or has a directed cycle
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            // edge-weighted digraph is cyclic
            if (HasCycle())
            {
                // verify cycle
                DirectedEdge first = null, last = null;
                foreach (DirectedEdge e in Cycle())
                {
                    if (first == null)
                    {
                        first = e;
                    }
                    if (last != null)
                    {
                        if (last.To() != e.From())
                        {
                            Console.Error.Write("cycle edges {0} and {1} not incident\n", last, e);
                            return false;
                        }
                    }
                    last = e;
                }

                if (last != null && first != null && last.To() != first.From())
                {
                    Console.Error.Write("cycle edges {0} and {1} not incident\n", last, first);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Unit tests the EdgeWeightedDirectedCycle data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Random random = new Random();

            // create random DAG with V vertices and E edges; then add F random edges
            int vCount = int.Parse(args[0]);
            int eCount = int.Parse(args[1]);
            int fCount = int.Parse(args[2]);
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(vCount);
            int[] vertices = new int[vCount];
            for (int i = 0; i < vCount; i++)
            {
                vertices[i] = i;
            }
            StdRandom.Shuffle(vertices);
            for (int i = 0; i < eCount; i++)
            {
                int v, w;
                do
                {
                    v = StdRandom.Uniform(vCount);
                    w = StdRandom.Uniform(vCount);
                } while (v >= w);
                double weight = random.NextDouble();
                g.AddEdge(new DirectedEdge(v, w, weight));
            }

            // add F extra edges
            for (int i = 0; i < fCount; i++)
            {
                int v = (int)(random.NextDouble() * vCount);
                int w = (int)(random.NextDouble() * vCount);
                double weight = random.NextDouble();
                g.AddEdge(new DirectedEdge(v, w, weight));
            }

            StdOut.PrintLn(g);

            // find a directed cycle
            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(g);
            if (finder.HasCycle())
            {
                StdOut.Print("Cyclein ");
                foreach (DirectedEdge e in finder.Cycle())
                {
                    StdOut.Print(e + " ");
                }
                StdOut.PrintLn();
            }

            // or give topologial sort
            else
            {
                StdOut.PrintLn("No directed cycle");
            }
        }
    }
}