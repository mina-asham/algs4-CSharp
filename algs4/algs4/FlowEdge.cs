using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class FlowEdge
    {
        /// <summary>
        /// From
        /// </summary>
        private readonly int _v;

        /// <summary>
        /// To
        /// </summary>
        private readonly int _w;

        /// <summary>
        /// Capacity
        /// </summary>
        private readonly double _capacity;

        /// <summary>
        /// Flow
        /// </summary>
        private double _flow;

        /// <summary>
        /// Initializes an edge from vertex v to vertex w with
        /// the given capacity and zero flow.
        /// </summary>
        /// <param name="v">the tail vertex</param>
        /// <param name="w">the head vertex</param>
        /// <param name="capacity">the capacity of the edge</param>
        public FlowEdge(int v, int w, double capacity)
        {
            if (v < 0)
            {
                throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
            }
            if (w < 0)
            {
                throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
            }
            if (!(capacity >= 0.0))
            {
                throw new ArgumentException("Edge capacity must be nonnegaitve");
            }
            _v = v;
            _w = w;
            _capacity = capacity;
            _flow = 0.0;
        }

        /// <summary>
        /// Initializes an edge from vertex v to vertex w with
        /// the given capacity and flow.
        /// </summary>
        /// <param name="v">the tail vertex</param>
        /// <param name="w">the head vertex</param>
        /// <param name="capacity">the capacity of the edge</param>
        /// <param name="flow">the flow on the edge</param>
        public FlowEdge(int v, int w, double capacity, double flow)
        {
            if (v < 0)
            {
                throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
            }
            if (w < 0)
            {
                throw new IndexOutOfRangeException("Vertex name must be a nonnegative integer");
            }
            if (!(capacity >= 0.0))
            {
                throw new ArgumentException("Edge capacity must be nonnegaitve");
            }
            if (!(flow <= capacity))
            {
                throw new ArgumentException("Flow exceeds capacity");
            }
            if (!(flow >= 0.0))
            {
                throw new ArgumentException("Flow must be nonnnegative");
            }
            _v = v;
            _w = w;
            _capacity = capacity;
            _flow = flow;
        }

        /// <summary>
        /// Initializes a flow edge from another flow edge.
        /// </summary>
        /// <param name="e">the edge to copy</param>
        public FlowEdge(FlowEdge e)
        {
            _v = e._v;
            _w = e._w;
            _capacity = e._capacity;
            _flow = e._flow;
        }

        /// <summary>
        /// Returns the tail vertex of the edge.
        /// </summary>
        /// <returns>the tail vertex of the edge</returns>
        public int From()
        {
            return _v;
        }

        /// <summary>
        /// Returns the head vertex of the edge.
        /// </summary>
        /// <returns>the head vertex of the edge</returns>
        public int To()
        {
            return _w;
        }

        /// <summary>
        /// Returns the capacity of the edge.
        /// </summary>
        /// <returns>the capacity of the edge</returns>
        public double Capacity()
        {
            return _capacity;
        }

        /// <summary>
        /// Returns the flow on the edge.
        /// </summary>
        /// <returns>the flow on the edge</returns>
        public double Flow()
        {
            return _flow;
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
        /// Returns the residual capacity of the edge in the direction to the given vertex.
        /// </summary>
        /// <param name="vertex">one endpoint of the edge</param>
        /// <returns>the residual capacity of the edge in the direction to the given vertex. If vertex is the tail vertex, the residual capacity equals capacity() - flow(); if vertex is the head vertex, the residual capacity equals flow().</returns>
        public double ResidualCapacityTo(int vertex)
        {
            // backward edge
            if (vertex == _v)
            {
                return _flow;
            }

            // forward edge
            if (vertex == _w)
            {
                return _capacity - _flow;
            }

            throw new ArgumentException("Illegal endpoint");
        }

        /// <summary>
        /// Increases the flow on the edge in the direction to the given vertex.
        ///   If vertex is the tail vertex, this increases the flow on the edge by delta;
        ///   if vertex is the head vertex, this decreases the flow on the edge by delta.
        /// </summary>
        /// <param name="vertex">one endpoint of the edge</param>
        /// <param name="delta"></param>
        public void AddResidualFlowTo(int vertex, double delta)
        {
            // backward edge
            if (vertex == _v)
            {
                _flow -= delta;
            }

            // forward edge
            else if (vertex == _w)
            {
                _flow += delta;
            }
            else
            {
                throw new ArgumentException("Illegal endpoint");
            }
            if (double.IsNaN(delta))
            {
                throw new ArgumentException("Change in flow = NaN");
            }
            if (!(_flow >= 0.0))
            {
                throw new ArgumentException("Flow is negative");
            }
            if (!(_flow <= _capacity))
            {
                throw new ArgumentException("Flow exceeds capacity");
            }
        }

        /// <summary>
        /// Returns a string representation of the edge.
        /// </summary>
        /// <returns>a string representation of the edge</returns>
        public override string ToString()
        {
            return _v + "->" + _w + " " + _flow + "/" + _capacity;
        }

        /// <summary>
        /// Unit tests the FlowEdge data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            FlowEdge e = new FlowEdge(12, 23, 3.14);
            StdOut.PrintLn(e);
        }
    }
}