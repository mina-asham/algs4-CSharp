using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class SeparateChainingHashST<TKey, TValue>
    {
        private const int InitCapacity = 4;

        // largest prime <= 2^i for i = 3 to 31
        // not currently used for doubling and shrinking
        // private static readonly int[] PRIMES = {
        //    7, 13, 31, 61, 127, 251, 509, 1021, 2039, 4093, 8191, 16381,
        //    32749, 65521, 131071, 262139, 524287, 1048573, 2097143, 4194301,
        //    8388593, 16777213, 33554393, 67108859, 134217689, 268435399,
        //    536870909, 1073741789, 2147483647
        // };

        /// <summary>
        /// Number of key-value pairs
        /// </summary>
        private int _n;

        /// <summary>
        /// Hash table size
        /// </summary>
        private int _m;

        /// <summary>
        /// Array of linked-list symbol tables
        /// </summary>
        private SequentialSearchST<TKey, TValue>[] _st;

        /// <summary>
        /// Create separate chaining hash table
        /// </summary>
        public SeparateChainingHashST()
            : this(InitCapacity)
        {
        }

        /// <summary>
        /// Create separate chaining hash table with M lists
        /// </summary>
        /// <param name="m"></param>
        public SeparateChainingHashST(int m)
        {
            _m = m;
            _st = new SequentialSearchST<TKey, TValue>[m];
            for (int i = 0; i < m; i++)
            {
                _st[i] = new SequentialSearchST<TKey, TValue>();
            }
        }

        /// <summary>
        /// Resize the hash table to have the given number of chains b rehashing all of the keys
        /// </summary>
        /// <param name="chains"></param>
        private void Resize(int chains)
        {
            SeparateChainingHashST<TKey, TValue> temp = new SeparateChainingHashST<TKey, TValue>(chains);
            for (int i = 0; i < _m; i++)
            {
                foreach (TKey key in _st[i].Keys())
                {
                    temp.Put(key, _st[i].Get(key));
                }
            }
            _m = temp._m;
            _n = temp._n;
            _st = temp._st;
        }

        /// <summary>
        /// Hash value between 0 and M-1
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int Hash(TKey key)
        {
            return (key.GetHashCode() & 0x7fffffff) % _m;
        }

        /// <summary>
        /// Return number of key-value pairs in symbol table
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Is the symbol table empty?
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Is the key in the symbol table?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            return Get(key) != null;
        }

        /// <summary>
        /// Return value associated with key, null if no such key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            int i = Hash(key);
            return _st[i].Get(key);
        }

        /// <summary>
        /// Insert key-value pair into the table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Put(TKey key, TValue val)
        {
            if (val == null)
            {
                Delete(key);
                return;
            }

            // double table size if average length of list >= 10
            if (_n >= 10 * _m)
            {
                Resize(2 * _m);
            }

            int i = Hash(key);
            if (!_st[i].Contains(key))
            {
                _n++;
            }
            _st[i].Put(key, val);
        }

        /// <summary>
        /// Delete key (and associated value) if key is in the table
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TKey key)
        {
            int i = Hash(key);
            if (_st[i].Contains(key))
            {
                _n--;
            }
            _st[i].Delete(key);

            // halve table size if average length of list <= 2
            if (_m > InitCapacity && _n <= 2 * _m)
            {
                Resize(_m / 2);
            }
        }

        /// <summary>
        /// Return keys in symbol table as an IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> Keys()
        {
            Queue<TKey> queue = new Queue<TKey>();
            for (int i = 0; i < _m; i++)
            {
                foreach (TKey key in _st[i].Keys())
                {
                    queue.Enqueue(key);
                }
            }
            return queue;
        }

        /// <summary>
        /// Unit test client.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            SeparateChainingHashST<string, int> st = new SeparateChainingHashST<string, int>();
            for (int i = 0; !StdIn.IsEmpty(); i++)
            {
                string key = StdIn.ReadString();
                st.Put(key, i);
            }

            // Print keys
            foreach (string s in st.Keys())
            {
                StdOut.PrintLn(s + " " + st.Get(s));
            }
        }
    }
}