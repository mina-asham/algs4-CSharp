using System;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class FlowNetwork
    {
        private readonly int _v;
        private int _e;
        private readonly Bag<FlowEdge>[] _adj;

        /// <summary>
        /// Initializes an empty flow network with <tt>V</tt> vertices and 0 edges.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        public FlowNetwork(int v)
        {
            if (v < 0)
            {
                throw new ArgumentException("Number of vertices in a Graph must be nonnegative");
            }
            _v = v;
            _e = 0;
            _adj = new Bag<FlowEdge>[v];
            for (int vertex = 0; vertex < v; vertex++)
            {
                _adj[vertex] = new Bag<FlowEdge>();
            }
        }

        /// <summary>
        /// Initializes a random flow network with <tt>V</tt> vertices and <em>E</em> edges.
        /// The capacities are integers between 0 and 99 and the flow values are zero.
        /// </summary>
        /// <param name="v">the number of vertices</param>
        /// <param name="e">the number of edges</param>
        public FlowNetwork(int v, int e)
            : this(v)
        {
            if (e < 0)
            {
                throw new ArgumentException("Number of edges must be nonnegative");
            }
            for (int i = 0; i < e; i++)
            {
                int vFrom = StdRandom.Uniform(v);
                int wTo = StdRandom.Uniform(v);
                double capacity = StdRandom.Uniform(100);
                AddEdge(new FlowEdge(vFrom, wTo, capacity));
            }
        }

        /// <summary>
        /// Initializes a flow network from an input stream.
        /// The format is the number of vertices <em>V</em>,
        /// followed by the number of edges <em>E</em>,
        /// followed by <em>E</em> pairs of vertices and edge capacities,
        /// with each entry separated by whitespace.
        /// </summary>
        /// <param name="input">the input stream</param>
        public FlowNetwork(In input)
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
                if (vFrom < 0 || vFrom >= _v)
                {
                    throw new IndexOutOfRangeException("vertex " + vFrom + " is not between 0 and " + (_v - 1));
                }
                if (wTo < 0 || wTo >= _v)
                {
                    throw new IndexOutOfRangeException("vertex " + wTo + " is not between 0 and " + (_v - 1));
                }
                double capacity = input.ReadDouble();
                AddEdge(new FlowEdge(vFrom, wTo, capacity));
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
        /// Adds the edge <tt>e</tt> to the network.
        /// </summary>
        /// <param name="e">the edge</param>
        public void AddEdge(FlowEdge e)
        {
            int v = e.From();
            int w = e.To();
            ValidateVertex(v);
            ValidateVertex(w);
            _adj[v].Add(e);
            _adj[w].Add(e);
            _e++;
        }

        /// <summary>
        /// Returns the edges incident on vertex <tt>v</tt> (includes both edges pointing to
        /// and from <tt>v</tt>).
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the edges incident on vertex <tt>v</tt> as an IEnumerable</returns>
        public IEnumerable<FlowEdge> Adj(int v)
        {
            ValidateVertex(v);
            return _adj[v];
        }

        /// <summary>
        /// Return list of all edges - excludes self loops
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FlowEdge> Edges()
        {
            Bag<FlowEdge> list = new Bag<FlowEdge>();
            for (int v = 0; v < _v; v++)
            {
                foreach (FlowEdge e in Adj(v))
                {
                    if (e.To() != v)
                    {
                        list.Add(e);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Returns a string representation of the flow network.
        /// This method takes time proportional to <em>E</em> + <em>V</em>.
        /// </summary>
        /// <returns>the number of vertices <em>V</em>, followed by the number of edges <em>E</em>, followed by the <em>V</em> adjacency lists</returns>
        public override string ToString()
        {
            string newline = Environment.NewLine;
            StringBuilder s = new StringBuilder();
            s.Append(_v + " " + _e + newline);
            for (int v = 0; v < _v; v++)
            {
                s.Append(v + ":  ");
                foreach (FlowEdge e in _adj[v])
                {
                    if (e.To() != v)
                    {
                        s.Append(e + "  ");
                    }
                }
                s.Append(newline);
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the <tt>FlowNetwork</tt> data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            FlowNetwork g = new FlowNetwork(input);
            StdOut.PrintLn(g);
        }
    }
}