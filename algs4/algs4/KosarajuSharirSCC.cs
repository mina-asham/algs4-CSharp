using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class KosarajuSharirSCC
    {
        /// <summary>
        /// marked[v] = has vertex v been visited?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// id[v] = id of strong component containing v
        /// </summary>
        private readonly int[] _id;

        /// <summary>
        /// Number of strongly-connected components
        /// </summary>
        private readonly int _count;

        /// <summary>
        /// Computes the strong components of the digraph G.
        /// </summary>
        /// <param name="g">the digraph</param>
        public KosarajuSharirSCC(Digraph g)
        {
            // compute reverse postorder of reverse graph
            DepthFirstOrder dfs = new DepthFirstOrder(g.Reverse());

            // run DFS on G, using reverse postorder to guide calculation
            _marked = new bool[g.V()];
            _id = new int[g.V()];
            foreach (int v in dfs.ReversePost())
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                    _count++;
                }
            }

            // check that id[] gives strong components
            Debug.Assert(Check(g));
        }

        /// <summary>
        /// DFS on graph G
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(Digraph g, int v)
        {
            _marked[v] = true;
            _id[v] = _count;
            foreach (int w in g.Adj(v))
            {
                if (!_marked[w])
                {
                    DFS(g, w);
                }
            }
        }

        /// <summary>
        /// Returns the number of strong components.
        /// </summary>
        /// <returns>the number of strong components</returns>
        public int Count()
        {
            return _count;
        }

        /// <summary>
        /// Are vertices v and w in the same strong component?
        /// </summary>
        /// <param name="v">one vertex</param>
        /// <param name="w">the other vertex</param>
        /// <returns>true if vertices v and w are in the same strong component, and false otherwise</returns>
        public bool StronglyConnected(int v, int w)
        {
            return _id[v] == _id[w];
        }

        /// <summary>
        /// Returns the component id of the strong component containing vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the component id of the strong component containing vertex v</returns>
        public int Id(int v)
        {
            return _id[v];
        }

        /// <summary>
        /// Does the id[] array contain the strongly connected components?
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private bool Check(Digraph g)
        {
            TransitiveClosure tc = new TransitiveClosure(g);
            for (int v = 0; v < g.V(); v++)
            {
                for (int w = 0; w < g.V(); w++)
                {
                    if (StronglyConnected(v, w) != (tc.Reachable(v, w) && tc.Reachable(w, v)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Unit tests the KosarajuSharirSCC data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);
            KosarajuSharirSCC scc = new KosarajuSharirSCC(g);

            // number of connected components
            int m = scc.Count();
            StdOut.PrintLn(m + " components");

            // compute list of vertices in each strong component
            Queue<int>[] components = new Queue<int>[m];
            for (int i = 0; i < m; i++)
            {
                components[i] = new Queue<int>();
            }
            for (int v = 0; v < g.V(); v++)
            {
                components[scc.Id(v)].Enqueue(v);
            }

            // Print results
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