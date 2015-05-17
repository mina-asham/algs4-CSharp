using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Queue<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Number of elements on queue
        /// </summary>
        private int _n;

        /// <summary>
        /// Beginning of queue
        /// </summary>
        private Node<TItem> _first;

        /// <summary>
        /// End of queue
        /// </summary>
        private Node<TItem> _last;

        /// <summary>
        /// Helper linked list class
        /// </summary>
        /// <typeparam name="TItemInner"></typeparam>
        private class Node<TItemInner>
        {
            public TItemInner Item { get; set; }
            public Node<TItemInner> Next { get; set; }
        }

        /// <summary>
        /// Initializes an empty queue.
        /// </summary>
        public Queue()
        {
            _first = null;
            _last = null;
            _n = 0;
        }

        /// <summary>
        /// Is this queue empty?
        /// </summary>
        /// <returns>true if this queue is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _first == null;
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
        /// Returns the item least recently added to this queue.
        /// </summary>
        /// <returns>the item least recently added to this queue</returns>
        public TItem Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Queue underflow");
            }
            return _first.Item;
        }

        /// <summary>
        /// Adds the item to this queue.
        /// </summary>
        /// <param name="item">the item to add</param>
        public void Enqueue(TItem item)
        {
            Node<TItem> oldlast = _last;
            _last = new Node<TItem>
            {
                Item = item,
                Next = null
            };
            if (IsEmpty())
            {
                _first = _last;
            }
            else
            {
                oldlast.Next = _last;
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
            TItem item = _first.Item;
            _first = _first.Next;
            _n--;
            if (IsEmpty())
            {
                // to avoid loitering
                _last = null;
            }
            return item;
        }

        /// <summary>
        /// Returns a string representation of this queue.
        /// </summary>
        /// <returns>the sequence of items in FIFO order, separated by spaces</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (TItem item in this)
            {
                s.Append(item + " ");
            }
            return s.ToString();
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in this queue in FIFO order.
        /// </summary>
        /// <returns>an iterator that iterates over the items in this queue in FIFO order</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new ListIterator<TItem>(_first);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// An iterator
        /// </summary>
        /// <typeparam name="TItemInner"></typeparam>
        private class ListIterator<TItemInner> : IEnumerator<TItemInner>
        {
            private readonly Node<TItemInner> _first;
            private Node<TItemInner> _current;

            public TItemInner Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public ListIterator(Node<TItemInner> first)
            {
                _first = first;
                Reset();
            }

            public bool MoveNext()
            {
                bool hasNext = _current != null;
                if (hasNext)
                {
                    Current = _current.Item;
                    _current = _current.Next;
                }

                return hasNext;
            }

            public void Reset()
            {
                _current = _first;
            }

            public void Dispose()
            {
                _current = null;
            }
        }

        /// <summary>
        /// Unit tests the Queue data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Queue<string> q = new Queue<string>();
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