using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class FordFulkerson
    {
        /// <summary>
        /// marked[v] = true iff s->v path in residual graph
        /// </summary>
        private bool[] _marked;

        /// <summary>
        /// edgeTo[v] = last edge on shortest residual s->v path
        /// </summary>
        private FlowEdge[] _edgeTo;

        /// <summary>
        /// Current value of max flow
        /// </summary>
        private readonly double _value;

        /// <summary>
        /// Compute a maximum flow and minimum cut in the network G
        /// from vertex s to vertex t.
        /// </summary>
        /// <param name="g">the flow network</param>
        /// <param name="s">the source vertex</param>
        /// <param name="t">the sink vertex</param>
        public FordFulkerson(FlowNetwork g, int s, int t)
        {
            Validate(s, g.V());
            Validate(t, g.V());
            if (s == t)
            {
                throw new ArgumentException("Source equals sink");
            }
            if (!IsFeasible(g, s, t))
            {
                throw new ArgumentException("Initial flow is infeasible");
            }

            // while there exists an augmenting path, use it
            _value = Excess(g, t);
            while (HasAugmentingPath(g, s, t))
            {
                // compute bottleneck capacity
                double bottle = double.PositiveInfinity;
                for (int v = t; v != s; v = _edgeTo[v].Other(v))
                {
                    bottle = Math.Min(bottle, _edgeTo[v].ResidualCapacityTo(v));
                }

                // augment flow
                for (int v = t; v != s; v = _edgeTo[v].Other(v))
                {
                    _edgeTo[v].AddResidualFlowTo(v, bottle);
                }

                _value += bottle;
            }
        }

        /// <summary>
        /// Returns the value of the maximum flow.
        /// </summary>
        /// <returns>the value of the maximum flow</returns>
        public double Value()
        {
            return _value;
        }

        /// <summary>
        /// Is vertex v on the s side of the minimum st-cut?
        /// </summary>
        /// <param name="v"></param>
        /// <returns>true if vertex v is on the s side of the micut, and false if vertex v is on the t side.</returns>
        public bool InCut(int v)
        {
            Validate(v, _marked.Length);
            return _marked[v];
        }

        /// <summary>
        /// Throw an exception if v is outside prescibed range
        /// </summary>
        /// <param name="v"></param>
        /// <param name="vMax"></param>
        private static void Validate(int v, int vMax)
        {
            if (v < 0 || v >= vMax)
            {
                throw new IndexOutOfRangeException("vertex " + v + " is not between 0 and " + (vMax - 1));
            }
        }

        /// <summary>
        /// Is there an augmenting path? 
        /// if so, upon termination edgeTo[] will contain a parent-link representation of such a path
        /// this implementation finds a shortest augmenting path (fewest number of edges),
        /// which performs well both in theory and in practice
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool HasAugmentingPath(FlowNetwork g, int s, int t)
        {
            _edgeTo = new FlowEdge[g.V()];
            _marked = new bool[g.V()];

            // breadth-first search
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(s);
            _marked[s] = true;
            while (!queue.IsEmpty() && !_marked[t])
            {
                int v = queue.Dequeue();

                foreach (FlowEdge e in g.Adj(v))
                {
                    int w = e.Other(v);

                    // if residual capacity from v to w
                    if (e.ResidualCapacityTo(w) > 0)
                    {
                        if (!_marked[w])
                        {
                            _edgeTo[w] = e;
                            _marked[w] = true;
                            queue.Enqueue(w);
                        }
                    }
                }
            }

            // is there an augmenting path?
            return _marked[t];
        }

        /// <summary>
        /// Return excess flow at vertex v
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static double Excess(FlowNetwork g, int v)
        {
            double excess = 0.0;
            foreach (FlowEdge e in g.Adj(v))
            {
                if (v == e.From())
                {
                    excess -= e.Flow();
                }
                else
                {
                    excess += e.Flow();
                }
            }
            return excess;
        }

        /// <summary>
        /// Return excess flow at vertex v
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private bool IsFeasible(FlowNetwork g, int s, int t)
        {
            const double epsilon = 1E-11;

            // check that capacity constraints are satisfied
            for (int v = 0; v < g.V(); v++)
            {
                foreach (FlowEdge e in g.Adj(v))
                {
                    if (e.Flow() < -epsilon || e.Flow() > e.Capacity() + epsilon)
                    {
                        Console.Error.WriteLine("Edge does not satisfy capacity constraints: " + e);
                        return false;
                    }
                }
            }

            // check that net flow into a vertex equals zero, except at source and sink
            if (Math.Abs(_value + Excess(g, s)) > epsilon)
            {
                Console.Error.WriteLine("Excess at source = " + Excess(g, s));
                Console.Error.WriteLine("Max flow         = " + _value);
                return false;
            }
            if (Math.Abs(_value - Excess(g, t)) > epsilon)
            {
                Console.Error.WriteLine("Excess at sink   = " + Excess(g, t));
                Console.Error.WriteLine("Max flow         = " + _value);
                return false;
            }
            for (int v = 0; v < g.V(); v++)
            {
                if (v == s || v == t)
                {
                    continue;
                }
                if (Math.Abs(Excess(g, v)) > epsilon)
                {
                    Console.Error.WriteLine("Net flow out of " + v + " doesn't equal zero");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check optimality conditions
        /// </summary>
        /// <param name="g"></param>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Check(FlowNetwork g, int s, int t)
        {
            // check that flow is feasible
            if (!IsFeasible(g, s, t))
            {
                Console.Error.WriteLine("Flow is infeasible");
                return false;
            }

            // check that s is on the source side of min cut and that t is not on source side
            if (!InCut(s))
            {
                Console.Error.WriteLine("source " + s + " is not on source side of min cut");
                return false;
            }
            if (InCut(t))
            {
                Console.Error.WriteLine("sink " + t + " is on source side of min cut");
                return false;
            }

            // check that value of min cut = value of max flow
            double mincutValue = 0.0;
            for (int v = 0; v < g.V(); v++)
            {
                foreach (FlowEdge e in g.Adj(v))
                {
                    if ((v == e.From()) && InCut(e.From()) && !InCut(e.To()))
                    {
                        mincutValue += e.Capacity();
                    }
                }
            }

            const double epsilon = 1E-11;
            if (Math.Abs(mincutValue - _value) > epsilon)
            {
                Console.Error.WriteLine("Max flow value = " + _value + ", min cut value = " + mincutValue);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Unit tests the FordFulkerson data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // create flow network with vMax vertices and E edges
            int v = int.Parse(args[0]);
            int e = int.Parse(args[1]);
            const int s = 0;
            int t = v - 1;
            FlowNetwork g = new FlowNetwork(v, e);
            StdOut.PrintLn(g);

            // compute maximum flow and minimum cut
            FordFulkerson maxflow = new FordFulkerson(g, s, t);
            StdOut.PrintLn("Max flow from " + s + " to " + t);
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                foreach (FlowEdge edge in g.Adj(vertex))
                {
                    if ((vertex == edge.From()) && edge.Flow() > 0)
                    {
                        StdOut.PrintLn("   " + edge);
                    }
                }
            }

            // print min-cut
            StdOut.Print("Min cut: ");
            for (int vertex = 0; vertex < g.V(); vertex++)
            {
                if (maxflow.InCut(vertex))
                {
                    StdOut.Print(vertex + " ");
                }
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Max flow value = " + maxflow.Value());
        }
    }
}