using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class WeightedQuickUnionUF
    {
        /// <summary>
        /// Where parent[i] = parent of i
        /// </summary>
        private readonly int[] _parent;

        /// <summary>
        /// WHere size[i] = number of objects in subtree rooted at i
        /// </summary>
        private readonly int[] _size;

        /// <summary>
        /// Number of components
        /// </summary>
        private int _count;

        /// <summary>
        /// Initializes an empty union-find data structure with N isolated components 0 through N-1.
        /// </summary>
        /// <param name="n">The number of objects</param>
        public WeightedQuickUnionUF(int n)
        {
            _count = n;
            _parent = new int[n];
            _size = new int[n];
            for (int i = 0; i < n; i++)
            {
                _parent[i] = i;
                _size[i] = 1;
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
            while (p != _parent[p])
            {
                p = _parent[p];
            }
            return p;
        }

        /// <summary>
        /// Validate that p is a valid index
        /// </summary>
        /// <param name="p">the index to validate</param>
        private void Validate(int p)
        {
            int n = _parent.Length;
            if (p < 0 || p >= n)
            {
                throw new IndexOutOfRangeException("index " + p + " is not between 0 and " + n);
            }
        }

        /// <summary>
        /// Are the two sites p and q in the same component?
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <param name="q">the integer representing the other site</param>
        /// <returns>true if the two sites p and q are in the same component, and false otherwise</returns>
        public bool Connected(int p, int q)
        {
            return Find(p) == Find(q);
        }

        /// <summary>
        /// Merges the component containing sitep with the component
        /// containing site q.
        /// </summary>
        /// <param name="p">the integer representing one site</param>
        /// <param name="q">the integer representing the other site</param>
        public void Union(int p, int q)
        {
            int rootP = Find(p);
            int rootQ = Find(q);
            if (rootP == rootQ)
            {
                return;
            }

            // make smaller root point to larger one
            if (_size[rootP] < _size[rootQ])
            {
                _parent[rootP] = rootQ;
                _size[rootQ] += _size[rootP];
            }
            else
            {
                _parent[rootQ] = rootP;
                _size[rootP] += _size[rootQ];
            }
            _count--;
        }

        /// <summary>
        /// Reads in a sequence of pairs of integers (between 0 and N-1) from standard input,
        /// where each integer represents some object;
        /// if the objects are in different components, merge the two components
        /// and print the pair to standard output.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            int n = StdIn.ReadInt();
            WeightedQuickUnionUF uf = new WeightedQuickUnionUF(n);
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