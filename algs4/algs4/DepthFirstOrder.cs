using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class DepthFirstOrder
    {
        /// <summary>
        /// marked[v] = has v been marked in dfs?
        /// </summary>
        private readonly bool[] _marked;

        /// <summary>
        /// pre[v]    = preorder  number of v
        /// </summary>
        private readonly int[] _pre;

        /// <summary>
        /// post[v]   = postorder number of v
        /// </summary>
        private readonly int[] _post;

        /// <summary>
        /// Vertices in preorder
        /// </summary>
        private readonly Queue<int> _preorder;

        /// <summary>
        /// Vertices in postorder
        /// </summary>
        private readonly Queue<int> _postorder;

        /// <summary>
        /// Counter or preorder numbering
        /// </summary>
        private int _preCounter;

        /// <summary>
        /// Counter foreach postorder numbering
        /// </summary>
        private int _postCounter;

        /// <summary>
        /// Determines a depth-first order for the digraph G.
        /// </summary>
        /// <param name="g">the digraph</param>
        public DepthFirstOrder(Digraph g)
        {
            _pre = new int[g.V()];
            _post = new int[g.V()];
            _postorder = new Queue<int>();
            _preorder = new Queue<int>();
            _marked = new bool[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                }
            }
        }

        /// <summary>
        /// Determines a depth-first order for the edge-weighted digraph G.
        /// </summary>
        /// <param name="g">the edge-weighted digraph</param>
        public DepthFirstOrder(EdgeWeightedDigraph g)
        {
            _pre = new int[g.V()];
            _post = new int[g.V()];
            _postorder = new Queue<int>();
            _preorder = new Queue<int>();
            _marked = new bool[g.V()];
            for (int v = 0; v < g.V(); v++)
            {
                if (!_marked[v])
                {
                    DFS(g, v);
                }
            }
        }

        /// <summary>
        /// Run DFS in digraph G from vertex v and compute preorder/postorder
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(Digraph g, int v)
        {
            _marked[v] = true;
            _pre[v] = _preCounter++;
            _preorder.Enqueue(v);
            foreach (int w in g.Adj(v))
            {
                if (!_marked[w])
                {
                    DFS(g, w);
                }
            }
            _postorder.Enqueue(v);
            _post[v] = _postCounter++;
        }

        /// <summary>
        /// Run DFS in edge-weighted digraph G from vertex v and compute preorder/postorder
        /// </summary>
        /// <param name="g"></param>
        /// <param name="v"></param>
        private void DFS(EdgeWeightedDigraph g, int v)
        {
            _marked[v] = true;
            _pre[v] = _preCounter++;
            _preorder.Enqueue(v);
            foreach (DirectedEdge e in g.Adj(v))
            {
                int w = e.To();
                if (!_marked[w])
                {
                    DFS(g, w);
                }
            }
            _postorder.Enqueue(v);
            _post[v] = _postCounter++;
        }

        /// <summary>
        /// Returns the preorder number of vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the preorder number of vertex v</returns>
        public int Pre(int v)
        {
            return _pre[v];
        }

        /// <summary>
        /// Returns the postorder number of vertex v.
        /// </summary>
        /// <param name="v">the vertex</param>
        /// <returns>the postorder number of vertex v</returns>
        public int Post(int v)
        {
            return _post[v];
        }

        /// <summary>
        /// Returns the vertices in postorder.
        /// </summary>
        /// <returns>the vertices in postorder, as an iterable of vertices</returns>
        public IEnumerable<int> Post()
        {
            return _postorder;
        }

        /// <summary>
        /// Returns the vertices in preorder.
        /// </summary>
        /// <returns>the vertices in preorder, as an iterable of vertices</returns>
        public IEnumerable<int> Pre()
        {
            return _preorder;
        }

        /// <summary>
        /// Returns the vertices in reverse postorder.
        /// </summary>
        /// <returns>the vertices in reverse postorder, as an iterable of vertices</returns>
        public IEnumerable<int> ReversePost()
        {
            Stack<int> reverse = new Stack<int>();
            foreach (int v in _postorder)
            {
                reverse.Push(v);
            }
            return reverse;
        }

        /// <summary>
        /// Check that pre() and post() are consistent with pre(v) and post(v)
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            // check that post(v) is consistent with post()
            int r = 0;
            foreach (int v in Post())
            {
                if (Post(v) != r)
                {
                    StdOut.PrintLn("post(v) and post() inconsistent");
                    return false;
                }
                r++;
            }

            // check that pre(v) is consistent with pre()
            r = 0;
            foreach (int v in Pre())
            {
                if (Pre(v) != r)
                {
                    StdOut.PrintLn("pre(v) and pre() inconsistent");
                    return false;
                }
                r++;
            }

            return true;
        }

        /// <summary>
        /// Unit tests the DepthFirstOrder data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            In input = new In(args[0]);
            Digraph g = new Digraph(input);

            DepthFirstOrder dfs = new DepthFirstOrder(g);
            StdOut.PrintLn("   v  pre post");
            StdOut.PrintLn("--------------");
            for (int v = 0; v < g.V(); v++)
            {
                StdOut.PrintF("{0:0000} {1:0000} {2:0000}\n", v, dfs.Pre(v), dfs.Post(v));
            }

            StdOut.Print("Preorderin  ");
            foreach (int v in dfs.Pre())
            {
                StdOut.Print(v + " ");
            }
            StdOut.PrintLn();

            StdOut.Print("Postorder: ");
            foreach (int v in dfs.Post())
            {
                StdOut.Print(v + " ");
            }
            StdOut.PrintLn();

            StdOut.Print("Reverse postorder: ");
            foreach (int v in dfs.ReversePost())
            {
                StdOut.Print(v + " ");
            }
            StdOut.PrintLn();
        }
    }
}
