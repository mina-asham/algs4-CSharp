using System;

namespace algs4.algs4
{
    public class LinearRegression
    {
        private readonly double _alpha;
        private readonly double _beta;
        private readonly double _r2;
        private readonly double _svar0;
        private readonly double _svar1;

        /// <summary>
        /// Performs a linear regression on the data points (y[i], x[i]).
        /// </summary>
        /// <param name="x">the values of the predictor variable</param>
        /// <param name="y">the corresponding values of the response variable</param>
        public LinearRegression(double[] x, double[] y)
        {
            if (x.Length != y.Length)
            {
                throw new ArgumentException("array lengths are not equal");
            }
            int n = x.Length;

            // first pass
            double sumx = 0.0, sumy = 0.0;
            for (int i = 0; i < n; i++)
            {
                sumx += x[i];
            }
            for (int i = 0; i < n; i++)
            {
                sumy += y[i];
            }
            double xbar = sumx / n;
            double ybar = sumy / n;

            // second pass: compute summary statistics
            double xxbar = 0.0, yybar = 0.0, xybar = 0.0;
            for (int i = 0; i < n; i++)
            {
                xxbar += (x[i] - xbar) * (x[i] - xbar);
                yybar += (y[i] - ybar) * (y[i] - ybar);
                xybar += (x[i] - xbar) * (y[i] - ybar);
            }
            _beta = xybar / xxbar;
            _alpha = ybar - _beta * xbar;

            // more statistical analysis
            double rss = 0.0; // residual sum of squares
            double ssr = 0.0; // regression sum of squares
            for (int i = 0; i < n; i++)
            {
                double fit = _beta * x[i] + _alpha;
                rss += (fit - y[i]) * (fit - y[i]);
                ssr += (fit - ybar) * (fit - ybar);
            }

            int degreesOfFreedom = n - 2;
            _r2 = ssr / yybar;
            double svar = rss / degreesOfFreedom;
            _svar1 = svar / xxbar;
            _svar0 = svar / n + xbar * xbar * _svar1;
        }

        /// <summary>
        /// Returns the y-intercept alpha of the best of the best-fit line y = alpha + beta * x.
        /// </summary>
        /// <returns>the y-intercept alpha of the best-fit line y = alpha + beta * x</returns>
        public double Intercept()
        {
            return _alpha;
        }

        /// <summary>
        /// Returns the slope beta * of the best of the best-fit line y = alpha + beta * x.
        /// </summary>
        /// <returns>the slope beta * of the best-fit line y = alpha + beta * x</returns>
        public double Slope()
        {
            return _beta;
        }

        /// <summary>
        /// Returns the coefficient of determination R^2.
        /// </summary>
        /// <returns>the coefficient of determination R^2, which is a real number between 0 and 1</returns>
        public double R2A()
        {
            return _r2;
        }

        /// <summary>
        /// Returns the standard error of the estimate for the intercept.
        /// </summary>
        /// <returns>the standard error of the estimate for the intercept</returns>
        public double InterceptStdErr()
        {
            return Math.Sqrt(_svar0);
        }

        /// <summary>
        /// Returns the standard error of the estimate for the slope.
        /// </summary>
        /// <returns>the standard error of the estimate for the slope</returns>
        public double SlopeStdErr()
        {
            return Math.Sqrt(_svar1);
        }

        /// <summary>
        /// Returns the expected response y given the value of the predictor
        /// variable x.
        /// </summary>
        /// <param name="x">the value of the predictor variable</param>
        /// <returns>the expected response y given the value of the predictor variable x</returns>
        public double Predict(double x)
        {
            return _beta * x + _alpha;
        }

        /// <summary>
        /// Returns a string representation of the simple linear regression model.
        /// </summary>
        /// <returns>a string representation of the simple linear regression model, including the best-fit line and the coefficient of determination R^2</returns>
        public override string ToString()
        {
            string s = "";
            s += string.Format("{0:0.00} N + {1:0.00}", Slope(), Intercept());
            return s + "  (R^2 = " + string.Format("{0:0.000}", R2A()) + ")";
        }
    }
}