using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class AcyclicSP
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
        /// Computes a shortest paths tree from s to every other vertex in
        /// the directed acyclic graph G.
        /// </summary>
        /// <param name="g">the acyclic digraph</param>
        /// <param name="s">the source vertex</param>
        public AcyclicSP(EdgeWeightedDigraph g, int s)
        {
            _distTo = new double[g.V()];
            _edgeTo = new DirectedEdge[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                _distTo[v] = double.PositiveInfinity;
            }
            _distTo[s] = 0.0;

            // visit vertices in toplogical order
            Topological topological = new Topological(g);
            if (!topological.HasOrder())
            {
                throw new ArgumentException("Digraph is not acyclic.");
            }
            foreach (int v in topological.Order())
            {
                foreach (DirectedEdge e in g.Adj(v))
                {
                    Relax(e);
                }
            }
        }

        /// <summary>
        /// Relax edge e
        /// </summary>
        /// <param name="e"></param>
        private void Relax(DirectedEdge e)
        {
            int v = e.From(), w = e.To();
            if (_distTo[w] > _distTo[v] + e.Weight())
            {
                _distTo[w] = _distTo[v] + e.Weight();
                _edgeTo[w] = e;
            }
        }

        /// <summary>
        /// Returns the length of a shortest path from the source vertex s to vertex v.
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>the length of a shortest path from the source vertex s to vertex v double.PositiveInfinity if no such path</returns>
        public double DistTo(int v)
        {
            return _distTo[v];
        }

        /// <summary>
        /// Is there a path from the source vertex s to vertex v?
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>true if there is a path from the source vertex  s to vertex v, and false otherwise</returns>
        public bool HasPathTo(int v)
        {
            return _distTo[v] < double.PositiveInfinity;
        }

        /// <summary>
        /// Returns a shortest path from the source vertex s to vertex v.
        /// </summary>
        /// <param name="v">the destination vertex</param>
        /// <returns>a shortest path from the source vertex s to vertex v   as an IEnumerable of edges, and null if no such path</returns>
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
        /// Unit tests the AcyclicSP data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            int s = int.Parse(args[1]);
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(input);

            // find shortest path from s to each other vertex in DAG
            AcyclicSP sp = new AcyclicSP(g, s);
            for (int v = 0; v < g.V(); v++)
            {
                if (sp.HasPathTo(v))
                {
                    StdOut.PrintF("{0} to {1} ({2:0.00})  ", s, v, sp.DistTo(v));
                    foreach (DirectedEdge e in sp.PathTo(v))
                    {
                        StdOut.Print(e + "   ");
                    }
                    StdOut.PrintLn();
                }
                else
                {
                    StdOut.PrintF("{0} to {1}         no path\n", s, v);
                }
            }
        }
    }
}