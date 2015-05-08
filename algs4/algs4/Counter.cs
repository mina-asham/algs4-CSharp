using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Counter : IComparable<Counter>
    {
        /// <summary>
        /// Counter name
        /// </summary>
        private readonly string _name;

        /// <summary>
        /// Current value
        /// </summary>
        private int _count;

        /// <summary>
        /// Initializes a new counter starting at 0, with the given id.
        /// </summary>
        /// <param name="id">the name of the counter</param>
        public Counter(string id)
        {
            _name = id;
        }

        /// <summary>
        /// Increments the counter by 1.
        /// </summary>
        public void Increment()
        {
            _count++;
        }

        /// <summary>
        /// Returns the current count.
        /// </summary>
        /// <returns></returns>
        public int Tally()
        {
            return _count;
        }

        /// <summary>
        /// Returns a string representation of this counter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _count + " " + _name;
        }

        /// <summary>
        /// Compares this counter to that counter.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public int CompareTo(Counter that)
        {
            if (_count < that._count)
            {
                return -1;
            }
            if (_count > that._count)
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Reads two command-line integers N and T; creates N counters;
        /// increments T counters at random; and prints results.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int n = int.Parse(args[0]);
            int t = int.Parse(args[1]);

            // create N counters
            Counter[] hits = new Counter[n];
            for (int i = 0; i < n; i++)
            {
                hits[i] = new Counter("counter" + i);
            }

            // increment T counters at random
            for (int i = 0; i < t; i++)
            {
                hits[StdRandom.Uniform(n)].Increment();
            }

            // print results
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintLn(hits[i]);
            }
        }
    }
}