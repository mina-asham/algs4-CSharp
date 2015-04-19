using System;

namespace algs4.stdlib
{
    public static class StdRandom
    {
        /// <summary>
        /// pseudo-random number generator
        /// </summary>
        private static Random _random;

        /// <summary>
        /// pseudo-random number generator seed
        /// </summary>
        private static int _seed;

        /// <summary>
        /// static initializer
        /// </summary>
        static StdRandom()
        {
            _seed = Environment.TickCount;
            _random = new Random(_seed);
        }

        /// <summary>
        /// Sets the seed of the psedurandom number generator.
        /// </summary>
        /// <param name="s"></param>
        public static void SetSeed(int s)
        {
            _seed = s;
            _random = new Random(_seed);
        }

        /// <summary>
        /// Returns the seed of the psedurandom number generator.
        /// </summary>
        /// <returns></returns>
        public static long GetSeed()
        {
            return _seed;
        }

        /// <summary>
        /// Return real number uniformly in [0, 1).
        /// </summary>
        /// <returns></returns>
        public static double Uniform()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// Returns an integer uniformly between 0 (inclusive) and N (exclusive).
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int Uniform(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("Parameter N must be positive");
            }
            return _random.Next(n);
        }

        ///////////////////////////////////////////////////////////////////////////
        //  STATIC METHODS BELOW RELY ON SYSTEM.RANDOM ONLY INDIRECTLY VIA
        //  THE STATIC METHODS ABOVE.
        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns a real number uniformly in [0, 1).
        /// </summary>
        /// <returns></returns>
        [Obsolete("clearer to use Uniform()")]
        public static double Random()
        {
            return Uniform();
        }

        /// <summary>
        /// Returns an integer uniformly in [a, b).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Uniform(int a, int b)
        {
            if (b <= a)
            {
                throw new ArgumentException("Invalid range");
            }
            if ((long)b - a >= int.MaxValue)
            {
                throw new ArgumentException("Invalid range");
            }
            return a + Uniform(b - a);
        }

        /// <summary>
        /// Returns a real number uniformly in [a, b).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Uniform(double a, double b)
        {
            if (!(a < b))
            {
                throw new ArgumentException("Invalid range");
            }
            return a + Uniform() * (b - a);
        }

        /// <summary>
        /// Returns a boolean, which is true with probability p, and false otherwise.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool Bernoulli(double p)
        {
            if (!(p >= 0.0 && p <= 1.0))
            {
                throw new ArgumentException("Probability must be between 0.0 and 1.0");
            }
            return Uniform() < p;
        }

        /// <summary>
        /// Returns a boolean, which is true with probability .5, and false otherwise.
        /// Returns a boolean, which is true with probability .5, and false otherwise.
        /// </summary>
        /// <returns></returns>
        public static bool Bernoulli()
        {
            return Bernoulli(0.5);
        }

        /// <summary>
        /// Returns a real number with a standard Gaussian distribution.
        /// </summary>
        /// <returns></returns>
        public static double Gaussian()
        {
            // use the polar form of the Box-Muller transform
            double r, x, y;
            do
            {
                x = Uniform(-1.0, 1.0);
                y = Uniform(-1.0, 1.0);
                r = x * x + y * y;
            } while (r >= 1 || Math.Abs(r) < double.Epsilon);

            // Remark:  y * Math.sqrt(-2 * Math.log(r) / r)
            // is an independent random gaussian
            return x * Math.Sqrt(-2 * Math.Log(r) / r);
        }

        /// <summary>
        /// Returns a real number from a gaussian distribution with given mean and StdDev
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="stddev"></param>
        /// <returns></returns>
        public static double Gaussian(double mean, double stddev)
        {
            return mean + stddev * Gaussian();
        }

        /// <summary>
        /// Returns an integer with a geometric distribution with mean 1/p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int Geometric(double p)
        {
            if (!(p >= 0.0 && p <= 1.0))
            {
                throw new ArgumentException("Probability must be between 0.0 and 1.0");
            }

            // using algorithm given by Knuth
            return (int)Math.Ceiling(Math.Log(Uniform()) / Math.Log(1.0 - p));
        }

        /// <summary>
        /// Return an integer with a Poisson distribution with mean lambda.
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static int Poisson(double lambda)
        {
            if (!(lambda > 0.0))
            {
                throw new ArgumentException("Parameter lambda must be positive");
            }
            if (double.IsInfinity(lambda))
            {
                throw new ArgumentException("Parameter lambda must not be infinite");
            }

            // using algorithm given by Knuth
            // see http://en.wikipedia.org/wiki/Poisson_distribution
            int k = 0;
            double p = 1.0;
            double capitalL = Math.Exp(-lambda);
            do
            {
                k++;
                p *= Uniform();
            } while (p >= capitalL);
            return k - 1;
        }

