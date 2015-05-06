using System;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Graph
    {
        private readonly int _v;
        private int _e;
        private readonly Bag<int>[] _adj;

        /// <summary>
        /// Initializes an empty graph with V vertices and 0 edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public Graph(int v)
        {
            if (v < 0)
            {
                throw new ArgumentException("Number of vertices must be nonnegative");
            }
            _v = v;
            _e = 0;
            _adj = new Bag<int>[v];
            for (int vertex = 0; vertex < v; vertex++)
            {
                _adj[vertex] = new Bag<int>();
            }
        }

        /// <summary>
        /// Initializes a graph from an input stream.
        /// The format is the number of vertices <em>V</em>,
        /// followed by the number of edges <em>E</em>,
        /// followed by <em>E</em> pairs of vertices, with each entry separated by whitespace.
        /// </summary>
        /// <param name="input">the input stream</param>
        public Graph(In input)
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
                AddEdge(v, w);
            }
        }

        /// <summary>
        /// Initializes a new graph that is a deep copy of G.
        /// </summary>
        /// <param name="g">the graph to copy</param>
        public Graph(Graph g)
            : this(g.V())
        {
            _e = g.E();
            for (int v = 0; v < g.V(); v++)
            {
                // reverse so that adjacency list is in same order as original
                Stack<int> reverse = new Stack<int>();
                foreach (int w in g._adj[v])
                {
                    reverse.Push(w);
                }
                foreach (int w in reverse)
                {
                    _adj[v].Add(w);
                }
            }
        }

        /// <summary>
        /// Returns the number of vertices in the graph.
        /// </summary>
        /// <returns>the number of vertices in the graph</returns>
        public int V()
        {
            return _v;
        }

        /// <summary>
        /// Returns the number of edges in the graph.
        /// </summary>
        /// <returns>the number of edges in the graph</returns>
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
        /// Adds the undirected edge v-w to the graph.
        /// </summary>
        /// <param name="v">one vertex in the edge</param>
        /// <param name="w">the other vertex in the edge</param>
        public void AddEdge(int v, int w)
        {
            ValidateVertex(v);
            ValidateVertex(w);
            _e++;
            _adj[v].Add(w);
            _adj[w].Add(v);
        }

        /// <summary>
        /// Returns the vertices adjacent to vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the vertices adjacent to vertex v as an Iterable</returns>
        public IEnumerable<int> Adj(int v)
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
        /// Returns a string representation of the graph.
        /// This method takes time proportional to E + V.
        /// </summary>
        /// <returns>the number of vertices V, followed by the number of edges E, followed by the V adjacency lists</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            string newline = Environment.NewLine;
            s.Append(_v + " vertices, " + _e + " edges " + newline);
            for (int v = 0; v < _v; v++)
            {
                s.Append(v + ": ");
                foreach (int w in _adj[v])
                {
                    s.Append(w + " ");
                }
                s.Append(newline);
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the Graph data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Graph g = new Graph(input);
            StdOut.PrintLn(g);
        }
    }
}