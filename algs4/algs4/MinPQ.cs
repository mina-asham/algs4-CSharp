using System;
using System.Collections;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class MinPQ<TKey> : IEnumerable<TKey> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Store items at indices 1 to N
        /// </summary>
        private TKey[] _pq;

        /// <summary>
        /// Number of items on priority queue
        /// </summary>
        private int _n;

        /// <summary>
        /// Optional comparator
        /// </summary>
        private readonly Comparer<TKey> _comparator;

        /// <summary>
        /// Initializes an empty priority queue with the given initial capacity.
        /// </summary>
        /// <param name="initCapacity">the initial capacity of the priority queue</param>
        public MinPQ(int initCapacity)
        {
            _pq = new TKey[initCapacity + 1];
            _n = 0;
        }

        /// <summary>
        /// Initializes an empty priority queue.
        /// </summary>
        public MinPQ()
            : this(1)
        {
        }

        /// <summary>
        /// Initializes an empty priority queue with the given initial capacity,
        /// using the given comparator.
        /// </summary>
        /// <param name="initCapacity">the initial capacity of the priority queue</param>
        /// <param name="comparator">the order to use when comparing keys</param>
        public MinPQ(int initCapacity, Comparer<TKey> comparator)
        {
            _comparator = comparator;
            _pq = new TKey[initCapacity + 1];
            _n = 0;
        }

        /// <summary>
        /// Initializes an empty priority queue using the given comparator.
        /// </summary>
        /// <param name="comparator">the order to use when comparing keys</param>
        public MinPQ(Comparer<TKey> comparator) : this(1, comparator) { }

        /// <summary>
        /// Initializes a priority queue from the array of keys.
        /// Takes time proportional to the number of keys, using sink-based heap construction.
        /// </summary>
        /// <param name="keys">the array of keys</param>
        public MinPQ(TKey[] keys)
        {
            _n = keys.Length;
            _pq = new TKey[keys.Length + 1];
            for (int i = 0; i < _n; i++)
            {
                _pq[i + 1] = keys[i];
            }
            for (int k = _n / 2; k >= 1; k--)
            {
                Sink(k);
            }
        }

        /// <summary>
        /// Is the priority queue empty?
        /// </summary>
        /// <returns>if the priority queue is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _n == 0;
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
        /// Returns a smallest key on the priority queue.
        /// </summary>
        /// <returns>a smallest key on the priority queue</returns>
        public TKey Min()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            return _pq[1];
        }

        /// <summary>
        /// Helper function to double the size of the heap array
        /// </summary>
        /// <param name="capacity"></param>
        private void Resize(int capacity)
        {
            TKey[] temp = new TKey[capacity];
            for (int i = 1; i <= _n; i++)
            {
                temp[i] = _pq[i];
            }
            _pq = temp;
        }

        /// <summary>
        /// Adds a new key to the priority queue.
        /// </summary>
        /// <param name="x">the key to add to the priority queue</param>
        public void Insert(TKey x)
        {
            // double size of array if necessary
            if (_n == _pq.Length - 1)
            {
                Resize(2 * _pq.Length);
            }

            // add x, and percolate it up to maintain heap invariant
            _pq[++_n] = x;
            Swim(_n);
        }

        /// <summary>
        /// Removes and returns a smallest key on the priority queue.
        /// </summary>
        /// <returns>a smallest key on the priority queue</returns>
        public TKey DelMin()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            Exch(1, _n);
            TKey min = _pq[_n--];
            Sink(1);

            // avoid loitering and help with garbage collection
            _pq[_n + 1] = default(TKey);

            if ((_n > 0) && (_n == (_pq.Length - 1) / 4))
            {
                Resize(_pq.Length / 2);
            }
            return min;
        }


        #region Helper functions to restore the heap invariant.

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
        #region Helper functions for compares and swaps.
        private bool Greater(int i, int j)
        {
            if (_comparator == null)
            {
                return _pq[i].CompareTo(_pq[j]) > 0;
            }
            return _comparator.Compare(_pq[i], _pq[j]) > 0;
        }

        private void Exch(int i, int j)
        {
            TKey swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
        }

        /// <summary>
        /// Is pq[1..N] a min heap?
        /// </summary>
        /// <returns></returns>
        public bool IsMinHeap()
        {
            return IsMinHeap(1);
        }

        /// <summary>
        /// Is subtree of pq[1..N] rooted at k a min heap?
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool IsMinHeap(int k)
        {
            if (k > _n) return true;
            int left = 2 * k, right = 2 * k + 1;
            if (left <= _n && Greater(k, left)) return false;
            if (right <= _n && Greater(k, right)) return false;
            return IsMinHeap(left) && IsMinHeap(right);
        }

        #endregion

        #region Iterators

        /// <summary>
        /// Returns an iterator that iterates over the keys on the priority queue
        /// in ascending order.
        /// </summary>
        /// <returns>an iterator that iterates over the keys in ascending order</returns>
        public IEnumerator<TKey> GetEnumerator()
        {
            return new HeapIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class HeapIterator : IEnumerator<TKey>
        {
            /// <summary>
            /// Create a new pq
            /// </summary>
            private MinPQ<TKey> _copy;
            private MinPQ<TKey> _minPq;

            public TKey Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Add all items to copy of heap
            /// takes linear time since already in heap order so no keys move
            /// </summary>
            /// <param name="minPq"></param>
            public HeapIterator(MinPQ<TKey> minPq)
            {
                _minPq = minPq;
                Reset();
            }

            public bool MoveNext()
            {
                bool hasNext = _copy.IsEmpty();
                if (hasNext)
                {
                    Current = _copy.DelMin();
                }
                return hasNext;
            }

            public void Reset()
            {
                if (_minPq._comparator == null)
                {
                    _copy = new MinPQ<TKey>(_minPq.Size());
                }
                else
                {
                    _copy = new MinPQ<TKey>(_minPq.Size(), _minPq._comparator);
                }
                for (int i = 1; i <= _minPq._n; i++)
                {
                    _copy.Insert(_minPq._pq[i]);
                }
            }

            public void Dispose()
            {
                _copy = null;
                _minPq = null;
            }
        }
        #endregion

        /// <summary>
        /// Unit tests the MinPQ data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            MinPQ<string> pq = new MinPQ<string>();
            while (!StdIn.IsEmpty())
            {
                string item = StdIn.ReadString();
                if (item != "-")
                {
                    pq.Insert(item);
                }
                else if (!pq.IsEmpty())
                {
                    StdOut.Print(pq.DelMin() + " ");
                }
            }
            StdOut.PrintLn("(" + pq.Size() + " left on pq)");
        }
    }
}
