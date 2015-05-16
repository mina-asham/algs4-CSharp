using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LazyPrimMST
    {
        /// <summary>
        /// Total weight of MST
        /// </summary>
        private double _weight;

        /// <summary>
        /// Edges in the MST
        /// </summary>
        private readonly Queue<Edge> _mst;

        /// <summary>
        /// marked[v] = true if v on tree
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// Edges with one endpoint in tree
        /// </summary>
        private readonly MinPQ<Edge> _pq;

        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="g">the edge-weighted graph</param>
        public LazyPrimMST(EdgeWeightedGraph g)
        {
            _mst = new Queue<Edge>();
            _pq = new MinPQ<Edge>();
            _marked = new bool[g.V()];
            for (int v = 0; v < g.V(); v++) // run Prim from all vertices to
            {
                if (!_marked[v])
                {
                    Prim(g, v); // get a minimum spanning forest
                }
            }

            // check optimality conditions
            Debug.Assert(Check(g));
        }

        /// <summary>
        /// Run Prim's algorithm
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        private void Prim(EdgeWeightedGraph g, int s)
        {
            Scan(g, s);
            while (!_pq.IsEmpty())
            {
                // better to stop when mst has V-1 edges
                Edge e = _pq.DelMin(); // smallest edge on pq
                int v = e.Either(), w = e.Other(v); // two endpoints
                Debug.Assert(_marked[v] || _marked[w]);
                if (_marked[v] && _marked[w])
                {
                    continue; // lazy, both v and w already scanned
                }
                _mst.Enqueue(e); // add e to MST
                _weight += e.Weight();
                if (!_marked[v])
                {
                    Scan(g, v); // v becomes part of tree
                }
                if (!_marked[w])
                {
                    Scan(g, w); // w becomes part of tree
                }
            }
        }

        /// <summary>
        /// Add all edges e incident to v onto pq if the other endpoint has not yet been scanned
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void Scan(EdgeWeightedGraph g, int v)
        {
            Debug.Assert(!_marked[v]);
            _marked[v] = true;
            foreach (Edge e in g.Adj(v))
            {
                if (!_marked[e.Other(v)])
                {
                    _pq.Insert(e);
                }
            }
        }

        /// <summary>
        /// Returns the edges in a minimum spanning tree (or forest).
        /// </summary>
        /// <returns>the edges in a minimum spanning tree (or forest) as an iterable of edges</returns>
        public IEnumerable<Edge> Edges()
        {
            return _mst;
        }

        /// <summary>
        /// Returns the sum of the edge weights in a minimum spanning tree (or forest).
        /// </summary>
        /// <returns>the sum of the edge weights in a minimum spanning tree (or forest)</returns>
        public double Weight()
        {
            return _weight;
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
            const double epsilon = 1E-12;
            if (Math.Abs(totalWeight - Weight()) > epsilon)
            {
                Console.Error.Write("Weight of edges does not equal Weight():{0} vs. {1}\n", totalWeight, Weight());
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
                foreach (Edge f in _mst)
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
        /// Unit tests the LazyPrimMST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedGraph g = new EdgeWeightedGraph(input);
            LazyPrimMST mst = new LazyPrimMST(g);
            foreach (Edge e in mst.Edges())
            {
                StdOut.PrintLn(e);
            }
            StdOut.PrintF("{0:0.00000}\n", mst.Weight());
        }
    }
}