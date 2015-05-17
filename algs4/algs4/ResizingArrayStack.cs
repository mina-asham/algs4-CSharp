using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class ResizingArrayStack<TItem> : IEnumerable<TItem>
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
        /// Initializes an empty stack.
        /// </summary>
        public ResizingArrayStack()
        {
            _a = new TItem[2];
        }

        /// <summary>
        /// Is this stack empty?
        /// </summary>
        /// <returns>true if this stack is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _n == 0;
        }

        /// <summary>
        /// Returns the number of items in the stack.
        /// </summary>
        /// <returns>the number of items in the stack</returns>
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
        /// Adds the item to this stack.
        /// </summary>
        /// <param name="item">the item to add</param>
        public void Push(TItem item)
        {
            if (_n == _a.Length)
            {
                Resize(2 * _a.Length); // double size of array if necessary
            }
            _a[_n++] = item; // add item
        }

        /// <summary>
        /// Removes and returns the item most recently added to this stack.
        /// </summary>
        /// <returns>the item most recently added</returns>
        public TItem Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack underflow");
            }
            TItem item = _a[_n - 1];
            _a[_n - 1] = default(TItem); // to avoid loitering
            _n--;
            // shrink size of array if necessary
            if (_n > 0 && _n == _a.Length / 4)
            {
                Resize(_a.Length / 2);
            }
            return item;
        }

        /// <summary>
        /// Returns (but does not remove) the item most recently added to this stack.
        /// </summary>
        /// <returns>the item most recently added to this stack</returns>
        public TItem Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Stack underflow");
            }
            return _a[_n - 1];
        }

        /// <summary>
        /// Returns an iterator to this stack that iterates through the items in LIFO order.
        /// </summary>
        /// <returns>an iterator to this stack that iterates through the items in LIFO order.</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new ReverseArrayIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// An iterator
        /// </summary>
        private class ReverseArrayIterator : IEnumerator<TItem>
        {
            private ResizingArrayStack<TItem> _resizingArrayStack;
            private int _i;

            public TItem Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public ReverseArrayIterator(ResizingArrayStack<TItem> resizingArrayStack)
            {
                _resizingArrayStack = resizingArrayStack;
                Reset();
            }

            public bool MoveNext()
            {
                bool hasNext = _i >= 0;
                if (hasNext)
                {
                    Current = _resizingArrayStack._a[_i--];
                }
                return hasNext;
            }

            public void Reset()
            {
                _i = _resizingArrayStack._n - 1;
            }

            public void Dispose()
            {
                _resizingArrayStack = null;
            }
        }

        /// <summary>
        /// Unit tests the Stack data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            ResizingArrayStack<string> s = new ResizingArrayStack<string>();
            while (!StdIn.IsEmpty())
            {
                string item = StdIn.ReadString();
                if (item != "-")
                {
                    s.Push(item);
                }
                else if (!s.IsEmpty())
                {
                    StdOut.Print(s.Pop() + " ");
                }
            }
            StdOut.PrintLn("(" + s.Size() + " left on stack)");
        }
    }
}