using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class TrieST<TValue>
    {
        /// <summary>
        /// Extended ASCII
        /// </summary>
        private const int R = 256;

        /// <summary>
        /// Root of trie
        /// </summary>
        private Node _root;

        /// <summary>
        /// Number of keys in trie
        /// </summary>
        private int _n;

        /// <summary>
        /// R-way trie node
        /// </summary>
        private class Node
        {
            public TValue Val { get; set; }
            public readonly Node[] Next = new Node[R];
        }

        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the value associated with the given key if the key is in the symbol table and null if the key is not in the symbol table</returns>
        public TValue Get(string key)
        {
            Node x = Get(_root, key, 0);
            if (x == null)
            {
                return default(TValue);
            }
            return x.Val;
        }

        /// <summary>
        /// Does this symbol table contain the given key?
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if this symbol table contains key and false otherwise</returns>
        public bool Contains(string key)
        {
            return Get(key) != null;
        }

        private Node Get(Node x, string key, int d)
        {
            if (x == null)
            {
                return null;
            }
            if (d == key.Length)
            {
                return x;
            }
            char c = key[d];
            return Get(x.Next[c], key, d + 1);
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
            if (val == null)
            {
                Delete(key);
            }
            else
            {
                _root = Put(_root, key, val, 0);
            }
        }

        private Node Put(Node x, string key, TValue val, int d)
        {
            if (x == null)
            {
                x = new Node();
            }
            if (d == key.Length)
            {
                if (x.Val == null)
                {
                    _n++;
                }
                x.Val = val;
                return x;
            }
            char c = key[d];
            x.Next[c] = Put(x.Next[c], key, val, d + 1);
            return x;
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
        /// Is this symbol table empty?
        /// </summary>
        /// <returns>true if this symbol table is empty and false otherwise</returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Returns all keys in the symbol table as an IEnumerable.
        /// To iterate over all of the keys in the symbol table named st,
        /// use the foreach each notation: for (Key key  in  st.keys()).
        /// </summary>
        /// <returns>all keys in the sybol table as an IEnumerable</returns>
        public IEnumerable<string> Keys()
        {
            return KeysWithPrefix("");
        }

        /// <summary>
        /// Returns all of the keys in the set that start with prefix.
        /// </summary>
        /// <param name="prefix">the prefix</param>
        /// <returns>all of the keys in the set that start with prefix, as an iterable</returns>
        public IEnumerable<string> KeysWithPrefix(string prefix)
        {
            Queue<string> results = new Queue<string>();
            Node x = Get(_root, prefix, 0);
            collect(x, new StringBuilder(prefix), results);
            return results;
        }

        private void collect(Node x, StringBuilder prefix, Queue<string> results)
        {
            if (x == null)
            {
                return;
            }
            if (x.Val != null)
            {
                results.Enqueue(prefix.ToString());
            }
            for (char c = (char)0; c < R; c++)
            {
                prefix.Append(c);
                collect(x.Next[c], prefix, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns all of the keys in the symbol table that match pattern,
        /// where . symbol is treated as a wildcard character.
        /// the pattern
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>all of the keys in the symbol table that match pattern,as an iterable, where . is treated as a wildcard character.</returns>
        public IEnumerable<string> KeysThatMatch(string pattern)
        {
            Queue<string> results = new Queue<string>();
            collect(_root, new StringBuilder(), pattern, results);
            return results;
        }

        private void collect(Node x, StringBuilder prefix, string pattern, Queue<string> results)
        {
            if (x == null)
            {
                return;
            }
            int d = prefix.Length;
            if (d == pattern.Length && x.Val != null)
            {
                results.Enqueue(prefix.ToString());
            }
            if (d == pattern.Length)
            {
                return;
            }
            char c = pattern[d];
            if (c == '.')
            {
                for (char ch = (char)0; ch < R; ch++)
                {
                    prefix.Append(ch);
                    collect(x.Next[ch], prefix, pattern, results);
                    prefix.Remove(prefix.Length - 1, 1);
                }
            }
            else
            {
                prefix.Append(c);
                collect(x.Next[c], prefix, pattern, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns the string in the symbol table that is the longest prefix of query,
        /// or null, if no such string.
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>the string in the symbol table that is the longest prefix of query, or null if no such string</returns>
        public string LongestPrefixOf(string query)
        {
            int length = LongestPrefixOf(_root, query, 0, 0);
            return query.Substring(0, length);
        }

        /// <summary>
        /// Returns the length of the longest string key in the subtrie
        /// rooted at x that is a prefix of the query string,
        /// assuming the first d character match and we have already
        /// found a prefix match of length length
        /// </summary>
        /// <param name="x"></param>
        /// <param name="query"></param>
        /// <param name="d"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private int LongestPrefixOf(Node x, string query, int d, int length)
        {
            if (x == null)
            {
                return length;
            }
            if (x.Val != null)
            {
                length = d;
            }
            if (d == query.Length)
            {
                return length;
            }
            char c = query[d];
            return LongestPrefixOf(x.Next[c], query, d + 1, length);
        }

        /// <summary>
        /// Removes the key from the set if the key is present.
        /// </summary>
        /// <param name="key">the key</param>
        public void Delete(string key)
        {
            _root = Delete(_root, key, 0);
        }

        private Node Delete(Node x, string key, int d)
        {
            if (x == null)
            {
                return null;
            }
            if (d == key.Length)
            {
                if (x.Val != null)
                {
                    _n--;
                }
                x.Val = default(TValue);
            }
            else
            {
                char c = key[d];
                x.Next[c] = Delete(x.Next[c], key, d + 1);
            }

            // remove subtrie rooted at x if it is completely empty
            if (x.Val != null)
            {
                return x;
            }
            for (int c = 0; c < R; c++)
            {
                if (x.Next[c] != null)
                {
                    return x;
                }
            }
            return null;
        }

        /// <summary>
        /// Unit tests the TrieST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // build symbol table from standard input
            TrieST<int> st = new TrieST<int>();
            for (int i = 0; !StdIn.IsEmpty(); i++)
            {
                string key = StdIn.ReadString();
                st.Put(key, i);
            }

            // Print results
            if (st.Size() < 100)
            {
                StdOut.PrintLn("keys(\"\"):");
                foreach (string key in st.Keys())
                {
                    StdOut.PrintLn(key + " " + st.Get(key));
                }
                StdOut.PrintLn();
            }

            StdOut.PrintLn("LongestPrefixOf(\"shellsort\"):");
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