using System;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Digraph
    {
        private readonly int _v;
        private int _e;
        private readonly Bag<int>[] _adj;

        /// <summary>
        /// Initializes an empty digraph with v vertices.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public Digraph(int v)
        {
            if (v < 0)
            {
                throw new ArgumentException("Number of vertices in a Digraph must be nonnegative");
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
        /// Initializes a digraph from an input stream.
        /// The format is the number of vertices v,
        /// followed by the number of edges E,
        /// followed by E pairs of vertices, with each entry separated by whitespace.
        /// </summary>
        /// <param name="input">the input stream</param>
        public Digraph(In input)
        {
            _v = input.ReadInt();
            if (_v < 0)
            {
                throw new ArgumentException("Number of vertices in a Digraph must be nonnegative");
            }
            _adj = new Bag<int>[_v];
            for (int v = 0; v < _v; v++)
            {
                _adj[v] = new Bag<int>();
            }
            int e = input.ReadInt();
            if (e < 0)
            {
                throw new ArgumentException("Number of edges in a Digraph must be nonnegative");
            }
            for (int i = 0; i < e; i++)
            {
                int v = input.ReadInt();
                int w = input.ReadInt();
                AddEdge(v, w);
            }
        }

        /// <summary>
        /// Initializes a new digraph that is a deep copy of G.
        /// </summary>
        /// <param name="g">the digraph to copy</param>
        public Digraph(Digraph g)
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
        /// Returns the number of vertices in the digraph.
        /// </summary>
        /// <returns>the number of vertices in the digraph</returns>
        public int V()
        {
            return _v;
        }

        /// <summary>
        /// Returns the number of edges in the digraph.
        /// </summary>
        /// <returns>the number of edges in the digraph</returns>
        public int E()
        {
            return _e;
        }

        /// <summary>
        /// Throw an IndexOutOfRangeException unless 0 &lt;= v &lt; _v
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
        /// Adds the directed edge v->w to the digraph.
        /// </summary>
        /// <param name="v">the tail vertex</param>
        /// <param name="w">the head vertex</param>
        public void AddEdge(int v, int w)
        {
            ValidateVertex(v);
            ValidateVertex(w);
            _adj[v].Add(w);
            _e++;
        }

        /// <summary>
        /// Returns the vertices adjacent from vertex v in the digraph.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the vertices adjacent from vertex v in the digraph, as an IEnumerable</returns>
        public IEnumerable<int> Adj(int v)
        {
            ValidateVertex(v);
            return _adj[v];
        }

        /// <summary>
        /// Returns the number of directed edges incident from vertex v.
        /// This is known as the outdegree of vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the outdegree of vertex v</returns>
        public int OutDegree(int v)
        {
            ValidateVertex(v);
            return _adj[v].Size();
        }

        /// <summary>
        /// Returns the reverse of the digraph.
        /// </summary>
        /// <returns>the reverse of the digraph</returns>
        public Digraph Reverse()
        {
            Digraph r = new Digraph(_v);
            for (int v = 0; v < _v; v++)
            {
                foreach (int w in Adj(v))
                {
                    r.AddEdge(w, v);
                }
            }
            return r;
        }

        /// <summary>
        /// Returns a string representation of the graph.
        /// This method takes time proportional to E + v.
        /// </summary>
        /// <returns>the number of vertices v, followed by the number of edges E, followed by the v adjacency lists</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            string newline = Environment.NewLine;
            s.Append(_v + " vertices, " + _e + " edges " + newline);
            for (int v = 0; v < _v; v++)
            {
                s.Append(string.Format("{0}: ", v));
                foreach (int w in _adj[v])
                {
                    s.Append(string.Format("{0} ", w));
                }
                s.Append(newline);
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the Digraph data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);
            StdOut.PrintLn(g);
        }
    }
}
