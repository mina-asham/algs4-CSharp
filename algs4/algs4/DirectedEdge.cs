using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DirectedEdge
    {
        private readonly int _v;
        private readonly int _w;
        private readonly double _weight;

        /// <summary>
        /// Initializes a directed edge from vertex v to vertex w with
        /// the given weight.
        /// </summary>
        /// <param name="v">the tail vertex</param>
        /// <param name="w">the head vertex</param>
        /// <param name="weight">the weight of the directed edge</param>
        public DirectedEdge(int v, int w, double weight)
        {
            if (v < 0)
            {
                throw new IndexOutOfRangeException("Vertex names must be nonnegative integers");
            }
            if (w < 0)
            {
                throw new IndexOutOfRangeException("Vertex names must be nonnegative integers");
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
        /// Returns the tail vertex of the directed edge.
        /// </summary>
        /// <returns>the tail vertex of the directed edge</returns>
        public int From()
        {
            return _v;
        }

        /// <summary>
        /// Returns the head vertex of the directed edge.
        /// </summary>
        /// <returns>the head vertex of the directed edge</returns>
        public int To()
        {
            return _w;
        }

        /// <summary>
        /// Returns the weight of the directed edge.
        /// </summary>
        /// <returns>the weight of the directed edge</returns>
        public double Weight()
        {
            return _weight;
        }

        /// <summary>
        /// Returns a string representation of the directed edge.
        /// </summary>
        /// <returns>a string representation of the directed edge</returns>
        public override string ToString()
        {
            return _v + "->" + _w + " " + string.Format("{0:00000.00}", _weight);
        }

        /// <summary>
        /// Unit tests the DirectedEdge data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            DirectedEdge e = new DirectedEdge(12, 23, 3.14);
            StdOut.PrintLn(e);
        }
    }
}