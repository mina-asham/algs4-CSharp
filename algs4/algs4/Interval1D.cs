using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Interval1D
    {
        /// <summary>
        /// Compares two intervals by left endpoint.
        /// </summary>
        public static readonly IComparer<Interval1D> LeftEndpointOrder = new LeftComparator();

        /// <summary>
        /// Compares two intervals by right endpoint.
        /// </summary>
        public static readonly IComparer<Interval1D> RightEndpointOrder = new RightComparator();

        /// <summary>
        /// Compares two intervals by length.
        /// </summary>
        public static readonly IComparer<Interval1D> LengthOrder = new LengthComparator();

        private readonly double _left;
        private readonly double _right;

        /// <summary>
        /// Initializes an interval [left, right].
        /// </summary>
        /// <param name="left">the left endpoint</param>
        /// <param name="right">the right endpoint</param>
        public Interval1D(double left, double right)
        {
            if (double.IsInfinity(left) || double.IsInfinity(right))
            {
                throw new ArgumentException("Endpoints must be finite");
            }
            if (double.IsNaN(left) || double.IsNaN(right))
            {
                throw new ArgumentException("Endpoints cannot be NaN");
            }

            if (left <= right)
            {
                _left = left;
                _right = right;
            }
            else
            {
                throw new ArgumentException("Illegal interval");
            }
        }

        /// <summary>
        /// Returns the left endpoint.
        /// </summary>
        /// <returns>the left endpoint</returns>
        public double Left()
        {
            return _left;
        }

        /// <summary>
        /// Returns the right endpoint.
        /// </summary>
        /// <returns>the right endpoint</returns>
        public double Right()
        {
            return _right;
        }

        /// <summary>
        /// Does this interval intersect that interval?
        /// </summary>
        /// <param name="that">the other interval</param>
        /// <returns>true if this interval intersects that interval; false otherwise</returns>
        public bool Intersects(Interval1D that)
        {
            if (_right < that._left)
            {
                return false;
            }
            return that._right >= _left;
        }

        /// <summary>
        /// Does this interval contain the value x?
        /// </summary>
        /// <param name="x">the value</param>
        /// <returns>true if this interval contains the value x; false otherwise</returns>
        public bool Contains(double x)
        {
            return (_left <= x) && (x <= _right);
        }

        /// <summary>
        /// Returns the length of this interval.
        /// </summary>
        /// <returns>the length of this interval (right - left)</returns>
        public double Length()
        {
            return _right - _left;
        }

        /// <summary>
        /// Returns a string representation of this interval.
        /// </summary>
        /// <returns>a string representation of this interval in the form [left, right]</returns>
        public override string ToString()
        {
            return "[" + _left + ", " + _right + "]";
        }

        /// <summary>
        /// Ascending order of left endpoint, breaking ties by right endpoint
        /// </summary>
        private class LeftComparator : IComparer<Interval1D>
        {
            public int Compare(Interval1D a, Interval1D b)
            {
                if (a._left < b._left)
                {
                    return -1;
                }
                if (a._left > b._left)
                {
                    return +1;
                }
                if (a._right < b._right)
                {
                    return -1;
                }
                if (a._right > b._right)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Ascending order of right endpoint, breaking ties by left endpoint
        /// </summary>
        private class RightComparator : IComparer<Interval1D>
        {
            public int Compare(Interval1D a, Interval1D b)
            {
                if (a._right < b._right)
                {
                    return -1;
                }
                if (a._right > b._right)
                {
                    return +1;
                }
                if (a._left < b._left)
                {
                    return -1;
                }
                if (a._left > b._left)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Ascending order of length
        /// </summary>
        private class LengthComparator : IComparer<Interval1D>
        {
            public int Compare(Interval1D a, Interval1D b)
            {
                double alen = a.Length();
                double blen = b.Length();
                if (alen < blen)
                {
                    return -1;
                }
                if (alen > blen)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Unit tests the Interval1D data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Interval1D[] intervals = new Interval1D[4];
            intervals[0] = new Interval1D(15.0, 33.0);
            intervals[1] = new Interval1D(45.0, 60.0);
            intervals[2] = new Interval1D(20.0, 70.0);
            intervals[3] = new Interval1D(46.0, 55.0);

            StdOut.PrintLn("Unsorted");
            for (int i = 0; i < intervals.Length; i++)
            {
                StdOut.PrintLn(intervals[i]);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Sort by left endpoint");
            Array.Sort(intervals, LeftEndpointOrder);
            for (int i = 0; i < intervals.Length; i++)
            {
                StdOut.PrintLn(intervals[i]);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Sort by right endpoint");
            Array.Sort(intervals, RightEndpointOrder);
            for (int i = 0; i < intervals.Length; i++)
            {
                StdOut.PrintLn(intervals[i]);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Sort by length");
            Array.Sort(intervals, LengthOrder);
            for (int i = 0; i < intervals.Length; i++)
            {
                StdOut.PrintLn(intervals[i]);
            }
            StdOut.PrintLn();
        }
    }
}