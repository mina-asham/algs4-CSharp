using System;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class TST<TValue>
    {
        /// <summary>
        /// Size
        /// </summary>
        private int _n;

        /// <summary>
        /// Root of TST
        /// </summary>
        private Node<TValue> _root;

        private class Node<TValueInner>
        {
            /// <summary>
            /// Character
            /// </summary>
            public char C { get; set; }

            /// <summary>
            /// Left subtries
            /// </summary>
            public Node<TValueInner> Left { get; set; }

            /// <summary>
            /// Middle subtries
            /// </summary>
            public Node<TValueInner> Mid { get; set; }

            /// <summary>
            /// Right subtries
            /// </summary>
            public Node<TValueInner> Right { get; set; }

            /// <summary>
            /// Value associated with string
            /// </summary>
            public TValue Val { get; set; }
        }

        /// <summary>
        /// Returns the number of key-value pairs in this symbol table.
        /// </summary>
        /// <returns>the number of key-value pairs in this symbol table</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Does this symbol table contain the given key?
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if this symbol table contains key and false otherwise</returns>
        public bool Contains(string key)
        {
            return typeof(TValue).IsValueType || !Equals(Get(key), default(TValue));
        }

        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the value associated with the given key if the key is in the symbol table and null if the key is not in the symbol table</returns>
        public TValue Get(string key)
        {
            if (key == null)
            {
                throw new NullReferenceException();
            }
            if (key.Length == 0)
            {
                throw new ArgumentException("key must have length >= 1");
            }
            Node<TValue> x = Get(_root, key, 0);
            if (x == null)
            {
                return default(TValue);
            }
            return x.Val;
        }

        /// <summary>
        /// Return subtrie corresponding to given key
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        private Node<TValue> Get(Node<TValue> x, string key, int d)
        {
            if (key == null)
            {
                throw new NullReferenceException();
            }
            if (key.Length == 0)
            {
                throw new ArgumentException("key must have length >= 1");
            }
            if (x == null)
            {
                return null;
            }
            char c = key[d];
            if (c < x.C)
            {
                return Get(x.Left, key, d);
            }
            if (c > x.C)
            {
                return Get(x.Right, key, d);
            }
            if (d < key.Length - 1)
            {
                return Get(x.Mid, key, d + 1);
            }
            return x;
        }

        /// <summary>
        /// Inserts the key-value pair into the symbol table, overwriting the old value
        /// with the new value if the key is already in the symbol table.
        /// If the value is null, this effectively deletes the key from the symbol table.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="val">the value</param>
        public void Put(string key, TValue val)
        {
            if (!Contains(key))
            {
                _n++;
            }
            _root = Put(_root, key, val, 0);
        }

        private Node<TValue> Put(Node<TValue> x, string key, TValue val, int d)
        {
            char c = key[d];
            if (x == null)
            {
                x = new Node<TValue>();
                x.C = c;
            }
            if (c < x.C)
            {
                x.Left = Put(x.Left, key, val, d);
            }
            else if (c > x.C)
            {
                x.Right = Put(x.Right, key, val, d);
            }
            else if (d < key.Length - 1)
            {
                x.Mid = Put(x.Mid, key, val, d + 1);
            }
            else
            {
                x.Val = val;
            }
            return x;
        }

        /// <summary>
        /// Returns the string in the symbol table that is the longest prefix of query,
        /// or null, if no such string.
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>the string in the symbol table that is the longest prefix of query, or null if no such string</returns>
        public string LongestPrefixOf(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }
            int length = 0;
            Node<TValue> x = _root;
            int i = 0;
            while (x != null && i < query.Length)
            {
                char c = query[i];
                if (c < x.C)
                {
                    x = x.Left;
                }
                else if (c > x.C)
                {
                    x = x.Right;
                }
                else
                {
                    i++;
                    if (typeof(TValue).IsValueType || !Equals(x.Val, default(TValue)))
                    {
                        length = i;
                    }
                    x = x.Mid;
                }
            }
            return query.Substring(0, length);
        }

        /// <summary>
        /// Returns all keys in the symbol table as an Iterable.
        /// To iterate over all of the keys in the symbol table named st,
        /// use the foreacheach notation: for (Key key in st.keys()).
        /// </summary>
        /// <returns>all keys in the sybol table as an Iterable</returns>
        public IEnumerable<string> Keys()
        {
            Queue<string> queue = new Queue<string>();
            Collect(_root, new StringBuilder(), queue);
            return queue;
        }

        /// <summary>
        /// Returns all of the keys in the set that start with prefix.
        /// </summary>
        /// <param name="prefix">the prefix</param>
        /// <returns>all of the keys in the set that start with prefix, as an iterable</returns>
        public IEnumerable<string> KeysWithPrefix(string prefix)
        {
            Queue<string> queue = new Queue<string>();
            Node<TValue> x = Get(_root, prefix, 0);
            if (x == null)
            {
                return queue;
            }
            if (typeof(TValue).IsValueType || !Equals(x.Val, default(TValue)))
            {
                queue.Enqueue(prefix);
            }
            Collect(x.Mid, new StringBuilder(prefix), queue);
            return queue;
        }

        /// <summary>
        /// All keys in subtrie rooted at x with given prefix
        /// </summary>
        /// <param name="x"></param>
        /// <param name="prefix"></param>
        /// <param name="queue"></param>
        private void Collect(Node<TValue> x, StringBuilder prefix, Queue<string> queue)
        {
            if (x == null)
            {
                return;
            }
            Collect(x.Left, prefix, queue);
            if (typeof(TValue).IsValueType || !Equals(x.Val, default(TValue)))
            {
                queue.Enqueue(prefix.ToString() + x.C);
            }
            Collect(x.Mid, prefix.Append(x.C), queue);
            prefix.Remove(prefix.Length - 1, 1);
            Collect(x.Right, prefix, queue);
        }

        /// <summary>
        /// Returns all of the keys in the symbol table that match pattern,
        /// where . symbol is treated as a wildcard character.
        /// </summary>
        /// <param name="pattern">the pattern</param>
        /// <returns>all of the keys in the symbol table that match pattern, as an iterable, where . is treated as a wildcard character.</returns>
        public IEnumerable<string> KeysThatMatch(string pattern)
        {
            Queue<string> queue = new Queue<string>();
            Collect(_root, new StringBuilder(), 0, pattern, queue);
            return queue;
        }

        private static void Collect(Node<TValue> x, StringBuilder prefix, int i, string pattern, Queue<string> queue)
        {
            if (x == null)
            {
                return;
            }
            char c = pattern[i];
            if (c == '.' || c < x.C)
            {
                Collect(x.Left, prefix, i, pattern, queue);
            }
            if (c == '.' || c == x.C)
            {
                if (i == pattern.Length - 1 && (typeof(TValue).IsValueType || !Equals(x.Val, default(TValue))))
                {
                    queue.Enqueue(prefix.ToString() + x.C);
                }
                if (i < pattern.Length - 1)
                {
                    Collect(x.Mid, prefix.Append(x.C), i + 1, pattern, queue);
                    prefix.Remove(prefix.Length - 1, 1);
                }
            }
            if (c == '.' || c > x.C)
            {
                Collect(x.Right, prefix, i, pattern, queue);
            }
        }

        /// <summary>
        /// Unit tests the TST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // build symbol table from standard input
            TST<int> st = new TST<int>();
            for (int i = 0; !StdIn.IsEmpty(); i++)
            {
                string key = StdIn.ReadString();
                st.Put(key, i);
            }

            // print results
            if (st.Size() < 100)
            {
                StdOut.PrintLn("keys(\"\"):");
                foreach (string key in st.Keys())
                {
                    StdOut.PrintLn(key + " " + st.Get(key));
                }
                StdOut.PrintLn();
            }

            StdOut.PrintLn("longestPrefixOf(\"shellsort\"):");
            StdOut.PrintLn(st.LongestPrefixOf("shellsort"));
            StdOut.PrintLn();

            StdOut.PrintLn("keysWithPrefix(\"shor\"):");
            foreach (string s in st.KeysWithPrefix("shor"))
            {
                StdOut.PrintLn(s);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("keysThatMatch(\".he.l.\"):");
            foreach (string s in st.KeysThatMatch(".he.l."))
            {
                StdOut.PrintLn(s);
            }
        }
    }
}