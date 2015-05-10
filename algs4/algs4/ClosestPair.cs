using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class ClosestPair
    {
        /// <summary>
        /// Closest pair of points and their Euclidean distance
        /// </summary>
        private Point2D _best1, _best2;

        private double _bestDistance = double.PositiveInfinity;

        public ClosestPair(Point2D[] points)
        {
            int n = points.Length;
            if (n <= 1)
            {
                return;
            }

            // sort by x-coordinate (breaking ties by y-coordinate)
            Point2D[] pointsByX = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                pointsByX[i] = points[i];
            }
            Array.Sort(pointsByX, Point2D.XOrderComparer);

            // check for coincident points
            for (int i = 0; i < n - 1; i++)
            {
                if (pointsByX[i].Equals(pointsByX[i + 1]))
                {
                    _bestDistance = 0.0;
                    _best1 = pointsByX[i];
                    _best2 = pointsByX[i + 1];
                    return;
                }
            }

            // sort by y-coordinate (but not yet sorted) 
            Point2D[] pointsByY = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                pointsByY[i] = pointsByX[i];
            }

            // auxiliary array
            Point2D[] aux = new Point2D[n];

            Closest(pointsByX, pointsByY, aux, 0, n - 1);
        }

        /// <summary>
        /// Find closest pair of points in pointsByX[lo..hi]
        /// precondition:  pointsByX[lo..hi] and pointsByY[lo..hi] are the same sequence of points
        /// precondition:  pointsByX[lo..hi] sorted by x-coordinate
        /// postcondition: pointsByY[lo..hi] sorted by y-coordinate
        /// </summary>
        /// <param name="pointsByX"></param>
        /// <param name="pointsByY"></param>
        /// <param name="aux"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        private double Closest(Point2D[] pointsByX, Point2D[] pointsByY, Point2D[] aux, int lo, int hi)
        {
            if (hi <= lo)
            {
                return double.PositiveInfinity;
            }

            int mid = lo + (hi - lo) / 2;
            Point2D median = pointsByX[mid];

            // compute closest pair with both endpoints in left subarray or both in right subarray
            double delta1 = Closest(pointsByX, pointsByY, aux, lo, mid);
            double delta2 = Closest(pointsByX, pointsByY, aux, mid + 1, hi);
            double delta = Math.Min(delta1, delta2);

            // merge back so that pointsByY[lo..hi] are sorted by y-coordinate
            Merge(pointsByY, aux, lo, mid, hi);

            // aux[0..M-1] = sequence of points closer than delta, sorted by y-coordinate
            int m = 0;
            for (int i = lo; i <= hi; i++)
            {
                if (Math.Abs(pointsByY[i].X() - median.X()) < delta)
                {
                    aux[m++] = pointsByY[i];
                }
            }

            // compare each point to its neighbors with y-coordinate closer than delta
            for (int i = 0; i < m; i++)
            {
                // a geometric packing argument shows that this loop iterates at most 7 times
                for (int j = i + 1; (j < m) && (aux[j].Y() - aux[i].Y() < delta); j++)
                {
                    double distance = aux[i].DistanceTo(aux[j]);
                    if (distance < delta)
                    {
                        delta = distance;
                        if (distance < _bestDistance)
                        {
                            _bestDistance = delta;
                            _best1 = aux[i];
                            _best2 = aux[j];
                        }
                    }
                }
            }
            return delta;
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

        /// <summary>
        /// Is v &lt; w ?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        private static bool Less<T>(T v, T w) where T : IComparable<T>
        {
            return (v.CompareTo(w) < 0);
        }

        /// <summary>
        /// Stably merge a[lo .. mid] with a[mid+1 ..hi] using aux[lo .. hi]
        /// precondition: a[lo .. mid] and a[mid+1 .. hi] are sorted subarrays
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="aux"></param>
        /// <param name="lo"></param>
        /// <param name="mid"></param>
        /// <param name="hi"></param>
        private static void Merge<T>(T[] a, T[] aux, int lo, int mid, int hi) where T : IComparable<T>
        {
            // copy to aux[]
            for (int k = lo; k <= hi; k++)
            {
                aux[k] = a[k];
            }

            // merge back to a[] 
            int i = lo, j = mid + 1;
            for (int k = lo; k <= hi; k++)
            {
                if (i > mid)
                {
                    a[k] = aux[j++];
                }
                else if (j > hi)
                {
                    a[k] = aux[i++];
                }
                else if (Less(aux[j], aux[i]))
                {
                    a[k] = aux[j++];
                }
                else
                {
                    a[k] = aux[i++];
                }
            }
        }

        public static void RunMain(string[] args)
        {
            int n = StdIn.ReadInt();
            Point2D[] points = new Point2D[n];
            for (int i = 0; i < n; i++)
            {
                double x = StdIn.ReadDouble();
                double y = StdIn.ReadDouble();
                points[i] = new Point2D(x, y);
            }
            ClosestPair closest = new ClosestPair(points);
            StdOut.PrintLn(closest.Distance() + " from " + closest.Either() + " to " + closest.Other());
        }
    }
}