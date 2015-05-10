using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Max children per B-tree node = M-1
        /// </summary>
        private const int M = 4;

        /// <summary>
        /// Root of the B-tree
        /// </summary>
        private Node _root;

        /// <summary>
        /// Height of the B-tree
        /// </summary>
        private int _ht;

        /// <summary>
        /// Number of key-value pairs in the B-tree
        /// </summary>
        private int _n;

        /// <summary>
        /// Helper B-tree node data type
        /// </summary>
        private class Node
        {
            /// <summary>
            /// Number of children
            /// </summary>
            public int N { get; set; }

            /// <summary>
            /// The array of children
            /// </summary>
            public Entry[] Children { get; private set; }

            /// <summary>
            /// Create a node with k children
            /// </summary>
            /// <param name="k"></param>
            public Node(int k)
            {
                N = k;
                Children = new Entry[M];
            }
        }

        /// <summary>
        /// Internal nodes: only use key and next
        /// External nodes: only use key and value
        /// </summary>
        private class Entry
        {
            public TKey Key { get; set; }

            public TValue Value { get; private set; }

            /// <summary>
            /// Helper field to iterate over array entries
            /// </summary>
            public Node Next { get; set; }

            public Entry(TKey key, TValue value, Node next)
            {
                Key = key;
                Value = value;
                Next = next;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BTree()
        {
            _root = new Node(0);
        }

        /// <summary>
        /// Return number of key-value pairs in the B-tree
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Return height of B-tree
        /// </summary>
        /// <returns></returns>
        public int Height()
        {
            return _ht;
        }

        /// <summary>
        /// Search for given key, return associated value; return null if no such key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            bool exists;
            return Search(_root, key, _ht, out exists);
        }

        public bool Contains(TKey key)
        {
            bool exists;
            Search(_root, key, _ht, out exists);
            return exists;
        }

        private TValue Search(Node x, TKey key, int ht, out bool exists)
        {
            Entry[] children = x.Children;

            // external node
            if (ht == 0)
            {
                for (int j = 0; j < x.N; j++)
                {
                    if (Eq(key, children[j].Key))
                    {
                        exists = true;
                        return children[j].Value;
                    }
                }
            }

            // internal node
            else
            {
                for (int j = 0; j < x.N; j++)
                {
                    if (j + 1 == x.N || Less(key, children[j + 1].Key))
                    {
                        return Search(children[j].Next, key, ht - 1, out exists);
                    }
                }
            }

            exists = false;
            return default(TValue);
        }

        /// <summary>
        /// Insert key-value pair
        /// add code to check for duplicate keys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKey key, TValue value)
        {
            Node u = Insert(_root, key, value, _ht);
            _n++;
            if (u == null)
            {
                return;
            }

            // need to split root
            Node t = new Node(2);
            t.Children[0] = new Entry(_root.Children[0].Key, default(TValue), _root);
            t.Children[1] = new Entry(u.Children[0].Key, default(TValue), u);
            _root = t;
            _ht++;
        }

        private Node Insert(Node h, TKey key, TValue value, int ht)
        {
            int j;
            Entry t = new Entry(key, value, null);

            // external node
            if (ht == 0)
            {
                for (j = 0; j < h.N; j++)
                {
                    if (Less(key, h.Children[j].Key))
                    {
                        break;
                    }
                }
            }

            // internal node
            else
            {
                for (j = 0; j < h.N; j++)
                {
                    if ((j + 1 == h.N) || Less(key, h.Children[j + 1].Key))
                    {
                        Node u = Insert(h.Children[j++].Next, key, value, ht - 1);
                        if (u == null)
                        {
                            return null;
                        }
                        t.Key = u.Children[0].Key;
                        t.Next = u;
                        break;
                    }
                }
            }

            for (int i = h.N; i > j; i--)
            {
                h.Children[i] = h.Children[i - 1];
            }
            h.Children[j] = t;
            h.N++;
            if (h.N < M)
            {
                return null;
            }
            return Split(h);
        }

        /// <summary>
        /// Split node in half
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private static Node Split(Node h)
        {
            Node t = new Node(M / 2);
            h.N = M / 2;
            for (int j = 0; j < M / 2; j++)
            {
                t.Children[j] = h.Children[M / 2 + j];
            }
            return t;
        }

        /// <summary>
        /// For debugging
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(_root, _ht, "") + "\n";
        }

        private string ToString(Node h, int ht, string indent)
        {
            string s = "";
            Entry[] children = h.Children;

            if (ht == 0)
            {
                for (int j = 0; j < h.N; j++)
                {
                    s += indent + children[j].Key + " " + children[j].Value + "\n";
                }
            }
            else
            {
                for (int j = 0; j < h.N; j++)
                {
                    if (j > 0)
                    {
                        s += indent + "(" + children[j].Key + ")\n";
                    }
                    s += ToString(children[j].Next, ht - 1, indent + "     ");
                }
            }
            return s;
        }

        /// <summary>
        /// Comparison functions - make Key instead of Key to avoid casts
        /// </summary>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <returns></returns>
        private static bool Less(TKey k1, TKey k2)
        {
            return k1.CompareTo(k2) < 0;
        }

        private static bool Eq(TKey k1, TKey k2)
        {
            return k1.CompareTo(k2) == 0;
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            BTree<string, string> st = new BTree<string, string>();

            st.Put("www.cs.princeton.edu", "128.112.136.11");
            st.Put("www.princeton.edu", "128.112.128.15");
            st.Put("www.yale.edu", "130.132.143.21");
            st.Put("www.simpsons.com", "209.052.165.60");
            st.Put("www.apple.com", "17.112.152.32");
            st.Put("www.amazon.com", "207.171.182.16");
            st.Put("www.ebay.com", "66.135.192.87");
            st.Put("www.cnn.com", "64.236.16.20");
            st.Put("www.google.com", "216.239.41.99");
            st.Put("www.nytimes.com", "199.239.136.200");
            st.Put("www.microsoft.com", "207.126.99.140");
            st.Put("www.dell.com", "143.166.224.230");
            st.Put("www.slashdot.org", "66.35.250.151");
            st.Put("www.espn.com", "199.181.135.201");
            st.Put("www.weather.com", "63.111.66.11");
            st.Put("www.yahoo.com", "216.109.118.65");

            StdOut.PrintLn("cs.princeton.edu:  " + st.Get("www.cs.princeton.edu"));
            StdOut.PrintLn("hardvardsucks.com: " + st.Get("www.harvardsucks.com"));
            StdOut.PrintLn("simpsons.com:      " + st.Get("www.simpsons.com"));
            StdOut.PrintLn("apple.com:         " + st.Get("www.apple.com"));
            StdOut.PrintLn("ebay.com:          " + st.Get("www.ebay.com"));
            StdOut.PrintLn("dell.com:          " + st.Get("www.dell.com"));
            StdOut.PrintLn();

            StdOut.PrintLn("size:    " + st.Size());
            StdOut.PrintLn("height:  " + st.Height());
            StdOut.PrintLn(st);
            StdOut.PrintLn();
        }
    }
}