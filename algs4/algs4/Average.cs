using algs4.stdlib;

namespace algs4.algs4
{
    public class Average
    {
        /// <summary>
        /// Reads in a sequence of real numbers from standard input and prints
        /// out their average to standard output.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            int count = 0;      // number of input values
            double sum = 0.0;   // sum of input values

            // read data and compute statistics
            while (!StdIn.IsEmpty())
            {
                double value = StdIn.ReadDouble();
                sum += value;
                count++;
            }

            // computer the average
            double average = sum / count;

            // print results
            StdOut.PrintLn("Average is " + average);
        }
    }
}
