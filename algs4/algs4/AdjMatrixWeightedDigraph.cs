using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class AdjMatrixEdgeWeightedDigraph
    {
        private readonly int _v;
        private int _e;
        private readonly DirectedEdge[][] _adj;

        /// <summary>
        /// Initializes an empty edge-weighted digraph with V vertices and 0 edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public AdjMatrixEdgeWeightedDigraph(int v)
        {
            if (v < 0)
            {
                throw new ArgumentException("Number of vertices must be nonnegative");
            }
            _v = v;
            _e = 0;
            _adj = new DirectedEdge[v][];
            for (int i = 0; i < v; i++)
            {
                _adj[i] = new DirectedEdge[v];
            }
        }

        /// <summary>
        /// Initializes a random edge-weighted digraph with V vertices and E edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        public AdjMatrixEdgeWeightedDigraph(int v, int e)
            : this(v)
        {
            if (e < 0)
            {
                throw new ArgumentException("Number of edges must be nonnegative");
            }
            if (e > v * v)
            {
                throw new ArgumentException("Too many edges");
            }

            Random random = new Random();
            // can be inefficient
            while (_e != e)
            {
                int vVertex = (int)(v * random.NextDouble());
                int wVertex = (int)(v * random.NextDouble());
                double weight = Math.Round(100 * random.NextDouble()) / 100.0;
                AddEdge(new DirectedEdge(vVertex, wVertex, weight));
            }
        }

        /// <summary>
        /// Returns the number of vertices in the edge-weighted digraph.
        /// </summary>
        /// <returns>the number of vertices in the edge-weighted digraph</returns>
        public int V()
        {
            return _v;
        }

        /// <summary>
        /// Returns the number of edges in the edge-weighted digraph.
        /// </summary>
        /// <returns>the number of edges in the edge-weighted digraph</returns>
        public int E()
        {
            return _e;
        }

        /// <summary>
        /// Adds the directed edge edge to the edge-weighted digraph (if there
        /// is not already an edge with the same endpoints).
        /// </summary>
        /// <param name="edge">the edge</param>
        public void AddEdge(DirectedEdge edge)
        {
            int v = edge.From();
            int w = edge.To();
            if (_adj[v][w] == null)
            {
                _e++;
                _adj[v][w] = edge;
            }
        }

        /// <summary>
        /// Returns the directed edges incident from vertex v.
        /// </summary>
        /// <param name="v">the directed edges incident from vertex v as an Iterable</param>
        /// <returns>the vertex</returns>
        public IEnumerable<DirectedEdge> Adj(int v)
        {
            return new AdjIterator(this, v);
        }

        /// <summary>
        /// Support iteration over graph vertices
        /// </summary>
        private class AdjIterator : IEnumerator<DirectedEdge>, IEnumerable<DirectedEdge>
        {
            private AdjMatrixEdgeWeightedDigraph _digraph;
            private readonly int _v;
            private int _w;

            public DirectedEdge Current { get; private set; }

            public AdjIterator(AdjMatrixEdgeWeightedDigraph digraph, int v)
            {
                _digraph = digraph;
                _v = v;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                while (_w < _digraph._v)
                {
                    if (_digraph._adj[_v][_w] != null)
                    {
                        Current = _digraph._adj[_v][_w];
                        return true;
                    }
                    _w++;
                }
                Current = null;
                return false;
            }

            public void Reset()
            {
                _w = 0;
            }

            public void Dispose()
            {
                _digraph = null;
            }

            public IEnumerator<DirectedEdge> GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// Returns a string representation of the edge-weighted digraph. This method takes
        /// time proportional to v^2.
        /// </summary>
        /// <returns>the number of vertices v, followed by the number of edges edge, followed by the v adjacency lists of edges</returns>
        public override string ToString()
        {
            string newline = Environment.NewLine;
            StringBuilder s = new StringBuilder();
            s.Append(_v + " " + _e + newline);
            for (int vertex = 0; vertex < _v; vertex++)
            {
                s.Append(vertex + ": ");
                foreach (DirectedEdge edge in Adj(vertex))
                {
                    s.Append(edge + "  ");
                }
                s.Append(newline);
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the AdjMatrixEdgeWeightedDigraph data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int v = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            AdjMatrixEdgeWeightedDigraph g = new AdjMatrixEdgeWeightedDigraph(v, e);
            StdOut.PrintLn(g);
        }
    }
}