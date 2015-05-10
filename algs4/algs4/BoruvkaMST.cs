using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BoruvkaMST
    {
        /// <summary>
        /// Edges in MST
        /// </summary>
        private readonly Bag<Edge> _mst = new Bag<Edge>();

        /// <summary>
        /// Weight of MST
        /// </summary>
        private readonly double _weight;

        /// <summary>
        /// Compute a minimum spanning tree (or forest) of an edge-weighted graph.
        /// </summary>
        /// <param name="g">the edge-weighted graph</param>
        public BoruvkaMST(EdgeWeightedGraph g)
        {
            UF uf = new UF(g.V());

            // repeat at most log V times or until we have V-1 edges
            for (int t = 1; t < g.V() && _mst.Size() < g.V() - 1; t = t + t)
            {
                // foreach tree in forest, find closest edge
                // if edge weights are equal, ties are broken in favor of first edge in G.Edges()
                Edge[] closest = new Edge[g.V()];
                foreach (Edge e in g.Edges())
                {
                    int v = e.Either(), w = e.Other(v);
                    int i = uf.Find(v), j = uf.Find(w);
                    if (i == j)
                    {
                        continue; // same tree
                    }
                    if (closest[i] == null || Less(e, closest[i]))
                    {
                        closest[i] = e;
                    }
                    if (closest[j] == null || Less(e, closest[j]))
                    {
                        closest[j] = e;
                    }
                }

                // add newly discovered edges to MST
                for (int i = 0; i < g.V(); i++)
                {
                    Edge e = closest[i];
                    if (e != null)
                    {
                        int v = e.Either(), w = e.Other(v);
                        // don't add the same edge twice
                        if (!uf.Connected(v, w))
                        {
                            _mst.Add(e);
                            _weight += e.Weight();
                            uf.Union(v, w);
                        }
                    }
                }
            }

            // check optimality conditions
            Debug.Assert(Check(g));
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
        /// Is the weight of edge e strictly less than that of edge f?
        /// </summary>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        private static bool Less(Edge e, Edge f)
        {
            return e.Weight() < f.Weight();
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
        /// Unit tests the BoruvkaMST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedGraph g = new EdgeWeightedGraph(input);
            BoruvkaMST mst = new BoruvkaMST(g);
            foreach (Edge e in mst.Edges())
            {
                StdOut.PrintLn(e);
            }
            StdOut.PrintF("{0:0.00000}\n", mst.Weight());
        }
    }
}