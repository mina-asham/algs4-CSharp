using System;
using algs4.stdlib;
using DotNetMatrix;

namespace algs4.algs4
{
    public class PolynomialRegression
    {
        /// <summary>
        /// Degree of the polynomial regression
        /// </summary>
        private readonly int _degree;

        /// <summary>
        /// The polynomial regression coefficients
        /// </summary>
        private readonly GeneralMatrix _beta;

        /// <summary>
        /// Sum of squares due to error
        /// </summary>
        private readonly double _sse;

        /// <summary>
        /// Total sum of squares
        /// </summary>
        private readonly double _sst;

        /// <summary>
        /// Performs a polynomial reggression on the data points (y[i], x[i]).
        /// </summary>
        /// <param name="x">the values of the predictor variable</param>
        /// <param name="y">the corresponding values of the response variable</param>
        /// <param name="degree">degree the degree of the polynomial to fit</param>
        public PolynomialRegression(double[] x, double[] y, int degree)
        {
            _degree = degree;
            int n = x.Length;

            // build Vandermonde matrix
            double[][] vandermonde = new double[n][];
            for (int i = 0; i < n; i++)
            {
                vandermonde[i] = new double[degree + 1];
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= degree; j++)
                {
                    vandermonde[i][j] = Math.Pow(x[i], j);
                }
            }
            GeneralMatrix xMatrix = new GeneralMatrix(vandermonde);

            // create matrix from vector
            GeneralMatrix yMatrix = new GeneralMatrix(y, n);

            // find least squares solution
            QRDecomposition qr = new QRDecomposition(xMatrix);
            _beta = qr.Solve(yMatrix);

            // mean of y[] values
            double sum = 0.0;
            for (int i = 0; i < n; i++)
            {
                sum += y[i];
            }
            double mean = sum / n;

            // total variation to be accounted for
            for (int i = 0; i < n; i++)
            {
                double dev = y[i] - mean;
                _sst += dev * dev;
            }

            // variation not accounted for
            GeneralMatrix residuals = xMatrix.Multiply(_beta).Subtract(yMatrix);
            _sse = residuals.Norm2() * residuals.Norm2();
        }

        /// <summary>
        /// Returns the jth regression coefficient
        /// </summary>
        /// <param name="j"></param>
        /// <returns>the jth regression coefficient</returns>
        public double Beta(int j)
        {
            return _beta.GetElement(j, 0);
        }

        /// <summary>
        /// Returns the degree of the polynomial to fit
        /// </summary>
        /// <returns>the degree of the polynomial to fit</returns>
        public int Degree()
        {
            return _degree;
        }

        /// <summary>
        /// Returns the coefficient of determination R<sup>2</sup>.
        /// </summary>
        /// <returns>the coefficient of determination R<sup>2</sup>, which is a real number between 0 and 1</returns>
        public double R2()
        {
            if (Math.Abs(_sst) < double.Epsilon)
            {
                return 1.0; // constant function
            }
            return 1.0 - _sse / _sst;
        }

        /// <summary>
        /// Returns the expected response y given the value of the predictor
        /// variable x.
        /// </summary>
        /// <param name="x">the value of the predictor variable</param>
        /// <returns>the expected response y given the value of the predictor variable x</returns>
        public double Predict(double x)
        {
            // horner's method
            double y = 0.0;
            for (int j = _degree; j >= 0; j--)
            {
                y = Beta(j) + (x * y);
            }
            return y;
        }

        /// <summary>
        /// Returns a string representation of the polynomial regression model.
        /// </summary>
        /// <returns>a string representation of the polynomial regression model, including the best-fit polynomial and the coefficient of determination R^2</returns>
        public override string ToString()
        {
            string s = "";
            int j = _degree;

            // ignoring leading zero coefficients
            while (j >= 0 && Math.Abs(Beta(j)) < 1E-5)
            {
                j--;
            }

            // create remaining terms
            for (; j >= 0; j--)
            {
                if (j == 0)
                {
                    s += string.Format("{0:0.00} ", Beta(j));
                }
                else if (j == 1)
                {
                    s += string.Format("{0:0.00} N + ", Beta(j));
                }
                else
                {
                    s += string.Format("{0:0.00} N^{1} + ", Beta(j), j);
                }
            }
            return s + "  (R^2 = " + string.Format("{0:0.000}", R2()) + ")";
        }

        public static void RunMain(string[] args)
        {
            double[] x = { 10, 20, 40, 80, 160, 200 };
            double[] y = { 100, 350, 1500, 6700, 20160, 40000 };
            PolynomialRegression regression = new PolynomialRegression(x, y, 3);
            StdOut.PrintLn(regression);
        }
    }
}