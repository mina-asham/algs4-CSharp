using System;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class EdgeWeightedDigraph
    {
        private readonly int _v;
        private int _e;
        private readonly Bag<DirectedEdge>[] _adj;

        /// <summary>
        /// Initializes an empty edge-weighted digraph with v vertices and 0 edges.
        /// param v the number of vertices
        /// </summary>
        /// <param name="v"></param>
        public EdgeWeightedDigraph(int v)
        {
            if (v < 0)
            {
                throw new ArgumentException("Number of vertices in a Digraph must be nonnegative");
            }
            _v = v;
            _e = 0;
            _adj = new Bag<DirectedEdge>[v];
            for (int vertex = 0; vertex < v; vertex++)
            {
                _adj[vertex] = new Bag<DirectedEdge>();
            }
        }

        /// <summary>
        /// Initializes a random edge-weighted digraph with v vertices and E edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        public EdgeWeightedDigraph(int v, int e)
            : this(v)
        {
            if (e < 0)
            {
                throw new ArgumentException("Number of edges in a Digraph must be nonnegative");
            }
            Random random = new Random();
            for (int i = 0; i < e; i++)
            {
                int vEdge = (int)(random.NextDouble() * v);
                int wEdge = (int)(random.NextDouble() * v);
                double weight = Math.Round(100 * random.NextDouble()) / 100.0;
                DirectedEdge edge = new DirectedEdge(vEdge, wEdge, weight);
                AddEdge(edge);
            }
        }

        /// <summary>
        /// Initializes an edge-weighted digraph from an input stream.
        /// The format is the number of vertices v,
        /// followed by the number of edges E,
        /// followed by E pairs of vertices and edge weights,
        /// with each entry separated by whitespace.
        /// </summary>
        /// <param name="input">the input stream</param>
        public EdgeWeightedDigraph(In input)
            : this(input.ReadInt())
        {
            int e = input.ReadInt();
            if (e < 0)
            {
                throw new ArgumentException("Number of edges must be nonnegative");
            }
            for (int i = 0; i < e; i++)
            {
                int v = input.ReadInt();
                int w = input.ReadInt();
                if (v < 0 || v >= _v)
                {
                    throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (_v - 1));
                }
                if (w < 0 || w >= _v)
                {
                    throw new IndexOutOfRangeException("vertex " + w + " is not between 0 and " + (_v - 1));
                }
                double weight = input.ReadDouble();
                AddEdge(new DirectedEdge(v, w, weight));
            }
        }

        /// <summary>
        /// Initializes a new edge-weighted digraph that is a deep copy of G.
        /// </summary>
        /// <param name="g">the edge-weighted graph to copy</param>
        public EdgeWeightedDigraph(EdgeWeightedDigraph g)
            : this(g.V())
        {
            _e = g.E();
            for (int v = 0; v < g.V(); v++)
            {
                // reverse so that adjacency list is in same order as original
                Stack<DirectedEdge> reverse = new Stack<DirectedEdge>();
                foreach (DirectedEdge e in g._adj[v])
                {
                    reverse.Push(e);
                }
                foreach (DirectedEdge e in reverse)
                {
                    _adj[v].Add(e);
                }
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
        /// Throw an IndexOutOfBoundsException unless 0 &lt;= v &lt; _v
        /// </summary>
        /// <param name="v"></param>
        private void ValidateVertex(int v)
        {
            if (v < 0 || v >= _v)
            {
                throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (_v - 1));
            }
        }

        /// <summary>
        /// Adds the directed edge e to the edge-weighted digraph.
        /// </summary>
        /// <param name="e">the edge</param>
        public void AddEdge(DirectedEdge e)
        {
            int v = e.From();
            int w = e.To();
            ValidateVertex(v);
            ValidateVertex(w);
            _adj[v].Add(e);
            _e++;
        }

        /// <summary>
        /// Returns the directed edges incident from vertex v.
        /// </summary>
        /// <param name="v">the directed edges incident from vertex v as an IEnumerable</param>
        /// <returns>the vertex</returns>
        public IEnumerable<DirectedEdge> Adj(int v)
        {
            ValidateVertex(v);
            return _adj[v];
        }

        /// <summary>
        /// Returns the number of directed edges incident from vertex v.
        /// This is known as the outdegree of vertex v.
        /// </summary>
        /// <param name="v">the outdegree of vertex v</param>
        /// <returns>the vertex</returns>
        public int Outdegree(int v)
        {
            ValidateVertex(v);
            return _adj[v].Size();
        }

        /// <summary>
        /// Returns all directed edges in the edge-weighted digraph.
        /// To iterate over the edges in the edge-weighted graph, use foreach notation:
        /// for (DirectedEdge e : G.edges()).
        /// </summary>
        /// <returns>all edges in the edge-weighted graph as an Iterable.</returns>
        public IEnumerable<DirectedEdge> Edges()
        {
            Bag<DirectedEdge> list = new Bag<DirectedEdge>();
            for (int v = 0; v < _v; v++)
            {
                foreach (DirectedEdge e in Adj(v))
                {
                    list.Add(e);
                }
            }
            return list;
        }

        /// <summary>
        /// Returns a string representation of the edge-weighted digraph.
        /// This method takes time proportional to E + v.
        /// </summary>
        /// <returns>the number of vertices v, followed by the number of edges E, followed by the v adjacency lists of edges</returns>
        public override string ToString()
        {
            string newline = Environment.NewLine;
            StringBuilder s = new StringBuilder();
            s.Append(_v + " " + _e + newline);
            for (int v = 0; v < _v; v++)
            {
                s.Append(v + ": ");
                foreach (DirectedEdge e in _adj[v])
                {
                    s.Append(e + "  ");
                }
                s.Append(newline);
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the EdgeWeightedDigraph data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(input);
            StdOut.PrintLn(g);
        }
    }
}
