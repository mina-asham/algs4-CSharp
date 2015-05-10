using algs4.stdlib;

namespace algs4.algs4
{
    public class DepthFirstSearch
    {
        /// <summary>
        /// marked[v] = is there an s-v path?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// Number of vertices connected to s
        /// </summary>
        private int _count;

        /// <summary>
        /// Computes the vertices in graph G that are
        /// connected to the source vertex s.
        /// </summary>
        /// <param name="g">the graph</param>
        /// <param name="s">the source vertex</param>
        public DepthFirstSearch(Graph g, int s)
        {
            _marked = new bool[g.V()];
            DFS(g, s);
        }

        /// <summary>
        /// Depth first search from v
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(Graph g, int v)
        {
            _count++;
            _marked[v] = true;
            foreach (int w in g.Adj(v))
            {
                if (!_marked[w])
                {
                    DFS(g, w);
                }
            }
        }

        /// <summary>
        /// Is there a path between the source vertex s and vertex v?
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>true if there is a path, false otherwise</returns>
        public bool Marked(int v)
        {
            return _marked[v];
        }

        /// <summary>
        /// Returns the number of vertices connected to the source vertex s.
        /// </summary>
        /// <returns>the number of vertices connected to the source vertex s</returns>
        public int Count()
        {
            return _count;
        }

        /// <summary>
        /// Unit tests the DepthFirstSearch data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Graph g = new Graph(input);
            int s = int.Parse(args[1]);
            DepthFirstSearch search = new DepthFirstSearch(g, s);
            for (int v = 0; v < g.V(); v++)
            {
                if (search.Marked(v))
                {
                    StdOut.Print(v + " ");
                }
            }

            StdOut.PrintLn();
            if (search.Count() != g.V())
            {
                StdOut.PrintLn("NOT connected");
            }
            else
            {
                StdOut.PrintLn("connected");
            }
        }
    }
}