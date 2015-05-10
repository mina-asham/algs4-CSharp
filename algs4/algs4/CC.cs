using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class CC
    {
        /// <summary>
        /// marked[v] = has vertex v been marked?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// id[v] = id of connected component containing v
        /// </summary>
        private readonly int[] _id;

        /// <summary>
        /// size[id] = number of vertices in given component
        /// </summary>
        private readonly int[] _size;

        /// <summary>
        /// Number of connected components
        /// </summary>
        private readonly int _count;

        /// <summary>
        /// Computes the connected components of the undirected graph G.
        /// </summary>
        /// <param name="g">the graph</param>
        public CC(Graph g)
        {
            _marked = new bool[g.V()];
            _id = new int[g.V()];
            _size = new int[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                    _count++;
                }
            }
        }

        /// <summary>
        /// Depth-first search
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(Graph g, int v)
        {
            _marked[v] = true;
            _id[v] = _count;
            _size[_count]++;
            foreach (int w in g.Adj(v))
            {
                if (!_marked[w])
                {
                    DFS(g, w);
                }
            }
        }

        /// <summary>
        /// Returns the component id of the connected component containing vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the component id of the connected component containing vertex v</returns>
        public int Id(int v)
        {
            return _id[v];
        }

        /// <summary>
        /// Returns the number of vertices in the connected component containing vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the number of vertices in the connected component containing vertex v</returns>
        public int Size(int v)
        {
            return _size[_id[v]];
        }

        /// <summary>
        /// Returns the number of connected components.
        /// </summary>
        /// <returns>the number of connected components</returns>
        public int Count()
        {
            return _count;
        }

        /// <summary>
        /// Are vertices v and w in the same connected component?
        /// </summary>
        /// <param name="v">one vertex</param>
        /// <param name="w">the other vertex</param>
        /// <returns>true if vertices v and w are in the same connected component, and false otherwise</returns>
        public bool Connected(int v, int w)
        {
            return Id(v) == Id(w);
        }

        /// <summary>
        /// Are vertices v and w in the same connected component?
        /// </summary>
        /// <param name="v">one vertex</param>
        /// <param name="w">the other vertex</param>
        /// <returns>true if vertices v and w are in the same connected component, and false otherwise</returns>
        [Obsolete("Use connected(v, w) instead.")]
        public bool AreConnected(int v, int w)
        {
            return Id(v) == Id(w);
        }

        /// <summary>
        /// Unit tests the CC data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Graph g = new Graph(input);
            CC cc = new CC(g);

            // number of connected components
            int m = cc.Count();
            StdOut.PrintLn(m + " components");

            // compute list of vertices in each connected component
            Queue<int>[] components = new Queue<int>[m];
            for (int i = 0; i < m; i++)
            {
                components[i] = new Queue<int>();
            }
            for (int v = 0; v < g.V(); v++)
            {
                components[cc.Id(v)].Enqueue(v);
            }

            // print results
            for (int i = 0; i < m; i++)
            {
                foreach (int v in components[i])
                {
                    StdOut.Print(v + " ");
                }
                StdOut.PrintLn();
            }
        }
    }
}