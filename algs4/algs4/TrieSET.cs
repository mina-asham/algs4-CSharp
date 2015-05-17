using System.Collections;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class TrieSET : IEnumerable<string>
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
        /// RR-way trie node
        /// </summary>
        private class Node
        {
            private readonly Node[] _next = new Node[R];

            public Node[] Next
            {
                get { return _next; }
            }

            public bool IsString { get; set; }
        }

        /// <summary>
        /// Does the set contain the given key?
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if the set contains key and false otherwise</returns>
        public bool Contains(string key)
        {
            Node x = Get(_root, key, 0);
            if (x == null)
            {
                return false;
            }
            return x.IsString;
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
        /// Adds the key to the set if it is not already present.
        /// </summary>
        /// <param name="key">the key to Add</param>
        public void Add(string key)
        {
            _root = Add(_root, key, 0);
        }

        private Node Add(Node x, string key, int d)
        {
            if (x == null)
            {
                x = new Node();
            }
            if (d == key.Length)
            {
                if (!x.IsString)
                {
                    _n++;
                }
                x.IsString = true;
            }
            else
            {
                char c = key[d];
                x.Next[c] = Add(x.Next[c], key, d + 1);
            }
            return x;
        }

        /// <summary>
        /// Returns the number of strings in the set.
        /// </summary>
        /// <returns>the number of strings in the set</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Is the set empty?
        /// </summary>
        /// <returns>true if the set is empty, and false otherwise</returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Returns all of the keys in the set, as an iterator.
        /// To iterate over all of the keys in a set named set, use the
        /// foreach each notation: for (Key key  in  set).
        /// </summary>
        /// <returns>an iterator to all of the keys in the set</returns>
        public IEnumerator<string> GetEnumerator()
        {
            return KeysWithPrefix("").GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            Collect(x, new StringBuilder(prefix), results);
            return results;
        }

        private static void Collect(Node x, StringBuilder prefix, Queue<string> results)
        {
            if (x == null)
            {
                return;
            }
            if (x.IsString)
            {
                results.Enqueue(prefix.ToString());
            }
            for (char c = (char)0; c < R; c++)
            {
                prefix.Append(c);
                Collect(x.Next[c], prefix, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns all of the keys in the set that match pattern,
        /// where . symbol is treated as a wildcard character.
        /// </summary>
        /// <param name="pattern">the pattern</param>
        /// <returns>all of the keys in the set that match pattern, as an iterable, where . is treated as a wildcard character.</returns>
        public IEnumerable<string> KeysThatMatch(string pattern)
        {
            Queue<string> results = new Queue<string>();
            StringBuilder prefix = new StringBuilder();
            Collect(_root, prefix, pattern, results);
            return results;
        }

        private void Collect(Node x, StringBuilder prefix, string pattern, Queue<string> results)
        {
            if (x == null)
            {
                return;
            }
            int d = prefix.Length;
            if (d == pattern.Length && x.IsString)
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
                    Collect(x.Next[ch], prefix, pattern, results);
                    prefix.Remove(prefix.Length - 1, 1);
                }
            }
            else
            {
                prefix.Append(c);
                Collect(x.Next[c], prefix, pattern, results);
                prefix.Remove(prefix.Length - 1, 1);
            }
        }

        /// <summary>
        /// Returns the string in the set that is the longest prefix of query,
        /// or null, if no such string.
        /// </summary>
        /// <param name="query">the query string</param>
        /// <returns>the string in the set that is the longest prefix of query, or null if no such string</returns>
        public string LongestPrefixOf(string query)
        {
            int length = LongestPrefixOf(_root, query, 0, -1);
            if (length == -1)
            {
                return null;
            }
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
            if (x.IsString)
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
                if (x.IsString)
                {
                    _n--;
                }
                x.IsString = false;
            }
            else
            {
                char c = key[d];
                x.Next[c] = Delete(x.Next[c], key, d + 1);
            }

            // remove subtrie rooted at x if it is completely empty
            if (x.IsString)
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
        /// Unit tests the TrieSET data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            TrieSET set = new TrieSET();
            while (!StdIn.IsEmpty())
            {
                string key = StdIn.ReadString();
                set.Add(key);
            }

            // Print results
            if (set.Size() < 100)
            {
                StdOut.PrintLn("keys(\"\"):");
                foreach (string key in set)
                {
                    StdOut.PrintLn(key);
                }
                StdOut.PrintLn();
            }

            StdOut.PrintLn("LongestPrefixOf(\"shellsort\"):");
            StdOut.PrintLn(set.LongestPrefixOf("shellsort"));
            StdOut.PrintLn();

            StdOut.PrintLn("LongestPrefixOf(\"xshellsort\"):");
            StdOut.PrintLn(set.LongestPrefixOf("xshellsort"));
            StdOut.PrintLn();

            StdOut.PrintLn("keysWithPrefix(\"shor\"):");
            foreach (string s in set.KeysWithPrefix("shor"))
            {
                StdOut.PrintLn(s);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("keysWithPrefix(\"shortening\"):");
            foreach (string s in set.KeysWithPrefix("shortening"))
            {
                StdOut.PrintLn(s);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("keysThatMatch(\".he.l.\"):");
            foreach (string s in set.KeysThatMatch(".he.l."))
            {
                StdOut.PrintLn(s);
            }
        }
    }
}