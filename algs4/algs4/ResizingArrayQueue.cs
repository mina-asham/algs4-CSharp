using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class ResizingArrayQueue<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Queue elements
        /// </summary>
        private TItem[] _q;

        /// <summary>
        /// Number of elements on queue
        /// </summary>
        private int _n;

        /// <summary>
        /// Index of first element of queue
        /// </summary>
        private int _first;

        /// <summary>
        /// Index of next available slot
        /// </summary>
        private int _last;

        /// <summary>
        /// Initializes an empty queue.
        /// </summary>
        public ResizingArrayQueue()
        {
            _q = new TItem[2];
        }

        /// <summary>
        /// Is this queue empty?
        /// </summary>
        /// <returns>true if this queue is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _n == 0;
        }

        /// <summary>
        /// Returns the number of items in this queue.
        /// </summary>
        /// <returns>the number of items in this queue</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Resize the underlying array
        /// </summary>
        /// <param name="max"></param>
        private void Resize(int max)
        {
            Debug.Assert(max >= _n);
            TItem[] temp = new TItem[max];
            for (int i = 0; i < _n; i++)
            {
                temp[i] = _q[(_first + i) % _q.Length];
            }
            _q = temp;
            _first = 0;
            _last = _n;
        }

        /// <summary>
        /// Adds the item to this queue.
        /// </summary>
        /// <param name="item">the item to add</param>
        public void Enqueue(TItem item)
        {
            // double size of array if necessary and recopy to front of array
            if (_n == _q.Length)
            {
                Resize(2 * _q.Length); // double size of array if necessary
            }
            _q[_last++] = item; // add item
            if (_last == _q.Length)
            {
                _last = 0; // wrap-around
            }
            _n++;
        }

        /// <summary>
        /// Removes and returns the item on this queue that was least recently added.
        /// </summary>
        /// <returns>the item on this queue that was least recently added</returns>
        public TItem Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue underflow");
            }
            TItem item = _q[_first];
            _q[_first] = default(TItem); // to avoid loitering
            _n--;
            _first++;
            if (_first == _q.Length)
            {
                _first = 0; // wrap-around
            }
            // shrink size of array if necessary
            if (_n > 0 && _n == _q.Length / 4)
            {
                Resize(_q.Length / 2);
            }
            return item;
        }

        /// <summary>
        /// Returns the item least recently added to this queue.
        /// </summary>
        /// <returns>the item least recently added to this queue</returns>
        public TItem Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue underflow");
            }
            return _q[_first];
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in this queue in FIFO order.
        /// </summary>
        /// <returns>an iterator that iterates over the items in this queue in FIFO order</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new ArrayIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// An iterator
        /// </summary>
        private class ArrayIterator : IEnumerator<TItem>
        {
            private int _i;
            private ResizingArrayQueue<TItem> _resizingArrayQueue;

            public TItem Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public ArrayIterator(ResizingArrayQueue<TItem> resizingArrayQueue)
            {
                _resizingArrayQueue = resizingArrayQueue;
                Reset();
            }

            public bool MoveNext()
            {
                bool hasNext = _i < _resizingArrayQueue._n;
                if (hasNext)
                {
                    TItem[] q = _resizingArrayQueue._q;
                    int first = _resizingArrayQueue._first;

                    Current = q[(_i + first) % q.Length];
                    _i++;
                }
                return hasNext;
            }

            public void Reset()
            {
                _i = 0;
            }

            public void Dispose()
            {
                _resizingArrayQueue = null;
            }
        }

        /// <summary>
        /// Unit tests the ResizingArrayQueue data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            ResizingArrayQueue<string> q = new ResizingArrayQueue<string>();
            while (!StdIn.IsEmpty())
            {
                string item = StdIn.ReadString();
                if (item != "-")
                {
                    q.Enqueue(item);
                }
                else if (!q.IsEmpty())
                {
                    StdOut.Print(q.Dequeue() + " ");
                }
            }
            StdOut.PrintLn("(" + q.Size() + " left on queue)");
        }
    }
}