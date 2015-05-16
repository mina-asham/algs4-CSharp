using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LinearProbingHashST<TKey, TValue>
    {
        private const int InitCapacity = 4;

        /// <summary>
        /// Number of key-value pairs in the symbol table
        /// </summary>
        private int _n;

        /// <summary>
        /// Size of linear probing table
        /// </summary>
        private int _m;

        /// <summary>
        /// The keys
        /// </summary>
        private TKey[] _keys;

        /// <summary>
        /// The values
        /// </summary>
        private TValue[] _vals;

        /// <summary>
        /// Create an empty hash table - use 16 as default size
        /// </summary>
        public LinearProbingHashST()
            : this(InitCapacity)
        {
        }

        /// <summary>
        /// Create linear proving hash table of given capacity
        /// </summary>
        /// <param name="capacity"></param>
        public LinearProbingHashST(int capacity)
        {
            _m = capacity;
            _keys = new TKey[_m];
            _vals = new TValue[_m];
        }

        /// <summary>
        /// Return the number of key-value pairs in the symbol table
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
        /// Does a key-value pair with the given key exist in the symbol table?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            bool exists;
            Get(key, out exists);
            return exists;
        }

        /// <summary>
        /// Hash function for keys - returns value between 0 and M-1
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int Hash(TKey key)
        {
            return (key.GetHashCode() & 0x7fffffff) % _m;
        }

        /// <summary>
        /// Resize the hash table to the given capacity by re-hashing all of the keys
        /// </summary>
        /// <param name="capacity"></param>
        private void Resize(int capacity)
        {
            LinearProbingHashST<TKey, TValue> temp = new LinearProbingHashST<TKey, TValue>(capacity);
            for (int i = 0; i < _m; i++)
            {
                if (_keys[i] != null)
                {
                    temp.Put(_keys[i], _vals[i]);
                }
            }
            _keys = temp._keys;
            _vals = temp._vals;
            _m = temp._m;
        }

        /// <summary>
        /// Insert the key-value pair into the symbol table
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

            // double table size if 50% full
            if (_n >= _m / 2)
            {
                Resize(2 * _m);
            }

            int i;
            for (i = Hash(key); _keys[i] != null; i = (i + 1) % _m)
            {
                if (_keys[i].Equals(key))
                {
                    _vals[i] = val;
                    return;
                }
            }
            _keys[i] = key;
            _vals[i] = val;
            _n++;
        }

        /// <summary>
        /// Return the value associated with the given key, null if no such value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            bool exists;
            return Get(key, out exists);
        }

        public TValue Get(TKey key, out bool exists)
        {
            for (int i = Hash(key); _keys[i] != null; i = (i + 1) % _m)
            {
                if (_keys[i].Equals(key))
                {
                    exists = true;
                    return _vals[i];
                }
            }
            exists = false;
            return default(TValue);
        }

        /// <summary>
        /// Delete the key (and associated value) from the symbol table
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TKey key)
        {
            if (!Contains(key))
            {
                return;
            }

            // find position i of key
            int i = Hash(key);
            while (!key.Equals(_keys[i]))
            {
                i = (i + 1) % _m;
            }

            // delete key and associated value
            _keys[i] = default(TKey);
            _vals[i] = default(TValue);

            // rehash all keys in same cluster
            i = (i + 1) % _m;
            while (_keys[i] != null)
            {
                // delete keys[i] an vals[i] and reinsert
                TKey keyToRehash = _keys[i];
                TValue valToRehash = _vals[i];
                _keys[i] = default(TKey);
                _vals[i] = default(TValue);
                _n--;
                Put(keyToRehash, valToRehash);
                i = (i + 1) % _m;
            }

            _n--;

            // halves size of array if it's 12.5% full or less
            if (_n > 0 && _n <= _m / 8)
            {
                Resize(_m / 2);
            }

            Debug.Assert(Check());
        }

        /// <summary>
        /// Return all of the keys as in IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> Keys()
        {
            Queue<TKey> queue = new Queue<TKey>();
            for (int i = 0; i < _m; i++)
            {
                if (_keys[i] != null)
                {
                    queue.Enqueue(_keys[i]);
                }
            }
            return queue;
        }

        /// <summary>
        /// Integrity check - don't check after each put() because
        /// integrity not maintained during a delete()
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            // check that hash table is at most 50% full
            if (_m < 2 * _n)
            {
                Console.Error.WriteLine("Hash table size M = " + _m + "; array size N = " + _n);
                return false;
            }

            // check that each key in table can be found by get()
            for (int i = 0; i < _m; i++)
            {
                if (_keys[i] == null)
                {
                    continue;
                }
                if (!Get(_keys[i]).Equals(_vals[i]))
                {
                    Console.Error.WriteLine("get[" + _keys[i] + "] = " + Get(_keys[i]) + "; vals[i] = " + _vals[i]);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Unit test client.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            LinearProbingHashST<string, int> st = new LinearProbingHashST<string, int>();
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