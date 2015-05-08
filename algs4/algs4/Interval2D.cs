using algs4.stdlib;

namespace algs4.algs4
{
    public class Interval2D
    {
        private readonly Interval1D _x;
        private readonly Interval1D _y;

        /// <summary>
        /// Initializes a two-dimensional interval.
        /// </summary>
        /// <param name="x">the one-dimensional interval of x-coordinates</param>
        /// <param name="y">the one-dimensional interval of y-coordinates</param>
        public Interval2D(Interval1D x, Interval1D y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Does this two-dimensional interval intersect that two-dimensional interval?
        /// </summary>
        /// <param name="that">the other two-dimensional interval</param>
        /// <returns>true if this two-dimensional interval intersects that two-dimensional interval; false otherwise</returns>
        public bool Intersects(Interval2D that)
        {
            if (!_x.Intersects(that._x))
            {
                return false;
            }
            return _y.Intersects(that._y);
        }

        /// <summary>
        /// Does this two-dimensional interval contain the point p?
        /// </summary>
        /// <param name="p">the two-dimensional point</param>
        /// <returns>true if this two-dimensional interval contains the point p; false otherwise</returns>
        public bool Contains(Point2D p)
        {
            return _x.Contains(p.X()) && _y.Contains(p.Y());
        }

        /// <summary>
        /// Returns the area of this two-dimensional interval.
        /// </summary>
        /// <returns>the area of this two-dimensional interval</returns>
        public double Area()
        {
            return _x.Length() * _y.Length();
        }

        /// <summary>
        /// Returns a string representation of this two-dimensional interval.
        /// </summary>
        /// <returns>a string representation of this two-dimensional interval in the form [xleft, xright] x [yleft, yright]</returns>
        public override string ToString()
        {
            return _x + " x " + _y;
        }

        /// <summary>
        /// Draws this two-dimensional interval to standard draw.
        /// </summary>
        public void Draw()
        {
            float xc = (float)((_x.Left() + _x.Right()) / 2.0);
            float yc = (float)((_y.Left() + _y.Right()) / 2.0);
            StdDraw.Rectangle(xc, yc, (float)(_x.Length() / 2.0), (float)(_y.Length() / 2.0));
        }

        /// <summary>
        /// Unit tests the Interval2D data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            double xlo = double.Parse(args[0]);
            double xhi = double.Parse(args[1]);
            double ylo = double.Parse(args[2]);
            double yhi = double.Parse(args[3]);
            int T = int.Parse(args[4]);

            Interval1D xinterval = new Interval1D(xlo, xhi);
            Interval1D yinterval = new Interval1D(ylo, yhi);
            Interval2D box = new Interval2D(xinterval, yinterval);
            box.Draw();

            Counter counter = new Counter("hits");
            for (int t = 0; t < T; t++)
            {
                double x = StdRandom.Uniform(0.0, 1.0);
                double y = StdRandom.Uniform(0.0, 1.0);
                Point2D p = new Point2D(x, y);

                if (box.Contains(p))
                {
                    counter.Increment();
                }
                else
                {
                    p.Draw();
                }
            }

            StdOut.PrintLn(counter);
            StdOut.PrintF("box area = %.2f\n", box.Area());
        }
    }
}