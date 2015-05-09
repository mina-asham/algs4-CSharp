using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class SequentialSearchST<TKey, TValue>
    {
        /// <summary>
        /// Number of key-value pairs
        /// </summary>
        private int _n;

        /// <summary>
        /// The linked list of key-value pairs
        /// </summary>
        private Node _first;

        /// <summary>
        /// A helper linked list data type
        /// </summary>
        private class Node
        {
            public TKey Key { get; private set; }

            public TValue Val { get; set; }

            public Node Next { get; set; }

            public Node(TKey key, TValue val, Node next)
            {
                Key = key;
                Val = val;
                Next = next;
            }
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
        /// Does this symbol table contain the given key?
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if this symbol table contains key and false otherwise</returns>
        public bool Contains(TKey key)
        {
            for (Node x = _first; x != null; x = x.Next)
            {
                if (key.Equals(x.Key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the value associated with the given key if the key is in the symbol table and null if the key is not in the symbol table</returns>
        public TValue Get(TKey key)
        {
            for (Node x = _first; x != null; x = x.Next)
            {
                if (key.Equals(x.Key))
                {
                    return x.Val;
                }
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Inserts the key-value pair into the symbol table, overwriting the old value
        /// with the new value if the key is already in the symbol table.
        /// If the value is null, this effectively deletes the key from the symbol table.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="val">the value</param>
        public void Put(TKey key, TValue val)
        {
            if (!typeof(TValue).IsValueType && Equals(val, default(TValue)))
            {
                Delete(key);
                return;
            }
            for (Node x = _first; x != null; x = x.Next)
            {
                if (key.Equals(x.Key))
                {
                    x.Val = val;
                    return;
                }
            }
            _first = new Node(key, val, _first);
            _n++;
        }

        /// <summary>
        /// Removes the key and associated value from the symbol table
        /// (if the key is in the symbol table).
        /// </summary>
        /// <param name="key">the key</param>
        public void Delete(TKey key)
        {
            _first = Delete(_first, key);
        }

        /// <summary>
        /// delete key in linked list beginning at Node x
        /// warning: function call stack too large if table is large
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node Delete(Node x, TKey key)
        {
            if (x == null)
            {
                return null;
            }
            if (key.Equals(x.Key))
            {
                _n--;
                return x.Next;
            }
            x.Next = Delete(x.Next, key);
            return x;
        }

        /// <summary>
        /// Returns all keys in the symbol table as an Iterable.
        /// To iterate over all of the keys in the symbol table named st,
        /// use the foreach notation: for (Key key : st.keys()).
        /// </summary>
        /// <returns>all keys in the sybol table as an Iterable</returns>
        public IEnumerable<TKey> Keys()
        {
            Queue<TKey> queue = new Queue<TKey>();
            for (Node x = _first; x != null; x = x.Next)
            {
                queue.Enqueue(x.Key);
            }
            return queue;
        }

        /// <summary>
        /// Unit tests the SequentialSearchST data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            SequentialSearchST<string, int> st = new SequentialSearchST<string, int>();
            for (int i = 0; !StdIn.IsEmpty(); i++)
            {
                string key = StdIn.ReadString();
                st.Put(key, i);
            }
            foreach (string s in st.Keys())
            {
                StdOut.PrintLn(s + " " + st.Get(s));
            }
        }
    }
}