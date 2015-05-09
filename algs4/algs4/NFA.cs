using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class NFA
    {
        /// <summary>
        /// Digraph of epsilon transitions
        /// </summary>
        private readonly Digraph _g;

        /// <summary>
        /// Regular expression
        /// </summary>
        private readonly string _regexp;

        /// <summary>
        /// Number of characters in regular expression
        /// </summary>
        private readonly int _m;

        /// <summary>
        /// Create the NFA for the given RE
        /// </summary>
        /// <param name="regexp"></param>
        public NFA(string regexp)
        {
            _regexp = regexp;
            _m = regexp.Length;
            Stack<int> ops = new Stack<int>();
            _g = new Digraph(_m + 1);
            for (int i = 0; i < _m; i++)
            {
                int lp = i;
                if (regexp[i] == '(' || regexp[i] == '|')
                {
                    ops.Push(i);
                }
                else if (regexp[i] == ')')
                {
                    int or = ops.Pop();

                    // 2-way or operator
                    if (regexp[or] == '|')
                    {
                        lp = ops.Pop();
                        _g.AddEdge(lp, or + 1);
                        _g.AddEdge(or, i);
                    }
                    else if (regexp[or] == '(')
                    {
                        lp = or;
                    }
                }

                // closure operator (uses 1-character lookahead)
                if (i < _m - 1 && regexp[i + 1] == '*')
                {
                    _g.AddEdge(lp, i + 1);
                    _g.AddEdge(i + 1, lp);
                }
                if (regexp[i] == '(' || regexp[i] == '*' || regexp[i] == ')')
                {
                    _g.AddEdge(i, i + 1);
                }
            }
        }

        /// <summary>
        /// Does the NFA recognize txt? 
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public bool Recognizes(string txt)
        {
            DirectedDFS dfs = new DirectedDFS(_g, 0);
            Bag<int> pc = new Bag<int>();
            for (int v = 0; v < _g.V(); v++)
            {
                if (dfs.Marked(v))
                {
                    pc.Add(v);
                }
            }

            // Compute possible NFA states for txt[i+1]
            for (int i = 0; i < txt.Length; i++)
            {
                Bag<int> match = new Bag<int>();
                foreach (int v in pc)
                {
                    if (v == _m)
                    {
                        continue;
                    }
                    if ((_regexp[v] == txt[i]) || _regexp[v] == '.')
                    {
                        match.Add(v + 1);
                    }
                }
                dfs = new DirectedDFS(_g, match);
                pc = new Bag<int>();
                for (int v = 0; v < _g.V(); v++)
                {
                    if (dfs.Marked(v))
                    {
                        pc.Add(v);
                    }
                }

                // optimization if no states reachable
                if (pc.Size() == 0)
                {
                    return false;
                }
            }

            // check for accept state
            foreach (int v in pc)
            {
                if (v == _m)
                {
                    return true;
                }
            }
            return false;
        }

        public static void RunMain(string[] args)
        {
            string regexp = "(" + args[0] + ")";
            string txt = args[1];
            if (txt.IndexOf('|') >= 0)
            {
                throw new ArgumentException("| character in text is not supported");
            }
            NFA nfa = new NFA(regexp);
            StdOut.PrintLn(nfa.Recognizes(txt));
        }
    }
}