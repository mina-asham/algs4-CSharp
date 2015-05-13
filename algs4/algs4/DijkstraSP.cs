using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DijkstraSP
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
        /// Priority queue of vertices
        /// </summary>
        private readonly IndexMinPQ<double> _pq;

        /// <summary>
        /// Computes a shortest paths tree from s to every other vertex in
        /// the edge-weighted digraph G.
        /// </summary>
        /// <param name="g">the edge-weighted digraph</param>
        /// <param name="s">the source vertex</param>
        public DijkstraSP(EdgeWeightedDigraph g, int s)
        {
            foreach (DirectedEdge e in g.Edges())
            {
                if (e.Weight() < 0)
                {
                    throw new ArgumentException("edge " + e + " has negative weight");
                }
            }

            _distTo = new double[g.V()];
            _edgeTo = new DirectedEdge[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = double.PositiveInfinity;
            }
            _distTo[s] = 0.0;

            // relax vertices in order of distance from s
            _pq = new IndexMinPQ<double>(g.V());
            _pq.Insert(s, _distTo[s]);
            while (!_pq.IsEmpty())
            {
                int v = _pq.DelMin();
                foreach (DirectedEdge e in g.Adj(v))
                {
                    Relax(e);
                }
            }

            // check optimality conditions
            Debug.Assert(Check(g, s));
        }

        /// <summary>
        /// Relax edge e and update pq if changed
        /// </summary>
        /// <param name="e"></param>
        private void Relax(DirectedEdge e)
        {
            int v = e.From(), w = e.To();
            if (_distTo[w] > _distTo[v] + e.Weight())
            {
                _distTo[w] = _distTo[v] + e.Weight();
                _edgeTo[w] = e;
                if (_pq.Contains(w))
                {
                    _pq.DecreaseKey(w, _distTo[w]);
                }
                else
                {
                    _pq.Insert(w, _distTo[w]);
                }
            }
        }

        /// <summary>
        /// Returns the length of a shortest path from the source vertex s to vertex v.
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>the length of a shortest path from the source vertex s to vertex v; double.PositiveInfinity if no such path</returns>
        public double DistTo(int v)
        {
            return _distTo[v];
        }

        /// <summary>
        /// Is there a path from the source vertex s to vertex v?
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>true if there is a path from the source vertex s to vertex v, and false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _distTo[v] < double.PositiveInfinity;
        }

        /// <summary>
        /// Returns a shortest path from the source vertex s to vertex v.
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>a shortest path from the source vertex s to vertex v as an iterable of edges, and null if no such path</returns>
        public IEnumerable<DirectedEdge> PathTo(int v)
        {
            if (!HasPathTo(v))
            {
                return null;
            }
            Stack<DirectedEdge> path = new Stack<DirectedEdge>();
            for (DirectedEdge e = _edgeTo[v]; e != null; e = _edgeTo[e.From()])
            {
                path.Push(e);
            }
            return path;
        }

        /// <summary>
        /// Check optimality conditions:
        /// (i) for all edges e:            distTo[e.To()] &lt;= distTo[e.From()] + e.Weight()
        /// (ii) for all edge e on the SPT: distTo[e.To()] == distTo[e.From()] + e.Weight()
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool Check(EdgeWeightedDigraph g, int s)
        {
            // check that edge weights are nonnegative
            foreach (DirectedEdge e in g.Edges())
            {
                if (e.Weight() < 0)
                {
                    Console.Error.WriteLine("negative edge weight detected");
                    return false;
                }
            }

            // check that distTo[v] and edgeTo[v] are consistent
            if (Math.Abs(_distTo[s]) > double.Epsilon || _edgeTo[s] != null)
            {
                Console.Error.WriteLine("distTo[s] and edgeTo[s] inconsistent");
                return false;
            }
            for (int v = 0; v < g.V(); v++)
            {
                if (v == s)
                {
                    continue;
                }
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
                if (_edgeTo[w] == null)
                {
                    continue;
                }
                DirectedEdge e = _edgeTo[w];
                int v = e.From();
                if (w != e.To())
                {
                    return false;
                }
                if (Math.Abs(_distTo[v] + e.Weight() - _distTo[w]) > double.Epsilon)
                {
                    Console.Error.WriteLine("edge " + e + " on shortest path not tight");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Unit tests the DijkstraSP data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(input);
            int s = int.Parse(args[1]);

            // compute shortest paths
            DijkstraSP sp = new DijkstraSP(g, s);

            // print shortest path
            for (int t = 0; t < g.V(); t++)
            {
                if (sp.HasPathTo(t))
                {
                    StdOut.PrintF("{0} to {1} ({2:0.00})  ", s, t, sp.DistTo(t));
                    if (sp.HasPathTo(t))
                    {
                        foreach (DirectedEdge e in sp.PathTo(t))
                        {
                            StdOut.Print(e + "   ");
                        }
                    }
                    StdOut.PrintLn();
                }
                else
                {
                    StdOut.PrintF("{0} to {1}         no path\n", s, t);
                }
            }
        }
    }
}