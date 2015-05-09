using System;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BinarySearchST<TKey, TValue> where TKey : IComparable<TKey>
    {
        private const int InitCapacity = 2;
        private TKey[] _keys;
        private TValue[] _vals;
        private int _n;

        /// <summary>
        /// Create an empty symbol table with default initial capacity
        /// </summary>
        public BinarySearchST()
            : this(InitCapacity)
        {
        }

        /// <summary>
        /// Create an empty symbol table with given initial capacity
        /// </summary>
        /// <param name="capacity"></param>
        public BinarySearchST(int capacity)
        {
            _n = 0;
            _keys = new TKey[capacity];
            _vals = new TValue[capacity];
        }

        /// <summary>
        /// Resize the underlying arrays
        /// </summary>
        /// <param name="capacity"></param>
        private void Resize(int capacity)
        {
            Debug.Assert(capacity >= _n);
            TKey[] tempk = new TKey[capacity];
            TValue[] tempv = new TValue[capacity];
            for (int i = 0; i < _n; i++)
            {
                tempk[i] = _keys[i];
                tempv[i] = _vals[i];
            }
            _vals = tempv;
            _keys = tempk;
        }

        /// <summary>
        /// Is the key in the table?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            try
            {
                Get(key);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Number of key-value pairs in the table
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
        /// Return the value associated with the given key, or null if no such key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            if (IsEmpty())
            {
                throw new KeyNotFoundException();
            }
            int i = Rank(key);
            if (i < _n && _keys[i].CompareTo(key) == 0)
            {
                return _vals[i];
            }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Return the number of keys in the table that are smaller than given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Rank(TKey key)
        {
            int lo = 0, hi = _n - 1;
            while (lo <= hi)
            {
                int m = lo + (hi - lo) / 2;
                int cmp = key.CompareTo(_keys[m]);
                if (cmp < 0)
                {
                    hi = m - 1;
                }
                else if (cmp > 0)
                {
                    lo = m + 1;
                }
                else
                {
                    return m;
                }
            }
            return lo;
        }

        /// <summary>
        /// Search for key. Update value if found; grow table if new. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Put(TKey key, TValue val)
        {
            if (!typeof(TValue).IsValueType && Equals(val, default(TValue)))
            {
                Delete(key);
                return;
            }

            int i = Rank(key);

            // key is already in table
            if (i < _n && _keys[i].CompareTo(key) == 0)
            {
                _vals[i] = val;
                return;
            }

            // insert new key-value pair
            if (_n == _keys.Length)
            {
                Resize(2 * _keys.Length);
            }

            for (int j = _n; j > i; j--)
            {
                _keys[j] = _keys[j - 1];
                _vals[j] = _vals[j - 1];
            }
            _keys[i] = key;
            _vals[i] = val;
            _n++;

            Debug.Assert(Check());
        }

        /// <summary>
        /// Remove the key-value pair if present
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TKey key)
        {
            if (IsEmpty())
            {
                return;
            }

            // compute rank
            int i = Rank(key);

            // key not in table
            if (i == _n || _keys[i].CompareTo(key) != 0)
            {
                return;
            }

            for (int j = i; j < _n - 1; j++)
            {
                _keys[j] = _keys[j + 1];
                _vals[j] = _vals[j + 1];
            }

            _n--;
            _keys[_n] = default(TKey); // to avoid loitering
            _vals[_n] = default(TValue);

            // resize if 1/4 full
            if (_n > 0 && _n == _keys.Length / 4)
            {
                Resize(_keys.Length / 2);
            }

            Debug.Assert(Check());
        }

        /// <summary>
        /// Delete the minimum key and its associated value
        /// </summary>
        public void DeleteMin()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Symbol table underflow error");
            }
            Delete(Min());
        }

        /// <summary>
        /// Delete the maximum key and its associated value
        /// </summary>
        public void DeleteMax()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Symbol table underflow error");
            }
            Delete(Max());
        }

        #region Ordered symbol table methods

        public TKey Min()
        {
            if (IsEmpty())
            {
                throw new KeyNotFoundException();
            }
            return _keys[0];
        }

        public TKey Max()
        {
            if (IsEmpty())
            {
                throw new KeyNotFoundException();
            }
            return _keys[_n - 1];
        }

        public TKey Select(int k)
        {
            if (k < 0 || k >= _n)
            {
                throw new IndexOutOfRangeException();
            }
            return _keys[k];
        }

        public TKey Floor(TKey key)
        {
            int i = Rank(key);
            if (i < _n && key.CompareTo(_keys[i]) == 0)
            {
                return _keys[i];
            }
            if (i == 0)
            {
                throw new KeyNotFoundException();
            }
            return _keys[i - 1];
        }

        public TKey Ceiling(TKey key)
        {
            int i = Rank(key);
            if (i == _n)
            {
                throw new KeyNotFoundException();
            }
            return _keys[i];
        }

        public int Size(TKey lo, TKey hi)
        {
            if (lo.CompareTo(hi) > 0)
            {
                return 0;
            }
            if (Contains(hi))
            {
                return Rank(hi) - Rank(lo) + 1;
            }
            return Rank(hi) - Rank(lo);
        }

        public IEnumerable<TKey> Keys()
        {
            return Keys(Min(), Max());
        }

        public IEnumerable<TKey> Keys(TKey lo, TKey hi)
        {
            Queue<TKey> queue = new Queue<TKey>();
            if (!typeof(TKey).IsValueType && Equals(lo, default(TKey)) && Equals(hi, default(TKey)))
            {
                return queue;
            }
            if (!typeof(TKey).IsValueType && Equals(lo, default(TKey)))
            {
                throw new NullReferenceException("lo is null in Keys()");
            }
            if (!typeof(TKey).IsValueType && Equals(hi, default(TKey)))
            {
                throw new NullReferenceException("hi is null in Keys()");
            }
            if (lo.CompareTo(hi) > 0)
            {
                return queue;
            }
            for (int i = Rank(lo); i < Rank(hi); i++)
            {
                queue.Enqueue(_keys[i]);
            }
            if (Contains(hi))
            {
                queue.Enqueue(_keys[Rank(hi)]);
            }
            return queue;
        }

        #endregion

        /// <summary>
        /// Check internal invariants
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            return IsSorted() && RankCheck();
        }

        /// <summary>
        /// Are the items in the array in ascending order?
        /// </summary>
        /// <returns></returns>
        private bool IsSorted()
        {
            for (int i = 1; i < Size(); i++)
            {
                if (_keys[i].CompareTo(_keys[i - 1]) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check that rank(select(i)) = i
        /// </summary>
        /// <returns></returns>
        private bool RankCheck()
        {
            for (int i = 0; i < Size(); i++)
            {
                if (i != Rank(Select(i)))
                {
                    return false;
                }
            }
            for (int i = 0; i < Size(); i++)
            {
                if (_keys[i].CompareTo(Select(Rank(_keys[i]))) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            BinarySearchST<string, int> st = new BinarySearchST<string, int>();
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