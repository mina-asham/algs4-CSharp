using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Point2D : IComparable<Point2D>
    {
        /// <summary>
        /// Compares two points by x-coordinate.
        /// </summary>
        public static readonly IComparer<Point2D> XOrderComparer = new XOrder();

        /// <summary>
        /// Compares two points by y-coordinate.
        /// </summary>
        public static readonly IComparer<Point2D> YOrderComparer = new YOrder();

        /// <summary>
        /// Compares two points by polar radius.
        /// </summary>
        public static readonly IComparer<Point2D> ROrderComparer = new ROrder();

        /// <summary>
        /// Compares two points by polar angle (between 0 and 2pi) with respect to this point.
        /// </summary>
        public readonly IComparer<Point2D> PolarOrderComparer;

        /// <summary>
        /// Compares two points by atan2() angle (between -pi and pi) with respect to this point.
        /// </summary>
        public readonly IComparer<Point2D> Atan2OrderComparer;

        /// <summary>
        /// Compares two points by distance to this point.
        /// </summary>
        public readonly IComparer<Point2D> DistanceToOrderComparer;

        /// <summary>
        /// X coordinate
        /// </summary>
        private readonly double _x;

        /// <summary>
        /// Y coordinate
        /// </summary>
        private readonly double _y;

        /// <summary>
        /// Initializes a new point (x, y).
        /// </summary>
        /// <param name="x">the x-coordinate</param>
        /// <param name="y">the y-coordinate</param>
        public Point2D(double x, double y)
        {
            if (double.IsInfinity(x) || double.IsInfinity(y))
            {
                throw new ArgumentException("Coordinates must be finite");
            }
            if (double.IsNaN(x) || double.IsNaN(y))
            {
                throw new ArgumentException("Coordinates cannot be NaN");
            }
            _x = x;
            _y = y;

            PolarOrderComparer = new PolarOrder(this);
            Atan2OrderComparer = new Atan2Order(this);
            DistanceToOrderComparer = new DistanceToOrder(this);
        }

        /// <summary>
        /// Returns the x-coordinate.
        /// </summary>
        /// <returns>the x-coordinate</returns>
        public double X()
        {
            return _x;
        }

        /// <summary>
        /// Returns the y-coordinate.
        /// </summary>
        /// <returns>the y-coordinate</returns>
        public double Y()
        {
            return _y;
        }

        /// <summary>
        /// Returns the polar radius of this point.
        /// </summary>
        /// <returns>the polar radius of this point in polar coordiantes: sqrt(x*x + y*y)</returns>
        public double R()
        {
            return Math.Sqrt(_x * _x + _y * _y);
        }

        /// <summary>
        /// Returns the angle of this point in polar coordinates.
        /// </summary>
        /// <returns>the angle (in radians) of this point in polar coordiantes (between -pi/2 and pi/2)</returns>
        public double Theta()
        {
            return Math.Atan2(_y, _x);
        }

        /// <summary>
        /// Returns the angle between this point and that point.
        /// </summary>
        /// <param name="that"></param>
        /// <returns>the angle in radians (between -pi and pi) between this point and that point (0 if equal)</returns>
        private double AngleTo(Point2D that)
        {
            double dx = that._x - _x;
            double dy = that._y - _y;
            return Math.Atan2(dy, dx);
        }

        /// <summary>
        /// Is a-&gt;b-&gt;c a counterclockwise turn?
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <param name="c">third point</param>
        /// <returns>{ -1, 0, +1 } if a-&gt;b-&gt;c is a { clockwise, collinear; counterclocwise } turn.</returns>
        public static int CCW(Point2D a, Point2D b, Point2D c)
        {
            double area2 = (b._x - a._x) * (c._y - a._y) - (b._y - a._y) * (c._x - a._x);
            if (area2 < 0)
            {
                return -1;
            }
            if (area2 > 0)
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Returns twice the signed area of the triangle a-b-c.
        /// </summary>
        /// <param name="a">first point</param>
        /// <param name="b">second point</param>
        /// <param name="c">third point</param>
        /// <returns>twice the signed area of the triangle a-b-c</returns>
        public static double Area2(Point2D a, Point2D b, Point2D c)
        {
            return (b._x - a._x) * (c._y - a._y) - (b._y - a._y) * (c._x - a._x);
        }

        /// <summary>
        /// Returns the Euclidean distance between this point and that point.
        /// </summary>
        /// <param name="that">the other point</param>
        /// <returns>the Euclidean distance between this point and that point</returns>
        public double DistanceTo(Point2D that)
        {
            double dx = _x - that._x;
            double dy = _y - that._y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Returns the square of the Euclidean distance between this point and that point.
        /// </summary>
        /// <param name="that">the other point</param>
        /// <returns>the square of the Euclidean distance between this point and that point</returns>
        public double DistanceSquaredTo(Point2D that)
        {
            double dx = _x - that._x;
            double dy = _y - that._y;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Compares this point to that point by y-coordinate, breaking ties by x-coordinate.
        /// </summary>
        /// <param name="that">the other point</param>
        /// <returns>{ a negative integer, zero, a positive integer } if this point is { less than, equal to, greater than } that point</returns>
        public int CompareTo(Point2D that)
        {
            if (_y < that._y)
            {
                return -1;
            }
            if (_y > that._y)
            {
                return +1;
            }
            if (_x < that._x)
            {
                return -1;
            }
            if (_x > that._x)
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Compare points according to their x-coordinate
        /// </summary>
        private class XOrder : IComparer<Point2D>
        {
            public int Compare(Point2D p, Point2D q)
            {
                if (p._x < q._x)
                {
                    return -1;
                }
                if (p._x > q._x)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Compare points according to their y-coordinate
        /// </summary>
        private class YOrder : IComparer<Point2D>
        {
            public int Compare(Point2D p, Point2D q)
            {
                if (p._y < q._y)
                {
                    return -1;
                }
                if (p._y > q._y)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Compare points according to their polar radius
        /// </summary>
        private class ROrder : IComparer<Point2D>
        {
            public int Compare(Point2D p, Point2D q)
            {
                double delta = (p._x * p._x + p._y * p._y) - (q._x * q._x + q._y * q._y);
                if (delta < 0)
                {
                    return -1;
                }
                if (delta > 0)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Compare other points relative to atan2 angle (bewteen -pi/2 and pi/2) they make with this Point
        /// </summary>
        private class Atan2Order : IComparer<Point2D>
        {
            private readonly Point2D _point;

            public Atan2Order(Point2D point)
            {
                _point = point;
            }

            public int Compare(Point2D q1, Point2D q2)
            {
                double angle1 = _point.AngleTo(q1);
                double angle2 = _point.AngleTo(q2);
                if (angle1 < angle2)
                {
                    return -1;
                }
                if (angle1 > angle2)
                {
                    return +1;
                }
                return 0;
            }
        }

        // compare other points relative to polar angle (between 0 and 2pi) they make with this Point
        private class PolarOrder : IComparer<Point2D>
        {
            private readonly Point2D _point;

            public PolarOrder(Point2D point)
            {
                _point = point;
            }

            public int Compare(Point2D q1, Point2D q2)
            {
                double dx1 = q1._x - _point._x;
                double dy1 = q1._y - _point._y;
                double dx2 = q2._x - _point._x;
                double dy2 = q2._y - _point._y;

                // q1 above; q2 below
                if (dy1 >= 0 && dy2 < 0)
                {
                    return -1;
                }

                // q1 below; q2 above
                if (dy2 >= 0 && dy1 < 0)
                {
                    return +1;
                }

                if (Math.Abs(dy1) < double.Epsilon && Math.Abs(dy2) < double.Epsilon)
                {
                    // 3-collinear and horizontal
                    if (dx1 >= 0 && dx2 < 0)
                    {
                        return -1;
                    }
                    if (dx2 >= 0 && dx1 < 0)
                    {
                        return +1;
                    }
                    return 0;
                }
                return -CCW(_point, q1, q2); // both above or below

                // Note: ccw() recomputes dx1, dy1, dx2, and dy2
            }
        }

        /// <summary>
        /// Compare points according to their distance to this point
        /// </summary>
        private class DistanceToOrder : IComparer<Point2D>
        {
            private readonly Point2D _point;

            public DistanceToOrder(Point2D point)
            {
                _point = point;
            }

            public int Compare(Point2D p, Point2D q)
            {
                double dist1 = _point.DistanceSquaredTo(p);
                double dist2 = _point.DistanceSquaredTo(q);
                if (dist1 < dist2)
                {
                    return -1;
                }
                if (dist1 > dist2)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Does this point equal y?
        /// </summary>
        /// <param name="other">the other point</param>
        /// <returns>true if this point equals the other point; false otherwise</returns>
        public override bool Equals(Object other)
        {
            if (other == this)
            {
                return true;
            }
            if (other == null)
            {
                return false;
            }
            if (other.GetType() != GetType())
            {
                return false;
            }
            Point2D that = (Point2D)other;
            return Math.Abs(_x - that._x) < double.Epsilon && Math.Abs(_y - that._y) < double.Epsilon;
        }

        /// <summary>
        /// Return a string representation of this point.
        /// </summary>
        /// <returns>a string representation of this point in the format (x, y)</returns>
        public override String ToString()
        {
            return "(" + _x + ", " + _y + ")";
        }

        /// <summary>
        /// Returns an integer hash code for this point.
        /// </summary>
        /// <returns>an integer hash code for this point</returns>
        public override int GetHashCode()
        {
            int hashX = _x.GetHashCode();
            int hashY = _y.GetHashCode();
            return 31 * hashX + hashY;
        }

        /// <summary>
        /// Plot this point using standard draw.
        /// </summary>
        public void Draw()
        {
            StdDraw.Point((float)_x, (float)_y);
        }

        /// <summary>
        /// Plot a line from this point to that point using standard draw.
        /// </summary>
        /// <param name="that">the other point</param>
        public void DrawTo(Point2D that)
        {
            StdDraw.Line((float)_x, (float)_y, (float)that._x, (float)that._y);
        }

        /// <summary>
        /// Unit tests the point data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(String[] args)
        {
            int x0 = int.Parse(args[0]);
            int y0 = int.Parse(args[1]);
            int n = int.Parse(args[2]);

            StdDraw.SetCanvasSize(800, 800);
            StdDraw.SetXscale(0, 100);
            StdDraw.SetYscale(0, 100);
            StdDraw.SetPenRadius(.005f);
            Point2D[] points = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                int x = StdRandom.Uniform(100);
                int y = StdRandom.Uniform(100);
                points[i] = new Point2D(x, y);
                points[i].Draw();
            }

            // draw p = (x0, x1) in red
            Point2D p = new Point2D(x0, y0);
            StdDraw.SetPenColor(StdDraw.Red);
            StdDraw.SetPenRadius(.02f);
            p.Draw();

            // draw line segments from p to each point, one at a time, in polar order
            StdDraw.SetPenRadius();
            StdDraw.SetPenColor(StdDraw.Blue);
            Array.Sort(points, p.PolarOrderComparer);
            for (int i = 0; i < n; i++)
            {
                p.DrawTo(points[i]);
                StdDraw.Show(100);
            }
        }
    }
}