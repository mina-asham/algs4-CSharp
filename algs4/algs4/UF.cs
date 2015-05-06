using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class UF
    {
        /// <summary>
        /// Where id[i] = parent of i
        /// </summary>
        private readonly int[] _id;

        /// <summary>
        /// Where rank[i] = rank of subtree rooted at i
        /// (cannot be more than 31)
        /// </summary>
        private readonly byte[] _rank;

        /// <summary>
        /// Number of components
        /// </summary>
        private int _count;

        /// <summary>
        /// Initializes an empty union-find data structure with n
        /// isolated components 0 through n-1
        /// </summary>
        /// <param name="n">the number of sites</param>
        public UF(int n)
        {
            if (n < 0)
            {
                throw new ArgumentException();
            }
            _count = n;
            _id = new int[n];
            _rank = new byte[n];
            for (int i = 0; i < n; i++)
            {
                _id[i] = i;
                _rank[i] = 0;
            }
        }

        /// <summary>
        /// Returns the component identifier for the component containing site p.
        /// </summary>
        /// <param name="p">the integer representing one object</param>
        /// <returns>the component identifier for the component containing site p</returns>
        public int Find(int p)
        {
            if (p < 0 || p >= _id.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            while (p != _id[p])
            {
                _id[p] = _id[_id[p]];
                p = _id[p];
            }
            return p;
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
        /// Are the two sites p and q in the same component?
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <param name="q">the integer representing the other site</param>
        /// <returns>true if the two sites p and q are in the same component; false otherwise</returns>
        public bool Connected(int p, int q)
        {
            return Find(p) == Find(q);
        }

        /// <summary>
        /// Merges the component containing site p with the 
        /// the component containing site q.
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <param name="q">the integer representing the other site</param>
        public void Union(int p, int q)
        {
            int i = Find(p);
            int j = Find(q);
            if (i == j)
            {
                return;
            }

            if (_rank[i] < _rank[j])
            {
                _id[i] = j;
            }
            else if (_rank[i] > _rank[j])
            {
                _id[j] = i;
            }
            else
            {
                _id[j] = i;
                _rank[i]++;
            }
            _count--;
        }

        /// <summary>
        /// Reads in a an integer N and a sequence of pairs of integers
        /// (between 0 and N-1) from standard input, where each integer
        /// in the pair represents some site;
        /// if the sites are in different components, merge the two components
        /// and print the pair to standard output.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            int n = StdIn.ReadInt();
            UF uf = new UF(n);
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