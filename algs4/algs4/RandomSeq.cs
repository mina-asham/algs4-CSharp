using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class RandomSeq
    {
        /// <summary>
        /// Reads in two command-line arguments lo and hi and prints N uniformly
        /// random real numbers in [lo, hi) to standard output.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // command-line arguments
            int n = int.Parse(args[0]);

            // for backward compatibility with Intro to Programming in Java version of RandomSeq
            if (args.Length == 1)
            {
                // generate and print N numbers between 0.0 and 1.0
                for (int i = 0; i < n; i++)
                {
                    double x = StdRandom.Uniform();
                    StdOut.PrintLn(x);
                }
            }

            else if (args.Length == 3)
            {
                double lo = double.Parse(args[1]);
                double hi = double.Parse(args[2]);

                // generate and print N numbers between lo and hi
                for (int i = 0; i < n; i++)
                {
                    double x = StdRandom.Uniform(lo, hi);
                    StdOut.PrintF("{0:0.00}\n", x);
                }
            }

            else
            {
                throw new ArgumentException("Invalid number of arguments");
            }
        }
    }
}