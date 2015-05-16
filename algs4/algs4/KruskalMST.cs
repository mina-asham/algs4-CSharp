using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class KruskalMST
    {
        /// <summary>
        /// Weight of MST
        /// </summary>
        private readonly double _weight;

        /// <summary>
        /// Edges in MST
        /// </summary>
        private readonly Queue<Edge> _mst = new Queue<Edge>();

        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="g">the edge-weighted graph</param>
        public KruskalMST(EdgeWeightedGraph g)
        {
            // more efficient to build heap by passing array of Edges
            MinPQ<Edge> pq = new MinPQ<Edge>();
            foreach (Edge e in g.Edges())
            {
                pq.Insert(e);
            }

            // run greedy algorithm
            UF uf = new UF(g.V());
            while (!pq.IsEmpty() && _mst.Size() < g.V() - 1)
            {
                Edge e = pq.DelMin();
                int v = e.Either();
                int w = e.Other(v);
                if (!uf.Connected(v, w))
                {
                    // v-w does not create a cycle
                    uf.Union(v, w); // merge v and w components
                    _mst.Enqueue(e); // add edge e to mst
                    _weight += e.Weight();
                }
            }

            // check optimality conditions
            Debug.Assert(Check(g));
        }

        /// <summary>
        /// Returns the Edges in a minimum spanning tree (or forest).
        /// </summary>
        /// <returns>the Edges in a minimum spanning tree (or forest) as an iterable of Edges</returns>
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
            // check total weight
            double total = 0.0;
            foreach (Edge e in Edges())
            {
                total += e.Weight();
            }
            double epsilon = 1E-12;
            if (Math.Abs(total - Weight()) > epsilon)
            {
                if (Math.Abs(total - Weight()) > epsilon)
                {
                    Console.Error.Write("Weight of Edges does not equal Weight(): {0} vs. {1}\n", total, Weight());
                    return false;
                }
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
                // all Edges in MST except e
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
        /// Unit tests the KruskalMST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedGraph g = new EdgeWeightedGraph(input);
            KruskalMST mst = new KruskalMST(g);
            foreach (Edge e in mst.Edges())
            {
                StdOut.PrintLn(e);
            }
            StdOut.PrintF("{0:0.00000}\n", mst.Weight());
        }
    }
}