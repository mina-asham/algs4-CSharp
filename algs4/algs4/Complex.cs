using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Complex
    {
        /// <summary>
        /// The real part
        /// </summary>
        private readonly double _re;

        /// <summary>
        /// The imaginary part
        /// </summary>
        private readonly double _im;

        /// <summary>
        /// Create a new object with the given real and imaginary parts
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imag"></param>
        public Complex(double real, double imag)
        {
            _re = real;
            _im = imag;
        }

        /// <summary>
        /// Return a string representation of the invoking Complex object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Math.Abs(_im) < double.Epsilon)
            {
                return _re + "";
            }
            if (Math.Abs(_re) < double.Epsilon)
            {
                return _im + "i";
            }
            if (_im < 0)
            {
                return _re + " - " + (-_im) + "i";
            }
            return _re + " + " + _im + "i";
        }

        /// <summary>
        /// Return abs/modulus/magnitude and angle/phase/argument
        /// </summary>
        /// <returns>Math.Sqrt(re*re + im*im)</returns>
        public double Abs()
        {
            return Math.Sqrt(_re * _re + _im * _im);
        }

        /// <summary>
        /// Between -pi and pi
        /// </summary>
        /// <returns></returns>
        public double Phase()
        {
            return Math.Atan2(_im, _re);
        }

        /// <summary>
        /// Return a new Complex object whose value is (this + b)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Complex Plus(Complex b)
        {
            Complex a = this; // invoking object
            double real = a._re + b._re;
            double imag = a._im + b._im;
            return new Complex(real, imag);
        }

        /// <summary>
        /// Return a new Complex object whose value is (this - b)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Complex Minus(Complex b)
        {
            Complex a = this;
            double real = a._re - b._re;
            double imag = a._im - b._im;
            return new Complex(real, imag);
        }

        /// <summary>
        /// Return a new Complex object whose value is (this * b)
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Complex Times(Complex b)
        {
            Complex a = this;
            double real = a._re * b._re - a._im * b._im;
            double imag = a._re * b._im + a._im * b._re;
            return new Complex(real, imag);
        }

        /// <summary>
        /// Scalar multiplication
        /// return a new object whose value is (this * alpha)
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public Complex Times(double alpha)
        {
            return new Complex(alpha * _re, alpha * _im);
        }

        /// <summary>
        /// Return a new Complex object whose value is the conjugate of this
        /// </summary>
        /// <returns></returns>
        public Complex Conjugate()
        {
            return new Complex(_re, -_im);
        }

        /// <summary>
        /// Return a new Complex object whose value is the reciprocal of this
        /// </summary>
        /// <returns></returns>
        public Complex Reciprocal()
        {
            double scale = _re * _re + _im * _im;
            return new Complex(_re / scale, -_im / scale);
        }

        /// <summary>
        /// Return the real part
        /// </summary>
        /// <returns></returns>
        public double Re()
        {
            return _re;
        }

        /// <summary>
        /// Return the imaginary part
        /// </summary>
        /// <returns></returns>
        public double Im()
        {
            return _im;
        }

        /// <summary>
        /// Return a / b
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public Complex Divides(Complex b)
        {
            Complex a = this;
            return a.Times(b.Reciprocal());
        }

        /// <summary>
        /// Return a new Complex object whose value is the complex exponential of this
        /// </summary>
        /// <returns></returns>
        public Complex Exp()
        {
            return new Complex(Math.Exp(_re) * Math.Cos(_im), Math.Exp(_re) * Math.Sin(_im));
        }

        /// <summary>
        /// Return a new Complex object whose value is the complex sine of this
        /// </summary>
        /// <returns></returns>
        public Complex Sin()
        {
            return new Complex(Math.Sin(_re) * Math.Cosh(_im), Math.Cos(_re) * Math.Sinh(_im));
        }

        /// <summary>
        /// Return a new Complex object whose value is the complex cosine of this
        /// </summary>
        /// <returns></returns>
        public Complex Cos()
        {
            return new Complex(Math.Cos(_re) * Math.Cosh(_im), -Math.Sin(_re) * Math.Sinh(_im));
        }

        /// <summary>
        /// Return a new Complex object whose value is the complex tangent of this
        /// </summary>
        /// <returns></returns>
        public Complex Tan()
        {
            return Sin().Divides(Cos());
        }

        /// <summary>
        /// A static version of plus
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Complex Plus(Complex a, Complex b)
        {
            double real = a._re + b._re;
            double imag = a._im + b._im;
            Complex sum = new Complex(real, imag);
            return sum;
        }

        /// <summary>
        /// Sample client for testing
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Complex a = new Complex(5.0, 6.0);
            Complex b = new Complex(-3.0, 4.0);

            StdOut.PrintLn("a            = " + a);
            StdOut.PrintLn("b            = " + b);
            StdOut.PrintLn("Re(a)        = " + a.Re());
            StdOut.PrintLn("Im(a)        = " + a.Im());
            StdOut.PrintLn("b + a        = " + b.Plus(a));
            StdOut.PrintLn("a - b        = " + a.Minus(b));
            StdOut.PrintLn("a * b        = " + a.Times(b));
            StdOut.PrintLn("b * a        = " + b.Times(a));
            StdOut.PrintLn("a / b        = " + a.Divides(b));
            StdOut.PrintLn("(a / b) * b  = " + a.Divides(b).Times(b));
            StdOut.PrintLn("conj(a)      = " + a.Conjugate());
            StdOut.PrintLn("|a|          = " + a.Abs());
            StdOut.PrintLn("tan(a)       = " + a.Tan());
        }
    }
}