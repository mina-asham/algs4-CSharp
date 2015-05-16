using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LinkedQueue<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Number of elements on queue
        /// </summary>
        private int _n;

        /// <summary>
        /// Beginning of queue
        /// </summary>
        private Node _first;

        /// <summary>
        /// End of queue
        /// </summary>
        private Node _last;

        /// <summary>
        /// Helper linked list class
        /// </summary>
        private class Node
        {
            public TItem Item { get; set; }
            public Node Next { get; set; }
        }

        /// <summary>
        /// Initializes an empty queue.
        /// </summary>
        public LinkedQueue()
        {
            _first = null;
            _last = null;
            _n = 0;
            Debug.Assert(Check());
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
            Node oldlast = _last;
            _last = new Node();
            _last.Item = item;
            _last.Next = null;
            if (IsEmpty())
            {
                _first = _last;
            }
            else
            {
                oldlast.Next = _last;
            }
            _n++;
            Debug.Assert(Check());
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
                _last = null; // to avoid loitering
            }
            Debug.Assert(Check());
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
        /// Check internal invariants
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            if (_n == 0)
            {
                if (_first != null)
                {
                    return false;
                }
                if (_last != null)
                {
                    return false;
                }
            }
            else if (_n == 1)
            {
                if (_first == null || _last == null)
                {
                    return false;
                }
                if (_first != _last)
                {
                    return false;
                }
                if (_first.Next != null)
                {
                    return false;
                }
            }
            else
            {
                if (_first == _last)
                {
                    return false;
                }
                if (_first.Next == null)
                {
                    return false;
                }
                if (_last.Next != null)
                {
                    return false;
                }

                // check internal consistency of instance variable N
                int numberOfNodes = 0;
                for (Node x = _first; x != null; x = x.Next)
                {
                    numberOfNodes++;
                }
                if (numberOfNodes != _n)
                {
                    return false;
                }

                // check internal consistency of instance variable last
                Node lastNode = _first;
                while (lastNode.Next != null)
                {
                    lastNode = lastNode.Next;
                }
                if (_last != lastNode)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in this queue in FIFO order.
        /// </summary>
        /// <returns>an iterator that iterates over the items in this queue in FIFO order</returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new ListIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// An iterator
        /// </summary>
        private class ListIterator : IEnumerator<TItem>
        {
            private Node _first;
            private Node _current;

            public ListIterator(LinkedQueue<TItem> linkedQueue)
            {
                _first = linkedQueue._first;
                Reset();
            }

            public TItem Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
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
                _first = null;
                _current = null;
            }
        }

        /// <summary>
        /// Unit tests the LinkedQueue data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            LinkedQueue<string> q = new LinkedQueue<string>();
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