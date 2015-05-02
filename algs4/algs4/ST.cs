using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class ST<TKey, TValue> where TKey : IComparable
    {
        private readonly SortedSet<TKey> _st;

        private readonly Dictionary<TKey, TValue> _std;

        /// <summary>
        /// Initializes an empty symbol table.
        /// </summary>
        public ST()
        {
            _st = new SortedSet<TKey>();
            _std = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Returns the value associated with the given key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the value associated with the given key if the key is in the symbol table and <tt>null</tt> if the key is not in the symbol table</returns>
        public TValue Get(TKey key)
        {
            if (!typeof(TKey).IsValueType && Equals(default(TKey), key))
            {
                throw new NullReferenceException("called get() with null key");
            }
            return _std[key];
        }

        /// <summary>
        /// Inserts the key-value pair into the symbol table, overwriting the old value
        /// with the new value if the key is already in the symbol table.
        /// If the value is <tt>null</tt>, this effectively deletes the key from the symbol table.
        /// </summary>
        /// <param name="key">the key</param>
        /// <param name="val">the value</param>
        public void Put(TKey key, TValue val)
        {
            if (!typeof(TKey).IsValueType && Equals(default(TKey), key))
            {
                throw new NullReferenceException("called put() with null key");
            }
            if (!typeof(TValue).IsValueType && Equals(default(TValue), val))
            {
                _st.Remove(key);
                _std.Remove(key);
            }
            else
            {
                _st.Add(key);
                _std[key] = val;
            }
        }

        /// <summary>
        /// Removes the key and associated value from the symbol table
        /// (if the key is in the symbol table).
        /// </summary>
        /// <param name="key">the key</param>
        public void Delete(TKey key)
        {
            if (!typeof(TKey).IsValueType && Equals(default(TKey), key))
            {
                throw new NullReferenceException("called delete() with null key");
            }
            _st.Remove(key);
            _std.Remove(key);
        }

        /// <summary>
        /// Does this symbol table contain the given key?
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns><tt>true</tt> if this symbol table contains <tt>key</tt> and <tt>false</tt> otherwise</returns>
        public bool Contains(TKey key)
        {
            if (!typeof(TKey).IsValueType && Equals(default(TKey), key))
            {
                throw new NullReferenceException("called contains() with null key");
            }
            return _st.Contains(key);
        }

        /// <summary>
        /// Returns the number of key-value pairs in this symbol table.
        /// </summary>
        /// <returns>the number of key-value pairs in this symbol table</returns>
        public int Size()
        {
            return _st.Count;
        }

        /// <summary>
        /// Is this symbol table empty?
        /// </summary>
        /// <returns><tt>true</tt> if this symbol table is empty and <tt>false</tt> otherwise</returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Returns all keys in the symbol table as an <tt>Iterable</tt>.
        /// To iterate over all of the keys in the symbol table named <tt>st</tt>,
        /// use the foreach notation: <tt>for (Key key : st.keys())</tt>.
        /// </summary>
        /// <returns>all keys in the sybol table as an <tt>Iterable</tt></returns>
        public IEnumerable<TKey> Keys()
        {
            return _st;
        }

        /// <summary>
        /// Returns the smallest key in the symbol table.
        /// </summary>
        /// <returns>the smallest key in the symbol table</returns>
        public TKey Min()
        {
            if (IsEmpty())
            {
                throw new KeyNotFoundException("called min() with empty symbol table");
            }
            return _st.Min;
        }

        /// <summary>
        /// Returns the largest key in the symbol table.
        /// </summary>
        /// <returns>the largest key in the symbol table</returns>
        public TKey Max()
        {
            if (IsEmpty())
            {
                throw new KeyNotFoundException("called max() with empty symbol table");
            }
            return _st.Max;
        }

        /// <summary>
        /// Returns the smallest key in the symbol table greater than or equal to <tt>key</tt>.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the smallest key in the symbol table greater than or equal to <tt>key</tt></returns>
        public TKey Ceiling(TKey key)
        {
            if (!typeof(TKey).IsValueType && Equals(key, default(TKey)))
            {
                throw new NullReferenceException("called Ceiling() with a null key");
            }
            TKey k = _st.GetViewBetween(key, _st.Max).Min;
            if (!typeof(TKey).IsValueType && Equals(k, default(TKey)))
            {
                throw new InvalidOperationException("all keys are less than " + key);
            }
            return k;
        }

        /// <summary>
        /// Returns the largest key in the symbol table less than or equal to <tt>key</tt>.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the largest key in the symbol table less than or equal to <tt>key</tt></returns>
        public TKey Floor(TKey key)
        {
            if (!typeof(TKey).IsValueType && Equals(key, default(TKey)))
            {
                throw new NullReferenceException("called Floor() with a null key");
            }
            TKey k = _st.GetViewBetween(_st.Min, key).Max;
            if (!typeof(TKey).IsValueType && Equals(k, default(TKey)))
            {
                throw new InvalidOperationException("all keys are greater than " + key);
            }
            return k;
        }

        /// <summary>
        /// Unit tests the <tt>ST</tt> data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            ST<string, int> st = new ST<string, int>();
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
