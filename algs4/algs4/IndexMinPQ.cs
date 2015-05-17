using System;
using System.Collections;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class IndexMinPQ<TKey> : IEnumerable<int> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Maximum number of elements on PQ
        /// </summary>
        private readonly int _nmax;

        /// <summary>
        /// Number of elements on PQ
        /// </summary>
        private int _n;

        /// <summary>
        /// Binary heap using 1-based indexing
        /// </summary>
        private readonly int[] _pq;

        /// <summary>
        /// Inverse of pq - qp[pq[i]] = pq[qp[i]] = i
        /// </summary>
        private readonly int[] _qp;

        /// <summary>
        /// keys[i] = priority of i
        /// </summary>
        private readonly TKey[] _keys;

        /// <summary>
        /// Initializes an empty indexed priority queue with indices between 0 and NMAX-1.
        /// </summary>
        /// <param name="nmax">the keys on the priority queue are index from 0 to nmax-1</param>
        public IndexMinPQ(int nmax)
        {
            if (nmax < 0)
            {
                throw new ArgumentException();
            }
            _nmax = nmax;
            _keys = new TKey[nmax + 1]; // make this of length NMAX??
            _pq = new int[nmax + 1];
            _qp = new int[nmax + 1]; // make this of length NMAX??
            for (int i = 0; i <= nmax; i++)
            {
                _qp[i] = -1;
            }
        }

        /// <summary>
        /// Is the priority queue empty?
        /// </summary>
        /// <returns>true if the priority queue is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _n == 0;
        }

        /// <summary>
        /// Is i an index on the priority queue?
        /// </summary>
        /// <param name="i">an index</param>
        /// <returns></returns>
        public bool Contains(int i)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            return _qp[i] != -1;
        }

        /// <summary>
        /// Returns the number of keys on the priority queue.
        /// </summary>
        /// <returns>the number of keys on the priority queue</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Associates key with index i.
        /// </summary>
        /// <param name="i">an index</param>
        /// <param name="key">the key to associate with index i</param>
        public void Insert(int i, TKey key)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            if (Contains(i))
            {
                throw new ArgumentException("index is already in the priority queue");
            }
            _n++;
            _qp[i] = _n;
            _pq[_n] = i;
            _keys[i] = key;
            Swim(_n);
        }

        /// <summary>
        /// Returns an index associated with a minimum key.
        /// </summary>
        /// <returns>an index associated with a minimum key</returns>
        public int MinIndex()
        {
            if (_n == 0)
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            return _pq[1];
        }

        /// <summary>
        /// Returns a minimum key.
        /// </summary>
        /// <returns>a minimum key</returns>
        public TKey MinKey()
        {
            if (_n == 0)
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            return _keys[_pq[1]];
        }

        /// <summary>
        /// Removes a minimum key and returns its associated index.
        /// </summary>
        /// <returns>an index associated with a minimum key</returns>
        public int DelMin()
        {
            if (_n == 0)
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            int min = _pq[1];
            Exch(1, _n--);
            Sink(1);
            _qp[min] = -1; // delete
            _keys[_pq[_n + 1]] = default(TKey); // to help with garbage collection
            _pq[_n + 1] = -1; // not needed
            return min;
        }

        /// <summary>
        /// Returns the key associated with index i.
        /// </summary>
        /// <param name="i">the index of the key to return</param>
        /// <returns>the key associated with index i</returns>
        public TKey KeyOf(int i)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            return _keys[i];
        }

        /// <summary>
        /// Change the key associated with index i to the specified value.
        /// </summary>
        /// <param name="i">the index of the key to change</param>
        /// <param name="key">change the key assocated with index i to this key</param>
        [Obsolete("Replaced by ChangeKey")]
        public void Change(int i, TKey key)
        {
            ChangeKey(i, key);
        }

        /// <summary>
        /// Change the key associated with index i to the specified value.
        /// </summary>
        /// <param name="i">the index of the key to change</param>
        /// <param name="key">change the key assocated with index i to this key</param>
        public void ChangeKey(int i, TKey key)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            _keys[i] = key;
            Swim(_qp[i]);
            Sink(_qp[i]);
        }

        /// <summary>
        /// Decrease the key associated with index i to the specified value.
        /// </summary>
        /// <param name="i">the index of the key to decrease</param>
        /// <param name="key">decrease the key assocated with index i to this key</param>
        public void DecreaseKey(int i, TKey key)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            if (_keys[i].CompareTo(key) <= 0)
            {
                throw new ArgumentException("Calling decreaseKey() with given argument would not strictly decrease the key");
            }
            _keys[i] = key;
            Swim(_qp[i]);
        }

        /// <summary>
        /// Increase the key associated with index i to the specified value.
        /// </summary>
        /// <param name="i">the index of the key to increase</param>
        /// <param name="key">increase the key assocated with index i to this key</param>
        public void IncreaseKey(int i, TKey key)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            if (_keys[i].CompareTo(key) >= 0)
            {
                throw new ArgumentException("Calling increaseKey() with given argument would not strictly increase the key");
            }
            _keys[i] = key;
            Sink(_qp[i]);
        }

        /// <summary>
        /// Remove the key associated with index i.
        /// </summary>
        /// <param name="i">the index of the key to remove</param>
        public void Delete(int i)
        {
            if (i < 0 || i >= _nmax)
            {
                throw new IndexOutOfRangeException();
            }
            if (!Contains(i))
            {
                throw new InvalidOperationException("index is not in the priority queue");
            }
            int index = _qp[i];
            Exch(index, _n--);
            Swim(index);
            Sink(index);
            _keys[i] = default(TKey);
            _qp[i] = -1;
        }

        #region General helper functions

        private bool Greater(int i, int j)
        {
            return _keys[_pq[i]].CompareTo(_keys[_pq[j]]) > 0;
        }

        private void Exch(int i, int j)
        {
            int swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
            _qp[_pq[i]] = i;
            _qp[_pq[j]] = j;
        }

        #endregion

        #region Heap helper functions

        private void Swim(int k)
        {
            while (k > 1 && Greater(k / 2, k))
            {
                Exch(k, k / 2);
                k = k / 2;
            }
        }

        private void Sink(int k)
        {
            while (2 * k <= _n)
            {
                int j = 2 * k;
                if (j < _n && Greater(j, j + 1))
                {
                    j++;
                }
                if (!Greater(k, j))
                {
                    break;
                }
                Exch(k, j);
                k = j;
            }
        }

        #endregion

        #region Iterators

        /// <summary>
        /// Returns an iterator that iterates over the keys on the
        /// priority queue in ascending order.
        /// </summary>
        /// <returns>an iterator that iterates over the keys in ascending order</returns>
        public IEnumerator<int> GetEnumerator()
        {
            return new HeapIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class HeapIterator : IEnumerator<int>
        {
            /// <summary>
            /// Create a new pq
            /// </summary>
            private IndexMinPQ<TKey> _copy;

            private IndexMinPQ<TKey> _original;

            public int Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Add all elements to copy of heap
            /// takes linear time since already in heap order so no keys move
            /// </summary>
            /// <param name="indexMinPQ"></param>
            public HeapIterator(IndexMinPQ<TKey> indexMinPQ)
            {
                _original = indexMinPQ;
                Reset();
            }

            public void Dispose()
            {
                _original = null;
                _copy = null;
            }

            public bool MoveNext()
            {
                bool hasNext = !_copy.IsEmpty();
                if (hasNext)
                {
                    Current = _copy.DelMin();
                }
                return hasNext;
            }

            public void Reset()
            {
                _copy = new IndexMinPQ<TKey>(_original._pq.Length - 1);
                for (int i = 1; i <= _original._n; i++)
                {
                    _copy.Insert(_original._pq[i], _original._keys[_original._pq[i]]);
                }
            }
        }

        #endregion

        /// <summary>
        /// Unit tests the IndexMinPQ data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // insert a bunch of strings
            string[] strings = { "it", "was", "the", "best", "of", "times", "it", "was", "the", "worst" };

            IndexMinPQ<string> pq = new IndexMinPQ<string>(strings.Length);
            for (int i = 0; i < strings.Length; i++)
            {
                pq.Insert(i, strings[i]);
            }

            // delete and print each key
            while (!pq.IsEmpty())
            {
                int i = pq.DelMin();
                StdOut.PrintLn(i + " " + strings[i]);
            }
            StdOut.PrintLn();

            // reinsert the same strings
            for (int i = 0; i < strings.Length; i++)
            {
                pq.Insert(i, strings[i]);
            }

            // print each key using the iterator
            foreach (int i in pq)
            {
                StdOut.PrintLn(i + " " + strings[i]);
            }
            while (!pq.IsEmpty())
            {
                pq.DelMin();
            }
        }
    }
}