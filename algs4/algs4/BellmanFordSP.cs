using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BellmanFordSP
    {
        /// <summary>
        /// distTo[v] = distance  of shortest s->v path
        /// </summary>
        private readonly double[] _distTo;

        /// <summary>
        /// edgeTo[v] = last edge on shortest s->v path
        /// </summary>
        private readonly DirectedEdge[] _edgeTo;

        /// <summary>
        /// onQueue[v] = is v currently on the queue?
        /// </summary>
        private readonly bool[] _onQueue;

        /// <summary>
        /// Queue of vertices.To relax
        /// </summary>
        private readonly Queue<int> _queue;

        /// <summary>
        /// Number of calls.To relax()
        /// </summary>
        private int _cost;

        /// <summary>
        /// Negative cycle (or null if no such cycle)
        /// </summary>
        private IEnumerable<DirectedEdge> _cycle;

        /// <summary>
        /// Computes a shortest paths tree.From s.To every other vertex in
        /// the edge.Weighted digraph G.
        /// </summary>
        /// <param name="g">the acyclic digraph</param>
        /// <param name="s">the source vertex</param>
        public BellmanFordSP(EdgeWeightedDigraph g, int s)
        {
            _distTo = new double[g.V()];
            _edgeTo = new DirectedEdge[g.V()];
            _onQueue = new bool[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = double.PositiveInfinity;
            }
            _distTo[s] = 0.0;

            // Bellman-Ford algorithm
            _queue = new Queue<int>();
            _queue.Enqueue(s);
            _onQueue[s] = true;
            while (_queue.Size() != 0 && !HasNegativeCycle())
            {
                int v = _queue.Dequeue();
                _onQueue[v] = false;
                Relax(g, v);
            }
        }

        /// <summary>
        /// Relax vertex v and put other endpoints on queue if changed
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void Relax(EdgeWeightedDigraph g, int v)
        {
            foreach (DirectedEdge e in g.Adj(v))
            {
                int w = e.To();
                if (_distTo[w] > _distTo[v] + e.Weight())
                {
                    _distTo[w] = _distTo[v] + e.Weight();
                    _edgeTo[w] = e;
                    if (!_onQueue[w])
                    {
                        _queue.Enqueue(w);
                        _onQueue[w] = true;
                    }
                }
                if (_cost++ % g.V() == 0)
                    FindNegativeCycle();
            }
        }

        /// <summary>
        /// Is there a negative cycle reachable.From the source vertex s?
        /// </summary>
        /// <returns>true if there is a negative cycle reachable.From the source vertex s, and false otherwise</returns>
        public bool HasNegativeCycle()
        {
            return _cycle != null;
        }

        /// <summary>
        /// Returns a negative cycle reachable.From the source vertex s, or null
        /// if there is no such cycle.
        /// </summary>
        /// <returns>a negative cycle reachable.From the soruce vertex s as an iterable of edges, and null if there is no such cycle</returns>
        public IEnumerable<DirectedEdge> NegativeCycle()
        {
            return _cycle;
        }

        /// <summary>
        /// By finding a cycle in predecessor graph
        /// </summary>
        private void FindNegativeCycle()
        {
            int v = _edgeTo.Length;
            EdgeWeightedDigraph spt = new EdgeWeightedDigraph(v);
            for (int vertex = 0; vertex < v; vertex++)
            {
                if (_edgeTo[vertex] != null)
                {
                    spt.AddEdge(_edgeTo[vertex]);
                }
            }

            EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(spt);
            _cycle = finder.Cycle();
        }

        /// <summary>
        /// Returns the length of a shortest path.From the source vertex s.To vertex v.
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>the length of a shortest path.From the source vertex s.To vertex v; double.PositiveInfinity if no such path</returns>
        public double DistTo(int v)
        {
            if (HasNegativeCycle())
            {
                throw new InvalidOperationException("Negative cost cycle exists");
            }
            return _distTo[v];
        }

        /// <summary>
        /// Is there a path.From the source s.To vertex v?
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>true if there is a path.From the source vertex s.To vertex v, and false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _distTo[v] < double.PositiveInfinity;
        }

        /// <summary>
        /// Returns a shortest path.From the source s.To vertex v.
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>a shortest path.From the source s.To vertex v as an iterable of edges, and null if no such path</returns>
        public IEnumerable<DirectedEdge> PathTo(int v)
        {
            if (HasNegativeCycle())
            {
                throw new InvalidOperationException("Negative cost cycle exists");
            }
            if (!HasPathTo(v)) return null;
            Stack<DirectedEdge> path = new Stack<DirectedEdge>();
            for (DirectedEdge e = _edgeTo[v]; e != null; e = _edgeTo[e.From()])
            {
                path.Push(e);
            }
            return path;
        }

        /// <summary>
        /// Check optimality conditions: either 
        /// (i) there exists a negative cycle reacheable from s
        ///     or 
        /// (ii)  for all edges e = v-&gt;w:            distTo[w] &lt;= distTo[v] + e.Weight()
        /// (ii') for all edges e = v-&gt;w on the SPT: distTo[w] == distTo[v] + e.Weight()
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Check(EdgeWeightedDigraph g, int s)
        {
            // has a negative cycle
            if (HasNegativeCycle())
            {
                double weight = 0.0;
                foreach (DirectedEdge e in NegativeCycle())
                {
                    weight += e.Weight();
                }
                if (weight >= 0.0)
                {
                    Console.Error.WriteLine("error: weight of negative cycle = " + weight);
                    return false;
                }
            }

            // no negative cycle reachable.From source
            else
            {
                // check that distTo[v] and edgeTo[v] are consistent
                if (Math.Abs(_distTo[s]) > double.Epsilon || _edgeTo[s] != null)
                {
                    Console.Error.WriteLine("distanceTo[s] and edgeTo[s] inconsistent");
                    return false;
                }
                for (int v = 0; v < g.V(); v++)
                {
                    if (v == s) continue;
                    if (_edgeTo[v] == null && !double.IsPositiveInfinity(_distTo[v]))
                    {
                        Console.Error.WriteLine("distTo[] and edgeTo[] inconsistent");
                        return false;
                    }
                }

                // check that all edges e = v->w satisfy distTo[w] <= distTo[v] + e.Weight()
                for (int v = 0; v < g.V(); v++)
                {
                    foreach (DirectedEdge e in g.Adj(v))
                    {
                        int w = e.To();
                        if (_distTo[v] + e.Weight() < _distTo[w])
                        {
                            Console.Error.WriteLine("edge " + e + " not relaxed");
                            return false;
                        }
                    }
                }

                // check that all edges e = v->w on SPT satisfy distTo[w] == distTo[v] + e.Weight()
                for (int w = 0; w < g.V(); w++)
                {
                    if (_edgeTo[w] == null) continue;
                    DirectedEdge e = _edgeTo[w];
                    int v = e.From();
                    if (w != e.To()) return false;
                    if (Math.Abs(_distTo[v] + e.Weight() - _distTo[w]) > double.Epsilon)
                    {
                        Console.Error.WriteLine("edge " + e + " on shortest path not tight");
                        return false;
                    }
                }
            }

            StdOut.PrintLn("Satisfies optimality conditions");
            StdOut.PrintLn();
            return true;
        }

        /// <summary>
        /// Unit tests the BellmanFordSP data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            int s = int.Parse(args[1]);
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(input);

            BellmanFordSP sp = new BellmanFordSP(g, s);

            // print negative cycle
            if (sp.HasNegativeCycle())
            {
                foreach (DirectedEdge e in sp.NegativeCycle())
                    StdOut.PrintLn(e);
            }

            // print shortest paths
            else
            {
                for (int v = 0; v < g.V(); v++)
                {
                    if (sp.HasPathTo(v))
                    {
                        StdOut.PrintF("{0}.To {1} ({2:00000.00})  ", s, v, sp.DistTo(v));
                        foreach (DirectedEdge e in sp.PathTo(v))
                        {
                            StdOut.Print(e + "   ");
                        }
                        StdOut.PrintLn();
                    }
                    else
                    {
                        StdOut.PrintF("{0}.To {1}           no path\n", s, v);
                    }
                }
            }
        }
    }
}
