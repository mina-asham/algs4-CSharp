using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class GrahamScan
    {
        private readonly Stack<Point2D> _hull = new Stack<Point2D>();

        public GrahamScan(Point2D[] pts)
        {
            // defensive copy
            int n = pts.Length;
            Point2D[] points = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                points[i] = pts[i];
            }

            // preprocess so that points[0] has lowest y-coordinate; break ties by x-coordinate
            // points[0] is an extreme point of the convex hull
            // (alternatively, could do easily in linear time)
            Array.Sort(points);

            // sort by polar angle with respect to base point points[0],
            // breaking ties by distance to points[0]
            Array.Sort(points, 1, n, points[0].PolarOrderComparer);

            _hull.Push(points[0]); // p[0] is first extreme point

            // find index k1 of first point not equal to points[0]
            int k1;
            for (k1 = 1; k1 < n; k1++)
            {
                if (!points[0].Equals(points[k1]))
                {
                    break;
                }
            }
            if (k1 == n)
            {
                return; // all points equal
            }

            // find index k2 of first point not collinear with points[0] and points[k1]
            int k2;
            for (k2 = k1 + 1; k2 < n; k2++)
            {
                if (Point2D.CCW(points[0], points[k1], points[k2]) != 0)
                {
                    break;
                }
            }
            _hull.Push(points[k2 - 1]); // points[k2-1] is second extreme point

            // Graham scan; note that points[N-1] is extreme point different from points[0]
            for (int i = k2; i < n; i++)
            {
                Point2D top = _hull.Pop();
                while (Point2D.CCW(_hull.Peek(), top, points[i]) <= 0)
                {
                    top = _hull.Pop();
                }
                _hull.Push(top);
                _hull.Push(points[i]);
            }

            Debug.Assert(IsConvex());
        }

        /// <summary>
        /// Return extreme points on convex hull in counterclockwise order as an IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Point2D> Hull()
        {
            Stack<Point2D> s = new Stack<Point2D>();
            foreach (Point2D p in _hull)
            {
                s.Push(p);
            }
            return s;
        }

        /// <summary>
        /// Check that boundary of hull is strictly convex
        /// </summary>
        /// <returns></returns>
        private bool IsConvex()
        {
            int n = _hull.Size();
            if (n <= 2)
            {
                return true;
            }

            Point2D[] points = new Point2D[n];
            int index = 0;
            foreach (Point2D p in Hull())
            {
                points[index++] = p;
            }

            for (int i = 0; i < n; i++)
            {
                if (Point2D.CCW(points[i], points[(i + 1) % n], points[(i + 2) % n]) <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int n = StdIn.ReadInt();
            Point2D[] points = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                int x = StdIn.ReadInt();
                int y = StdIn.ReadInt();
                points[i] = new Point2D(x, y);
            }
            GrahamScan graham = new GrahamScan(points);
            foreach (Point2D p in graham.Hull())
            {
                StdOut.PrintLn(p);
            }
        }
    }
}