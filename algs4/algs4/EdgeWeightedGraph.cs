using System;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class EdgeWeightedGraph
    {
        private readonly int _v;
        private int _e;
        private readonly Bag<Edge>[] _adj;

        /// <summary>
        /// Initializes an empty edge-weighted graph with V vertices and 0 edges.
        /// param V the number of vertices
        /// </summary>
        /// <param name="v"></param>
        public EdgeWeightedGraph(int v)
        {
            if (v < 0)
            {
                throw new ArgumentException("Number of vertices must be nonnegative");
            }
            _v = v;
            _e = 0;
            _adj = new Bag<Edge>[v];
            for (int vertex = 0; vertex < v; vertex++)
            {
                _adj[vertex] = new Bag<Edge>();
            }
        }

        /// <summary>
        /// Initializes a random edge-weighted graph with V vertices and E edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        public EdgeWeightedGraph(int v, int e)
            : this(v)
        {
            if (e < 0)
            {
                throw new ArgumentException("Number of edges must be nonnegative");
            }
            Random random = new Random();
            for (int i = 0; i < e; i++)
            {
                int vFrom = (int)(random.NextDouble() * v);
                int wTo = (int)(random.NextDouble() * v);
                double weight = Math.Round(100 * random.NextDouble()) / 100.0;
                Edge edge = new Edge(vFrom, wTo, weight);
                AddEdge(edge);
            }
        }

        /// <summary>
        /// Initializes an edge-weighted graph from an input stream.
        /// The format is the number of vertices V,
        /// followed by the number of edges E,
        /// followed by E pairs of vertices and edge weights,
        /// with each entry separated by whitespace.
        /// </summary>
        /// <param name="input">the input stream</param>
        public EdgeWeightedGraph(In input)
            : this(input.ReadInt())
        {
            int e = input.ReadInt();
            if (e < 0)
            {
                throw new ArgumentException("Number of edges must be nonnegative");
            }
            for (int i = 0; i < e; i++)
            {
                int vFrom = input.ReadInt();
                int wTo = input.ReadInt();
                double weight = input.ReadDouble();
                Edge edge = new Edge(vFrom, wTo, weight);
                AddEdge(edge);
            }
        }

        /// <summary>
        /// Initializes a new edge-weighted graph that is a deep copy of G.
        /// </summary>
        /// <param name="g">the edge-weighted graph to copy</param>
        public EdgeWeightedGraph(EdgeWeightedGraph g)
            : this(g.V())
        {
            _e = g.E();
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                // reverse so that adjacency list is in same order as original
                Stack<Edge> reverse = new Stack<Edge>();
                foreach (Edge e in g._adj[vertex])
                {
                    reverse.Push(e);
                }
                foreach (Edge e in reverse)
                {
                    _adj[vertex].Add(e);
                }
            }
        }

        /// <summary>
        /// Returns the number of vertices in the edge-weighted graph.
        /// </summary>
        /// <returns>the number of vertices in the edge-weighted graph</returns>
        public int V()
        {
            return _v;
        }

        /// <summary>
        /// Returns the number of edges in the edge-weighted graph.
        /// </summary>
        /// <returns>the number of edges in the edge-weighted graph</returns>
        public int E()
        {
            return _e;
        }

        /// <summary>
        /// Throw an IndexOutOfBoundsException unless 0 &lt;= v &lt; V
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
        /// Adds the undirected edge e to the edge-weighted graph.
        /// </summary>
        /// <param name="e">the edge</param>
        public void AddEdge(Edge e)
        {
            int v = e.Either();
            int w = e.Other(v);
            ValidateVertex(v);
            ValidateVertex(w);
            _adj[v].Add(e);
            _adj[w].Add(e);
            _e++;
        }

        /// <summary>
        /// Returns the edges incident on vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the edges incident on vertex v as an Iterable</returns>
        public IEnumerable<Edge> Adj(int v)
        {
            ValidateVertex(v);
            return _adj[v];
        }

        /// <summary>
        /// Returns the degree of vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the degree of vertex v</returns>
        public int Degree(int v)
        {
            ValidateVertex(v);
            return _adj[v].Size();
        }

        /// <summary>
        /// Returns all edges in the edge-weighted graph.
        /// To iterate over the edges in the edge-weighted graph, use foreach notation:
        /// foreach (Edge e in G.edges()).
        /// </summary>
        /// <returns>all edges in the edge-weighted graph as an Iterable.</returns>
        public IEnumerable<Edge> Edges()
        {
            Bag<Edge> list = new Bag<Edge>();
            for (int v = 0; v < _v; v++)
            {
                int selfLoops = 0;
                foreach (Edge e in Adj(v))
                {
                    if (e.Other(v) > v)
                    {
                        list.Add(e);
                    }
                    // only add one copy of each self loop (self loops will be consecutive)
                    else if (e.Other(v) == v)
                    {
                        if (selfLoops % 2 == 0)
                        {
                            list.Add(e);
                        }
                        selfLoops++;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Returns a string representation of the edge-weighted graph.
        /// This method takes time proportional to E + V.
        /// </summary>
        /// <returns>the number of vertices V, followed by the number of edges E, followed by the V adjacency lists of edges</returns>
        public override string ToString()
        {
            string newline = Environment.NewLine;
            StringBuilder s = new StringBuilder();
            s.Append(_v + " " + _e + newline);
            for (int v = 0; v < _v; v++)
            {
                s.Append(v + ": ");
                foreach (Edge e in _adj[v])
                {
                    s.Append(e + "  ");
                }
                s.Append(newline);
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the EdgeWeightedGraph data type.
        /// </summary>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            EdgeWeightedGraph g = new EdgeWeightedGraph(input);
            StdOut.PrintLn(g);
        }
    }
}