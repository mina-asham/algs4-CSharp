using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class SymbolDigraph
    {
        /// <summary>
        /// string -> index
        /// </summary>
        private readonly ST<string, int> _st;

        /// <summary>
        /// index  -> string
        /// </summary>
        private readonly string[] _keys;

        private readonly Digraph _g;

        /// <summary>
        /// Initializes a digraph from a file using the specified delimiter.
        /// Each line in the file contains
        /// the name of a vertex, followed by a list of the names
        /// of the vertices adjacent to that vertex, separated by the delimiter.
        /// </summary>
        /// <param name="filename">the name of the file</param>
        /// <param name="delimiter">the delimiter between fields</param>
        public SymbolDigraph(string filename, string delimiter)
        {
            _st = new ST<string, int>();

            // First pass builds the index by reading strings to associate
            // distinct strings with an index
            In input = new In(filename);
            while (input.HasNextLine())
            {
                string[] a = input.ReadLine().Split(new[] { delimiter }, StringSplitOptions.None);
                for (int i = 0; i < a.Length; i++)
                {
                    if (!_st.Contains(a[i]))
                    {
                        _st.Put(a[i], _st.Size());
                    }
                }
            }

            // inverted index to get string keys in an aray
            _keys = new string[_st.Size()];
            foreach (string name in _st.Keys())
            {
                _keys[_st.Get(name)] = name;
            }

            // second pass builds the digraph by connecting first vertex on each
            // line to all others
            _g = new Digraph(_st.Size());
            input = new In(filename);
            while (input.HasNextLine())
            {
                string[] a = input.ReadLine().Split(new[] { delimiter }, StringSplitOptions.None);
                int v = _st.Get(a[0]);
                for (int i = 1; i < a.Length; i++)
                {
                    int w = _st.Get(a[i]);
                    _g.AddEdge(v, w);
                }
            }
        }

        /// <summary>
        /// Does the digraph contain the vertex named s?
        /// </summary>
        /// <param name="s">the name of a vertex</param>
        /// <returns>true if s is the name of a vertex, and false otherwise</returns>
        public bool Contains(string s)
        {
            return _st.Contains(s);
        }

        /// <summary>
        /// Returns the integer associated with the vertex named s.
        /// </summary>
        /// <param name="s">the name of a vertex</param>
        /// <returns>the integer (between 0 and _v - 1) associated with the vertex named s</returns>
        public int Index(string s)
        {
            return _st.Get(s);
        }

        /// <summary>
        /// Returns the name of the vertex associated with the integer v.
        /// </summary>
        /// <param name="v">the integer corresponding to a vertex (between 0 and _v - 1) </param>
        /// <returns>the name of the vertex associated with the integer v</returns>
        public string Name(int v)
        {
            return _keys[v];
        }

        /// <summary>
        /// Returns the digraph assoicated with the symbol graph. It is the client's responsibility
        /// not to mutate the digraph.
        /// </summary>
        /// <returns>the digraph associated with the symbol digraph</returns>
        public Digraph G()
        {
            return _g;
        }

        /// <summary>
        /// Unit tests the SymbolDigraph data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string filename = args[0];
            string delimiter = args[1];
            SymbolDigraph sg = new SymbolDigraph(filename, delimiter);
            Digraph g = sg.G();
            while (!StdIn.IsEmpty())
            {
                string t = StdIn.ReadLine();
                foreach (int v in g.Adj(sg.Index(t)))
                {
                    StdOut.PrintLn("   " + sg.Name(v));
                }
            }
        }
    }
}
