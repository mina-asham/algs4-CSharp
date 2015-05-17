using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class PrimMST
    {
        /// <summary>
        /// edgeTo[v] = shortest edge from tree vertex to non-tree vertex
        /// </summary>
        private readonly Edge[] _edgeTo;

        /// <summary>
        /// distTo[v] = weight of shortest such edge
        /// </summary>
        private readonly double[] _distTo;

        /// <summary>
        /// marked[v] = true if v on tree, false otherwise
        /// </summary>
        private readonly bool[] _marked;

        private readonly IndexMinPQ<double> _pq;

        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="g">the edge-weighted graph</param>
        public PrimMST(EdgeWeightedGraph g)
        {
            _edgeTo = new Edge[g.V()];
            _distTo = new double[g.V()];
            _marked = new bool[g.V()];
            _pq = new IndexMinPQ<double>(g.V());
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = double.PositiveInfinity;
            }

            for (int v = 0; v < g.V(); v++) // run from each vertex to find
            {
                if (!_marked[v])
                {
                    Prim(g, v); // minimum spanning forest
                }
            }

            // check optimality conditions
            Debug.Assert(Check(g));
        }

        /// <summary>
        /// Run Prim's algorithm in graph G, starting from vertex s
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        private void Prim(EdgeWeightedGraph g, int s)
        {
            _distTo[s] = 0.0;
            _pq.Insert(s, _distTo[s]);
            while (!_pq.IsEmpty())
            {
                int v = _pq.DelMin();
                Scan(g, v);
            }
        }

        /// <summary>
        /// Scan vertex v
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void Scan(EdgeWeightedGraph g, int v)
        {
            _marked[v] = true;
            foreach (Edge e in g.Adj(v))
            {
                int w = e.Other(v);
                if (_marked[w])
                {
                    continue; // v-w is obsolete edge
                }
                if (e.Weight() < _distTo[w])
                {
                    _distTo[w] = e.Weight();
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
        }

        /// <summary>
        /// Returns the edges in a minimum spanning tree (or forest).
        /// </summary>
        /// <returns>the edges in a minimum spanning tree (or forest) as an iterable of edges</returns>
        public IEnumerable<Edge> Edges()
        {
            Queue<Edge> mst = new Queue<Edge>();
            for (int v = 0; v < _edgeTo.Length; v++)
            {
                Edge e = _edgeTo[v];
                if (e != null)
                {
                    mst.Enqueue(e);
                }
            }
            return mst;
        }

        /// <summary>
        /// Returns the sum of the edge weights in a minimum spanning tree (or forest).
        /// </summary>
        /// <returns>the sum of the edge weights in a minimum spanning tree (or forest)</returns>
        public double Weight()
        {
            double weight = 0.0;
            foreach (Edge e in Edges())
            {
                weight += e.Weight();
            }
            return weight;
        }

        /// <summary>
        /// Check optimality conditions (takes time proportional to E V lg* V)
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool Check(EdgeWeightedGraph g)
        {
            // check weight
            double totalWeight = 0.0;
            foreach (Edge e in Edges())
            {
                totalWeight += e.Weight();
            }
            double epsilon = 1E-12;
            if (Math.Abs(totalWeight - Weight()) > epsilon)
            {
                Console.Error.Write("Weight of edges does not equal Weight(): {0} vs. {1}\n", totalWeight, Weight());
                return false;
            }

            // check that it is acyclic
            UF uf = new UF(g.V());
            foreach (Edge e in Edges())
            {
                int v = e.Either(), w = e.Other(v);
                if (uf.Connected(v, w))
                {
                    Console.Error.WriteLine("Not a forest");
                    return false;
                }
                uf.Union(v, w);
            }

            // check that it is a spanning forest
            foreach (Edge e in g.Edges())
            {
                int v = e.Either(), w = e.Other(v);
                if (!uf.Connected(v, w))
                {
                    Console.Error.WriteLine("Not a spanning forest");
                    return false;
                }
            }

            // check that it is a minimal spanning forest (cut optimality conditions)
            foreach (Edge e in Edges())
            {
                // all edges in MST except e
                uf = new UF(g.V());
                foreach (Edge f in Edges())
                {
                    int x = f.Either(), y = f.Other(x);
                    if (f != e)
                    {
                        uf.Union(x, y);
                    }
                }

                // check that e is min weight edge in crossing cut
                foreach (Edge f in g.Edges())
                {
                    int x = f.Either(), y = f.Other(x);
                    if (!uf.Connected(x, y))
                    {
                        if (f.Weight() < e.Weight())
                        {
                            Console.Error.WriteLine("Edge " + f + " violates cut optimality conditions");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Unit tests the PrimMST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedGraph g = new EdgeWeightedGraph(input);
            PrimMST mst = new PrimMST(g);
            foreach (Edge e in mst.Edges())
            {
                StdOut.PrintLn(e);
            }
            StdOut.PrintF("{0:0.00000}\n", mst.Weight());
        }
    }
}