using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class QuickFindUF
    {
        /// <summary>
        /// id[i] = component identifier of i
        /// </summary>
        private readonly int[] _id;

        /// <summary>
        /// Number of components
        /// </summary>
        private int _count;

        /// <summary>
        /// Initializes an empty union-find data structure with N isolated components 0 through N-1.
        /// </summary>
        /// <param name="n">the number of objects</param>
        public QuickFindUF(int n)
        {
            _count = n;
            _id = new int[n];
            for (int i = 0; i < n; i++)
            {
                _id[i] = i;
            }
        }

        /// <summary>
        /// Returns the number of components.
        /// </summary>
        /// <returns>the number of components (between 1 and N)</returns>
        public int Count()
        {
            return _count;
        }

        /// <summary>
        /// Returns the component identifier for the component containing site p.
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <returns>the component identifier for the component containing site p</returns>
        public int Find(int p)
        {
            Validate(p);
            return _id[p];
        }

        /// <summary>
        /// Validate that p is a valid index
        /// </summary>
        /// <param name="p"></param>
        private void Validate(int p)
        {
            int n = _id.Length;
            if (p < 0 || p >= n)
            {
                throw new IndexOutOfRangeException("index " + p + " is not between 0 and " + n);
            }
        }

        /// <summary>
        /// Are the two sites p and q/tt> in the same component?
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <param name="q">the integer representing the other site</param>
        /// <returns>true if the two sites p and q are in the same component, and false otherwise</returns>
        public bool Connected(int p, int q)
        {
            Validate(p);
            Validate(q);
            return _id[p] == _id[q];
        }

        /// <summary>
        /// Merges the component containing sitep with the component
        /// containing site q.
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <param name="q">the integer representing the other site</param>
        public void Union(int p, int q)
        {
            if (Connected(p, q))
            {
                return;
            }
            int pid = _id[p];
            for (int i = 0; i < _id.Length; i++)
            {
                if (_id[i] == pid)
                {
                    _id[i] = _id[q];
                }
            }
            _count--;
        }

        /// <summary>
        /// Reads in a sequence of pairs of integers (between 0 and N-1) from standard input,
        /// where each integer represents some object;
        /// if the objects are in different components, merge the two components
        /// and Print the pair to standard output.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int n = StdIn.ReadInt();
            QuickFindUF uf = new QuickFindUF(n);
            while (!StdIn.IsEmpty())
            {
                int p = StdIn.ReadInt();
                int q = StdIn.ReadInt();
                if (uf.Connected(p, q))
                {
                    continue;
                }
                uf.Union(p, q);
                StdOut.PrintLn(p + " " + q);
            }
            StdOut.PrintLn(uf.Count() + " components");
        }
    }
}