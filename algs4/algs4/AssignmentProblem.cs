using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class AssignmentProblem
    {
        private const int Unmatched = -1;

        /// <summary>
        /// Number of rows and columns
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// The N-by-N cost matrix
        /// </summary>
        private readonly double[][] _weight;

        /// <summary>
        /// px[i] = dual variable for row i
        /// </summary>
        private readonly double[] _px;

        /// <summary>
        /// py[j] = dual variable for col j
        /// </summary>
        private readonly double[] _py;

        /// <summary>
        /// xy[i] = j means i-j is a match
        /// </summary>
        private readonly int[] _xy;

        /// <summary>
        /// yx[j] = i means i-j is a match
        /// </summary>
        private readonly int[] _yx;

        public AssignmentProblem(double[][] weight)
        {
            _n = weight.Length;
            _weight = new double[_n][];
            for (int i = 0; i < _n; i++)
            {
                weight[i] = new double[_n];
                for (int j = 0; j < _n; j++)
                {
                    _weight[i][j] = weight[i][j];
                }
            }

            // dual variables
            _px = new double[_n];
            _py = new double[_n];

            // initial matching is empty
            _xy = new int[_n];
            _yx = new int[_n];
            for (int i = 0; i < _n; i++)
            {
                _xy[i] = Unmatched;
            }
            for (int j = 0; j < _n; j++)
            {
                _yx[j] = Unmatched;
            }

            // add N edges To matching
            for (int k = 0; k < _n; k++)
            {
                Debug.Assert(IsDualFeasible());
                Debug.Assert(IsComplementarySlack());
                Augment();
            }
            Debug.Assert(Check());
        }

        /// <summary>
        /// Find shortest augmenting path and upate
        /// </summary>
        private void Augment()
        {
            // build residual graph
            EdgeWeightedDigraph g = new EdgeWeightedDigraph(2 * _n + 2);
            int s = 2 * _n, t = 2 * _n + 1;
            for (int i = 0; i < _n; i++)
            {
                if (_xy[i] == Unmatched)
                {
                    g.AddEdge(new DirectedEdge(s, i, 0.0));
                }
            }
            for (int j = 0; j < _n; j++)
            {
                if (_yx[j] == Unmatched)
                {
                    g.AddEdge(new DirectedEdge(_n + j, t, _py[j]));
                }
            }
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    if (_xy[i] == j)
                    {
                        g.AddEdge(new DirectedEdge(_n + j, i, 0.0));
                    }
                    else
                    {
                        g.AddEdge(new DirectedEdge(i, _n + j, Reduced(i, j)));
                    }
                }
            }

            // compute shortest path From s To every other vertex
            DijkstraSP spt = new DijkstraSP(g, s);

            // augment along alternating path
            foreach (DirectedEdge e in spt.PathTo(t))
            {
                int i = e.From(), j = e.To() - _n;
                if (i < _n)
                {
                    _xy[i] = j;
                    _yx[j] = i;
                }
            }

            // update dual variables
            for (int i = 0; i < _n; i++)
            {
                _px[i] += spt.DistTo(i);
            }
            for (int j = 0; j < _n; j++)
            {
                _py[j] += spt.DistTo(_n + j);
            }
        }

        /// <summary>
        /// Reduced cost of i-j
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private double Reduced(int i, int j)
        {
            return _weight[i][j] + _px[i] - _py[j];
        }

        /// <summary>
        /// Dual variable for row i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double DualRow(int i)
        {
            return _px[i];
        }

        /// <summary>
        /// Dual variable for column j
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public double DualCol(int j)
        {
            return _py[j];
        }

        /// <summary>
        /// Total weight of min weight perfect matching
        /// </summary>
        /// <returns></returns>
        public double Weight()
        {
            double total = 0.0;
            for (int i = 0; i < _n; i++)
            {
                if (_xy[i] != Unmatched)
                {
                    total += _weight[i][_xy[i]];
                }
            }
            return total;
        }

        public int Sol(int i)
        {
            return _xy[i];
        }

        /// <summary>
        /// Check that dual variables are feasible
        /// </summary>
        /// <returns></returns>
        private bool IsDualFeasible()
        {
            // check that all edges have >= 0 reduced cost
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    if (Reduced(i, j) < 0)
                    {
                        StdOut.PrintLn("Dual variables are not feasible");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Check that primal and dual variables are complementary slack
        /// </summary>
        /// <returns></returns>
        private bool IsComplementarySlack()
        {
            // check that all matched edges have 0-reduced cost
            for (int i = 0; i < _n; i++)
            {
                if ((_xy[i] != Unmatched) && (Math.Abs(Reduced(i, _xy[i])) > double.Epsilon))
                {
                    StdOut.PrintLn("Primal and dual variables are not complementary slack");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check that primal variables are a perfect matching
        /// </summary>
        /// <returns></returns>
        private bool IsPerfectMatching()
        {
            // check that xy[] is a perfect matching
            bool[] perm = new bool[_n];
            for (int i = 0; i < _n; i++)
            {
                if (perm[_xy[i]])
                {
                    StdOut.PrintLn("Not a perfect matching");
                    return false;
                }
                perm[_xy[i]] = true;
            }

            // check that xy[] and yx[] are inverses
            for (int j = 0; j < _n; j++)
            {
                if (_xy[_yx[j]] != j)
                {
                    StdOut.PrintLn("xy[] and yx[] are not inverses");
                    return false;
                }
            }
            for (int i = 0; i < _n; i++)
            {
                if (_yx[_xy[i]] != i)
                {
                    StdOut.PrintLn("xy[] and yx[] are not inverses");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check optimality conditions
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            return IsPerfectMatching() && IsDualFeasible() && IsComplementarySlack();
        }

        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            int n = input.ReadInt();
            double[][] weight = new double[n][];
            for (int i = 0; i < n; i++)
            {
                weight[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    weight[i][j] = input.ReadDouble();
                }
            }

            AssignmentProblem assignment = new AssignmentProblem(weight);
            StdOut.PrintLn("weight = " + assignment.Weight());
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn(i + "-" + assignment.Sol(i) + "' " + weight[i][assignment.Sol(i)]);
            }

            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn("px[" + i + "] = " + assignment.DualRow(i));
            }
            for (int j = 0; j < n; j++)
            {
                StdOut.PrintLn("py[" + j + "] = " + assignment.DualCol(j));
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    StdOut.PrintLn("reduced[" + i + "-" + j + "] = " + assignment.Reduced(i, j));
                }
            }
        }
    }
}