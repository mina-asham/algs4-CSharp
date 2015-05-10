using System.Linq;
using algs4.stdlib;

namespace algs4.algs4
{
    public class FarthestPair
    {
        /// <summary>
        /// Farthest pair of points and distance
        /// </summary>
        private readonly Point2D _best1, _best2;

        private readonly double _bestDistance = double.NegativeInfinity;

        public FarthestPair(Point2D[] points)
        {
            GrahamScan graham = new GrahamScan(points);

            // single point
            if (points.Length <= 1)
            {
                return;
            }

            // number of points on the hull
            int pointCount = graham.Hull().Count();

            // the hull, in counterclockwise order
            Point2D[] hull = new Point2D[pointCount + 1];
            int m = 1;
            foreach (Point2D p in graham.Hull())
            {
                hull[m++] = p;
            }

            // all points are equal
            if (pointCount == 1)
            {
                return;
            }

            // points are collinear
            if (pointCount == 2)
            {
                _best1 = hull[1];
                _best2 = hull[2];
                _bestDistance = _best1.DistanceTo(_best2);
                return;
            }

            // k = farthest vertex from edge from hull[1] to hull[M]
            int k = 2;
            while (Point2D.Area2(hull[pointCount], hull[k + 1], hull[1]) > Point2D.Area2(hull[pointCount], hull[k], hull[1]))
            {
                k++;
            }

            int j = k;
            for (int i = 1; i <= k; i++)
            {
                // StdOut.println("hull[i] + " and " + hull[j] + " are antipodal");
                if (hull[i].DistanceTo(hull[j]) > _bestDistance)
                {
                    _best1 = hull[i];
                    _best2 = hull[j];
                    _bestDistance = hull[i].DistanceTo(hull[j]);
                }
                while ((j < pointCount) && Point2D.Area2(hull[i], hull[j + 1], hull[i + 1]) > Point2D.Area2(hull[i], hull[j], hull[i + 1]))
                {
                    j++;
                    // StdOut.println(hull[i] + " and " + hull[j] + " are antipodal");
                    double distance = hull[i].DistanceTo(hull[j]);
                    if (distance > _bestDistance)
                    {
                        _best1 = hull[i];
                        _best2 = hull[j];
                        _bestDistance = hull[i].DistanceTo(hull[j]);
                    }
                }
            }
        }

        public Point2D Either()
        {
            return _best1;
        }

        public Point2D Other()
        {
            return _best2;
        }

        public double Distance()
        {
            return _bestDistance;
        }

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
            FarthestPair farthest = new FarthestPair(points);
            StdOut.PrintLn(farthest.Distance() + " from " + farthest.Either() + " to " + farthest.Other());
        }
    }
}