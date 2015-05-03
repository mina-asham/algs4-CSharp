using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Edge : IComparable<Edge>
    {
        private readonly int _v;
        private readonly int _w;
        private readonly double _weight;

        /// <summary>
        /// Initializes an edge between vertices v and w of
        /// the given <tt>weight</tt>.
        /// </summary>
        /// <param name="v">one vertex</param>
        /// <param name="w">the other vertex</param>
        /// <param name="weight">the weight of the edge</param>
        public Edge(int v, int w, double weight)
        {
            if (v < 0)
            {
                throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
            }
            if (w < 0)
            {
                throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
            }
            if (double.IsNaN(weight))
            {
                throw new ArgumentException("Weight is NaN");
            }
            _v = v;
            _w = w;
            _weight = weight;
        }

        /// <summary>
        /// Returns the weight of the edge.
        /// </summary>
        /// <returns>the weight of the edge</returns>
        public double Weight()
        {
            return _weight;
        }

        /// <summary>
        /// Returns either endpoint of the edge.
        /// </summary>
        /// <returns>either endpoint of the edge</returns>
        public int Either()
        {
            return _v;
        }

        /// <summary>
        /// Returns the endpoint of the edge that is different from the given vertex
        /// (unless the edge represents a self-loop in which case it returns the same vertex).
        /// </summary>
        /// <param name="vertex">one endpoint of the edge</param>
        /// <returns>the endpoint of the edge that is different from the given vertex (unless the edge represents a self-loop in which case it returns the same vertex)</returns>
        public int Other(int vertex)
        {
            if (vertex == _v)
            {
                return _w;
            }
            if (vertex == _w)
            {
                return _v;
            }
            throw new ArgumentException("Illegal endpoint");
        }

        /// <summary>
        /// Compares two edges by weight.
        /// </summary>
        /// <param name="that">the other edge</param>
        /// <returns>a negative integer, zero, or positive integer depending on whether this edge is less than, equal to, or greater than that edge</returns>
        public int CompareTo(Edge that)
        {
            if (Weight() < that.Weight())
            {
                return -1;
            }
            if (Weight() > that.Weight())
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Returns a string representation of the edge.
        /// </summary>
        /// <returns>a string representation of the edge</returns>
        public override string ToString()
        {
            return string.Format("{0}-{1} {2:0.00000}", _v, _w, _weight);
        }

        /// <summary>
        /// Unit tests the <tt>Edge</tt> data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Edge e = new Edge(12, 23, 3.14);
            StdOut.PrintLn(e);
        }
    }
}
