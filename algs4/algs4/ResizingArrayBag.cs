using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class ResizingArrayBag<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Array of items
        /// </summary>
        private TItem[] _a;

        /// <summary>
        /// Number of elements on stack
        /// </summary>
        private int _n;

        /// <summary>
        /// Initializes an empty bag.
        /// </summary>
        public ResizingArrayBag()
        {
            _a = new TItem[2];
        }

        /// <summary>
        /// Is this bag empty?
        /// </summary>
        /// <returns>true if this bag is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _n == 0;
        }

        /// <summary>
        /// Returns the number of items in this bag.
        /// </summary>
        /// <returns>the number of items in this bag</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Resize the underlying array holding the elements
        /// </summary>
        /// <param name="capacity"></param>
        private void Resize(int capacity)
        {
            Debug.Assert(capacity >= _n);
            TItem[] temp = new TItem[capacity];
            for (int i = 0; i < _n; i++)
            {
                temp[i] = _a[i];
            }
            _a = temp;
        }

        /// <summary>
        /// Adds the item to this bag.
        /// </summary>
        /// <param name="item">the item to add to this bag</param>
        public void Add(TItem item)
        {
            if (_n == _a.Length)
            {
                Resize(2 * _a.Length); // double size of array if necessary
            }
            _a[_n++] = item; // add item
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in the bag in arbitrary order.
        /// </summary>
        /// <returns>an iterator that iterates over the items in the bag in arbitrary order</returns>
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
            private readonly ResizingArrayBag<TItem> _resizingArrayBag;

            public ArrayIterator(ResizingArrayBag<TItem> resizingArrayBag)
            {
                _resizingArrayBag = resizingArrayBag;
            }

            public TItem Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                bool hasNext = _i < _resizingArrayBag._n;
                if (hasNext)
                {
                    Current = _resizingArrayBag._a[_i++];
                }
                return hasNext;
            }

            public void Reset()
            {
                _i = 0;
            }

            public void Dispose()
            {
            }
        }

        /// <summary>
        /// Unit tests the ResizingArrayBag data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            ResizingArrayBag<string> bag = new ResizingArrayBag<string>
            {
                "Hello", "World", "how", "are", "you"
            };

            foreach (string s in bag)
            {
                StdOut.PrintLn(s);
            }
        }
    }
}