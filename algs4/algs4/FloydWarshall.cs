using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class FloydWarshall
    {
        /// <summary>
        /// Is there a negative cycle?
        /// </summary>
        private readonly bool _hasNegativeCycle;

        /// <summary>
        /// distTo[v][w] = length of shortest v->w path
        /// </summary>
        private readonly double[][] _distTo;

        /// <summary>
        /// edgeTo[v][w] = last edge on shortest v->w path
        /// </summary>
        private readonly DirectedEdge[][] _edgeTo;

        /// <summary>
        /// Computes a shortest paths tree From each vertex To To every other vertex in
        /// the edge-weighted digraph G. If no such shortest path exists for
        /// some pair of vertices, it computes a negative cycle.
        /// </summary>
        /// <param name="g">the edge-weighted digraph</param>
        public FloydWarshall(AdjMatrixEdgeWeightedDigraph g)
        {
            int v = g.V();
            _distTo = new double[v][];
            _edgeTo = new DirectedEdge[v][];

            for (int i = 0; i < v; i++)
            {
                _distTo[i] = new double[v];
                _edgeTo[i] = new DirectedEdge[v];
            }

            // initialize distances To infinity
            for (int vertex = 0; vertex < v; vertex++)
            {
                for (int w = 0; w < v; w++)
                {
                    _distTo[vertex][w] = double.PositiveInfinity;
                }
            }

            // initialize distances using edge-weighted digraph's
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                foreach (DirectedEdge e in g.Adj(vertex))
                {
                    _distTo[e.From()][e.To()] = e.Weight();
                    _edgeTo[e.From()][e.To()] = e;
                }
                // in case of self-loops
                if (_distTo[vertex][vertex] >= 0.0)
                {
                    _distTo[vertex][vertex] = 0.0;
                    _edgeTo[vertex][vertex] = null;
                }
            }

            // Floyd-Warshall updates
            for (int i = 0; i < v; i++)
            {
                // compute shortest paths using only 0, 1, ..., i as intermediate vertices
                for (int vertex = 0; vertex < v; vertex++)
                {
                    if (_edgeTo[vertex][i] == null)
                    {
                        continue; // optimization
                    }
                    for (int w = 0; w < v; w++)
                    {
                        if (_distTo[vertex][w] > _distTo[vertex][i] + _distTo[i][w])
                        {
                            _distTo[vertex][w] = _distTo[vertex][i] + _distTo[i][w];
                            _edgeTo[vertex][w] = _edgeTo[i][w];
                        }
                    }
                    // check for negative cycle
                    if (_distTo[vertex][vertex] < 0.0)
                    {
                        _hasNegativeCycle = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Is there a negative cycle?
        /// </summary>
        /// <returns>true if there is a negative cycle, and false otherwise</returns>
        public bool HasNegativeCycle()
        {
            return _hasNegativeCycle;
        }

        /// <summary>
        /// Returns a negative cycle, or null if there is no such cycle.
        /// </summary>
        /// <returns>a negative cycle as an iterable of edges, or null if there is no such cycle</returns>
        public IEnumerable<DirectedEdge> NegativeCycle()
        {
            for (int vertex = 0; vertex < _distTo.Length; vertex++)
            {
                // negative cycle in v's predecessor graph
                if (_distTo[vertex][vertex] < 0.0)
                {
                    int v = _edgeTo.Length;
                    EdgeWeightedDigraph spt = new EdgeWeightedDigraph(v);
                    for (int w = 0; w < v; w++)
                    {
                        if (_edgeTo[vertex][w] != null)
                        {
                            spt.AddEdge(_edgeTo[vertex][w]);
                        }
                    }
                    EdgeWeightedDirectedCycle finder = new EdgeWeightedDirectedCycle(spt);
                    Debug.Assert(finder.HasCycle());
                    return finder.Cycle();
                }
            }
            return null;
        }

        /// <summary>
        /// Is there a path From the vertex s To vertex t?
        /// </summary>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the destination vertex</param>
        /// <returns>true if there is a path From vertex s to vertex t, and false otherwise</returns>
        public bool HasPath(int s, int t)
        {
            return _distTo[s][t] < double.PositiveInfinity;
        }

        /// <summary>
        /// Returns the length of a shortest path From vertex s To vertex t.
        /// </summary>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the destination vertex</param>
        /// <returns>the length of a shortest path From vertex s To vertex t; double.PositiveInfinity if no such path</returns>
        public double Dist(int s, int t)
        {
            if (HasNegativeCycle())
            {
                throw new InvalidOperationException("Negative cost cycle exists");
            }
            return _distTo[s][t];
        }

        /// <summary>
        /// Returns a shortest path From vertex s To vertex t.
        /// </summary>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the destination vertex</param>
        /// <returns>a shortest path From vertex s To vertex t as an iterable of edges, and null if no such path</returns>
        public IEnumerable<DirectedEdge> Path(int s, int t)
        {
            if (HasNegativeCycle())
            {
                throw new InvalidOperationException("Negative cost cycle exists");
            }
            if (!HasPath(s, t))
            {
                return null;
            }
            Stack<DirectedEdge> path = new Stack<DirectedEdge>();
            for (DirectedEdge e = _edgeTo[s][t]; e != null; e = _edgeTo[s][e.From()])
            {
                path.Push(e);
            }
            return path;
        }

        /// <summary>
        /// Check optimality conditions
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Check(EdgeWeightedDigraph g, int s)
        {
            // no negative cycle
            if (!HasNegativeCycle())
            {
                for (int v = 0; v < g.V(); v++)
                {
                    foreach (DirectedEdge e in g.Adj(v))
                    {
                        int w = e.To();
                        for (int i = 0; i < g.V(); i++)
                        {
                            if (_distTo[i][w] > _distTo[i][v] + e.Weight())
                            {
                                Console.Error.WriteLine("edge " + e + " is eligible");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Unit tests the FloydWarshall data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Random random = new Random();

            // random graph with V vertices and E edges, parallel edges allowed
            int v = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            AdjMatrixEdgeWeightedDigraph g = new AdjMatrixEdgeWeightedDigraph(v);
            for (int i = 0; i < e; i++)
            {
                int vFrom = (int)(v * random.NextDouble());
                int wTo = (int)(v * random.NextDouble());
                double weight = Math.Round(100 * (random.NextDouble() - 0.15)) / 100.0;
                if (vFrom == wTo)
                {
                    g.AddEdge(new DirectedEdge(vFrom, wTo, Math.Abs(weight)));
                }
                else
                {
                    g.AddEdge(new DirectedEdge(vFrom, wTo, weight));
                }
            }

            StdOut.PrintLn(g);

            // run Floyd-Warshall algorithm
            FloydWarshall spt = new FloydWarshall(g);

            // Print all-pairs shortest path distances
            StdOut.PrintF("  ");
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                StdOut.PrintF("{0:000000} ", vertex);
            }
            StdOut.PrintLn();
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                StdOut.PrintF("{0:000}: ", vertex);
                for (int w = 0; w < g.V(); w++)
                {
                    if (spt.HasPath(vertex, w))
                    {
                        StdOut.PrintF("{0:000000.00} ", spt.Dist(vertex, w));
                    }
                    else
                    {
                        StdOut.PrintF("  Inf ");
                    }
                }
                StdOut.PrintLn();
            }

            // Print negative cycle
            if (spt.HasNegativeCycle())
            {
                StdOut.PrintLn("Negative cost cycle:");
                foreach (DirectedEdge edge in spt.NegativeCycle())
                {
                    StdOut.PrintLn(edge);
                }
                StdOut.PrintLn();
            }

            // Print all-pairs shortest paths
            else
            {
                for (int vertex = 0; vertex < g.V(); vertex++)
                {
                    for (int w = 0; w < g.V(); w++)
                    {
                        if (spt.HasPath(vertex, w))
                        {
                            StdOut.PrintF("{0} To {1} ({2:00000.00})  ", vertex, w, spt.Dist(vertex, w));
                            foreach (DirectedEdge edge in spt.Path(vertex, w))
                            {
                                StdOut.Print(edge + "  ");
                            }
                            StdOut.PrintLn();
                        }
                        else
                        {
                            StdOut.PrintF("{0} To {1} no path\n", vertex, w);
                        }
                    }
                }
            }
        }
    }
}