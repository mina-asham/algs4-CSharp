using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class MaxPQ<TKey> : IEnumerable<TKey> where TKey : IComparable<TKey>
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
        /// Optional Comparator
        /// </summary>
        private readonly IComparer<TKey> _comparator;

        /// <summary>
        /// Initializes an empty priority queue with the given initial capacity.
        /// </summary>
        /// <param name="initCapacity">the initial capacity of the priority queue</param>
        public MaxPQ(int initCapacity)
        {
            _pq = new TKey[initCapacity + 1];
            _n = 0;
        }

        /// <summary>
        /// Initializes an empty priority queue.
        /// </summary>
        public MaxPQ()
            : this(1)
        {
        }

        /// <summary>
        /// Initializes an empty priority queue with the given initial capacity,
        /// using the given comparator.
        /// </summary>
        /// <param name="initCapacity">the initial capacity of the priority queue</param>
        /// <param name="comparator">the order in which to compare the keys</param>
        public MaxPQ(int initCapacity, IComparer<TKey> comparator)
        {
            _comparator = comparator;
            _pq = new TKey[initCapacity + 1];
            _n = 0;
        }

        /// <summary>
        /// Initializes an empty priority queue using the given comparator.
        /// </summary>
        /// <param name="comparator">the order in which to compare the keys</param>
        public MaxPQ(IComparer<TKey> comparator)
            : this(1, comparator)
        {
        }

        /// <summary>
        /// Initializes a priority queue from the array of keys.
        /// Takes time proportional to the number of keys, using sink-based heap construction.
        /// </summary>
        /// <param name="keys">the array of keys</param>
        public MaxPQ(TKey[] keys)
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
            Debug.Assert(IsMaxHeap());
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
        /// Returns the number of keys on the priority queue.
        /// </summary>
        /// <returns>the number of keys on the priority queue</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Returns a largest key on the priority queue.
        /// </summary>
        /// <returns>a largest key on the priority queue</returns>
        public TKey Max()
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
            Debug.Assert(capacity > _n);
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
        /// <param name="x">the new key to add to the priority queue</param>
        public void Insert(TKey x)
        {
            // double size of array if necessary
            if (_n >= _pq.Length - 1)
            {
                Resize(2 * _pq.Length);
            }

            // add x, and percolate it up to maintain heap invariant
            _pq[++_n] = x;
            Swim(_n);
            Debug.Assert(IsMaxHeap());
        }

        /// <summary>
        /// Removes and returns a largest key on the priority queue.
        /// </summary>
        /// <returns>a largest key on the priority queue</returns>
        public TKey DelMax()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Priority queue underflow");
            }
            TKey max = _pq[1];
            Exch(1, _n--);
            Sink(1);
            _pq[_n + 1] = default(TKey); // to avoid loiterig and help with garbage collection
            if ((_n > 0) && (_n == (_pq.Length - 1) / 4))
            {
                Resize(_pq.Length / 2);
            }
            Debug.Assert(IsMaxHeap());
            return max;
        }

        #region Helper functions to restore the heap invariant.

        private void Swim(int k)
        {
            while (k > 1 && Less(k / 2, k))
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
                if (j < _n && Less(j, j + 1))
                {
                    j++;
                }
                if (!Less(k, j))
                {
                    break;
                }
                Exch(k, j);
                k = j;
            }
        }

        #endregion

        #region Helper functions for compares and swaps.

        private bool Less(int i, int j)
        {
            if (_comparator == null)
            {
                return _pq[i].CompareTo(_pq[j]) < 0;
            }
            else
            {
                return _comparator.Compare(_pq[i], _pq[j]) < 0;
            }
        }

        private void Exch(int i, int j)
        {
            TKey swap = _pq[i];
            _pq[i] = _pq[j];
            _pq[j] = swap;
        }

        #endregion

        /// <summary>
        /// Is pq[1..N] a max heap?
        /// </summary>
        /// <returns></returns>
        private bool IsMaxHeap()
        {
            return IsMaxHeap(1);
        }

        /// <summary>
        /// Is subtree of pq[1..N] rooted at k a max heap?
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        private bool IsMaxHeap(int k)
        {
            if (k > _n)
            {
                return true;
            }
            int left = 2 * k, right = 2 * k + 1;
            if (left <= _n && Less(k, left))
            {
                return false;
            }
            if (right <= _n && Less(k, right))
            {
                return false;
            }
            return IsMaxHeap(left) && IsMaxHeap(right);
        }

        #region IEnumerator

        /// <summary>
        /// Returns an iterator that iterates over the keys on the priority queue
        /// in descending order.
        /// The iterator doesn't implement remove() since it's optional.
        /// </summary>
        /// <returns>an iterator that iterates over the keys in descending order</returns>
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
            private MaxPQ<TKey> _copy;

            private MaxPQ<TKey> _maxPq;

            public TKey Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Add all items to copy of heap
            /// takes linear time since already in heap order so no keys move
            /// </summary>
            /// <param name="maxPq"></param>
            public HeapIterator(MaxPQ<TKey> maxPq)
            {
                _maxPq = maxPq;
                Reset();
            }

            public bool MoveNext()
            {
                bool hasNext = _copy.IsEmpty();
                if (hasNext)
                {
                    Current = _copy.DelMax();
                }
                return hasNext;
            }

            public void Reset()
            {
                if (_maxPq._comparator == null)
                {
                    _copy = new MaxPQ<TKey>(_maxPq.Size());
                }
                else
                {
                    _copy = new MaxPQ<TKey>(_maxPq.Size(), _maxPq._comparator);
                }
                for (int i = 1; i <= _maxPq._n; i++)
                {
                    _copy.Insert(_maxPq._pq[i]);
                }
            }

            public void Dispose()
            {
                _copy = null;
                _maxPq = null;
            }
        }

        #endregion

        /// <summary>
        /// Unit tests the MaxPQ data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            MaxPQ<string> pq = new MaxPQ<string>();
            while (!StdIn.IsEmpty())
            {
                string item = StdIn.ReadString();
                if (item != "-")
                {
                    pq.Insert(item);
                }
                else if (!pq.IsEmpty())
                {
                    StdOut.Print(pq.DelMax() + " ");
                }
            }
            StdOut.PrintLn("(" + pq.Size() + " left on pq)");
        }
    }
}