        /// <summary>
        /// Returns a real number with a Pareto distribution with parameter alpha.
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static double Pareto(double alpha)
        {
            if (!(alpha > 0.0))
            {
                throw new ArgumentException("Shape parameter alpha must be positive");
            }
            return Math.Pow(1 - Uniform(), -1.0 / alpha) - 1.0;
        }

        /// <summary>
        /// Returns a real number with a Cauchy distribution.
        /// </summary>
        /// <returns></returns>
        public static double Cauchy()
        {
            return Math.Tan(Math.PI * (Uniform() - 0.5));
        }

        /// <summary>
        /// Returns a number from a discrete distribution: i with probability a[i].
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Discrete(double[] a)
        {
            const double epsilon = 1E-14;
            double sum = 0.0;
            for (int i = 0; i < a.Length; i++)
            {
                if (!(a[i] >= 0.0))
                {
                    throw new ArgumentException("array entry " + i + " must be nonnegative: " + a[i]);
                }
                sum = sum + a[i];
            }
            if (sum > 1.0 + epsilon || sum < 1.0 - epsilon)
            {
                throw new ArgumentException("sum of array entries does not approximately equal 1.0: " + sum);
            }

            // the for loop may not return a value when both r is (nearly) 1.0 and when the
            // cumulative sum is less than 1.0 (as a result of floating-point roundoff error)
            while (true)
            {
                double r = Uniform();
                sum = 0.0;
                for (int i = 0; i < a.Length; i++)
                {
                    sum = sum + a[i];
                    if (sum > r)
                    {
                        return i;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a real number from an exponential distribution with rate lambda.
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static double Exp(double lambda)
        {
            if (!(lambda > 0.0))
            {
                throw new ArgumentException("Rate lambda must be positive");
            }

            return -Math.Log(1 - Uniform()) / lambda;
        }

        /// <summary>
        /// Rearrange the elements of an array in random order.
        /// </summary>
        /// <param name="a"></param>
        public static void Shuffle(object[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                int r = i + Uniform(a.Length - i);     // between i and N-1
                object temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }

        /// <summary>
        /// Rearrange the elements of a double array in random order.
        /// </summary>
        /// <param name="a"></param>
        public static void Shuffle(double[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                int r = i + Uniform(a.Length - i);     // between i and N-1
                double temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }

        /// <summary>
        /// Rearrange the elements of an int array in random order.
        /// </summary>
        /// <param name="a"></param>
        public static void Shuffle(int[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                int r = i + Uniform(a.Length - i);     // between i and N-1
                int temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }

        /// <summary>
        /// Rearrange the elements of the subarray a[lo..hi] in random order.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        public static void Shuffle(Object[] a, int lo, int hi)
        {
            if (lo < 0 || lo > hi || hi >= a.Length)
            {
                throw new IndexOutOfRangeException("Illegal subarray range");
            }
            for (int i = lo; i <= hi; i++)
            {
                int r = i + Uniform(hi - i + 1);     // between i and hi
                Object temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }

        /// <summary>
        /// Rearrange the elements of the subarray a[lo..hi] in random order.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        public static void Shuffle(double[] a, int lo, int hi)
        {
            if (lo < 0 || lo > hi || hi >= a.Length)
            {
                throw new IndexOutOfRangeException("Illegal subarray range");
            }
            for (int i = lo; i <= hi; i++)
            {
                int r = i + Uniform(hi - i + 1);     // between i and hi
                double temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }

        /// <summary>
        /// Rearrange the elements of the subarray a[lo..hi] in random order.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        public static void Shuffle(int[] a, int lo, int hi)
        {
            if (lo < 0 || lo > hi || hi >= a.Length)
            {
                throw new IndexOutOfRangeException("Illegal subarray range");
            }
            for (int i = lo; i <= hi; i++)
            {
                int r = i + Uniform(hi - i + 1);     // between i and hi
                int temp = a[i];
                a[i] = a[r];
                a[r] = temp;
            }
        }

        /// <summary>
        /// Unit test.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            int n = int.Parse(args[0]);
            if (args.Length == 2)
            {
                SetSeed(int.Parse(args[1]));
            }
            double[] t = { .5, .3, .1, .1 };

            StdOut.PrintLn("seed = " + GetSeed());
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintF("{0:00} ", Uniform(100));
                StdOut.PrintF("{0:00.00000} ", Uniform(10.0, 99.0));
                StdOut.PrintF("{0,5} ", Bernoulli(.5));
                StdOut.PrintF("{0:0.00000} ", Gaussian(9.0, .2));
                StdOut.PrintF("{0:00} ", Discrete(t));
                StdOut.PrintLn();
            }

            string[] a = "A B C D E F G".Split(' ');
            foreach (string s in a)
            {
                StdOut.Print(s + " ");
            }
            StdOut.PrintLn();
        }
    }
}
