using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Vector
    {

        /// <summary>
        /// Length of the vector
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// Array of vector's components
        /// </summary>
        private readonly double[] _data;

        /// <summary>
        /// Initializes a d-dimensional zero vector.
        /// </summary>
        /// <param name="d">the dimension of the vector</param>
        public Vector(int d)
        {
            _n = d;
            _data = new double[_n];
        }

        /// <summary>
        /// Initializes a vector from either an array or a vararg list.
        /// The vararg syntax supports a constructor that takes a variable number of
        /// arugments such as Vector x = new Vector(1.0, 2.0, 3.0, 4.0).
        /// </summary>
        /// <param name="a">the array or vararg list</param>
        public Vector(params double[] a)
        {
            _n = a.Length;

            // defensive copy so that client can't alter our copy of data[]
            _data = new double[_n];
            for (int i = 0; i < _n; i++)
            {
                _data[i] = a[i];
            }
        }

        /// <summary>
        /// Returns the length of this vector.
        /// </summary>
        /// <returns>the dimension of this vector</returns>
        public int Length()
        {
            return _n;
        }

        /// <summary>
        /// Returns the inner product of this vector with that vector.
        /// </summary>
        /// <param name="that">the other vector</param>
        /// <returns>the dot product between this vector and that vector</returns>
        public double Dot(Vector that)
        {
            if (_n != that._n)
            {
                throw new ArgumentException("Dimensions don't agree");
            }
            double sum = 0.0;
            for (int i = 0; i < _n; i++)
            {
                sum = sum + (_data[i] * that._data[i]);
            }
            return sum;
        }

        /// <summary>
        /// Returns the Euclidean norm of this vector.
        /// </summary>
        /// <returns>the Euclidean norm of this vector</returns>
        public double Magnitude()
        {
            return Math.Sqrt(Dot(this));
        }

        /// <summary>
        /// Returns the Euclidean distance between this vector and that vector.
        /// </summary>
        /// <param name="that">the other vector</param>
        /// <returns>the Euclidean distance between this vector and that vector</returns>
        public double DistanceTo(Vector that)
        {
            if (_n != that._n)
            {
                throw new ArgumentException("Dimensions don't agree");
            }
            return Minus(that).Magnitude();
        }

        /// <summary>
        /// Returns the sum of this vector and that vector: this + that.
        /// </summary>
        /// <param name="that">the vector to add to this vector</param>
        /// <returns>the sum of this vector and that vector</returns>
        public Vector Plus(Vector that)
        {
            if (_n != that._n)
            {
                throw new ArgumentException("Dimensions don't agree");
            }
            Vector c = new Vector(_n);
            for (int i = 0; i < _n; i++)
            {
                c._data[i] = _data[i] + that._data[i];
            }
            return c;
        }

        /// <summary>
        /// Returns the difference between this vector and that vector: this - that.
        /// </summary>
        /// <param name="that">the vector to subtract from this vector</param>
        /// <returns>the difference between this vector and that vector</returns>
        public Vector Minus(Vector that)
        {
            if (_n != that._n)
            {
                throw new ArgumentException("Dimensions don't agree");
            }
            Vector c = new Vector(_n);
            for (int i = 0; i < _n; i++)
            {
                c._data[i] = _data[i] - that._data[i];
            }
            return c;
        }

        /// <summary>
        /// Returns the ith cartesian coordinate.
        /// </summary>
        /// <param name="i">the coordinate index</param>
        /// <returns>the ith cartesian coordinate</returns>
        public double Cartesian(int i)
        {
            return _data[i];
        }

        /// <summary>
        /// Returns the product of this factor multiplied by the scalar factor: this * factor.
        /// </summary>
        /// <param name="factor">the multiplier</param>
        /// <returns>the scalar product of this vector and factor</returns>
        public Vector Times(double factor)
        {
            Vector c = new Vector(_n);
            for (int i = 0; i < _n; i++)
            {
                c._data[i] = factor * _data[i];
            }
            return c;
        }

        /// <summary>
        /// Returns a unit vector in the direction of this vector.
        /// </summary>
        /// <returns>a unit vector in the direction of this vector</returns>
        public Vector Direction()
        {
            if (Math.Abs(Magnitude()) < double.Epsilon)
            {
                throw new ArithmeticException("Zero-vector has no direction");
            }
            return Times(1.0 / Magnitude());
        }

        /// <summary>
        /// Returns a string representation of this vector.
        /// </summary>
        /// <returns>a string representation of this vector, which consists of the vector entries, separates by single spaces</returns>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _n; i++)
            {
                s = s + _data[i] + " ";
            }
            return s;
        }

        /// <summary>
        /// Unit tests the data type methods.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            double[] xdata = { 1.0, 2.0, 3.0, 4.0 };
            double[] ydata = { 5.0, 2.0, 4.0, 1.0 };
            Vector x = new Vector(xdata);
            Vector y = new Vector(ydata);

            StdOut.PrintLn("   x       = " + x);
            StdOut.PrintLn("   y       = " + y);

            Vector z = x.Plus(y);
            StdOut.PrintLn("   z       = " + z);

            z = z.Times(10.0);
            StdOut.PrintLn(" 10z       = " + z);

            StdOut.PrintLn("  |x|      = " + x.Magnitude());
            StdOut.PrintLn(" <x, y>    = " + x.Dot(y));
            StdOut.PrintLn("dist(x, y) = " + x.DistanceTo(y));
            StdOut.PrintLn("dir(x)     = " + x.Direction());
        }
    }
}
