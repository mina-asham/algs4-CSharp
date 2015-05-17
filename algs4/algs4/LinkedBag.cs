using System.Collections;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LinkedBag<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Number of elements in bag
        /// </summary>
        private int _n;

        /// <summary>
        /// Beginning of bag
        /// </summary>
        private Node _first;

        /// <summary>
        /// Helper linked list class
        /// </summary>
        private class Node
        {
            public TItem Item { get; set; }
            public Node Next { get; set; }
        }

        /// <summary>
        /// Initializes an empty bag.
        /// </summary>
        public LinkedBag()
        {
            _first = null;
            _n = 0;
        }

        /// <summary>
        /// Is this bag empty?
        /// </summary>
        /// <returns>true if this bag is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _first == null;
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
        /// Adds the item to this bag.
        /// </summary>
        /// <param name="item">the item to add to this bag</param>
        public void Add(TItem item)
        {
            Node oldfirst = _first;
            _first = new Node
            {
                Item = item,
                Next = oldfirst
            };
            _n++;
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in the bag.
        /// </summary>
        /// <returns></returns>
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

            public ListIterator(LinkedBag<TItem> linkedBag)
            {
                _first = linkedBag._first;
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
        /// Unit tests the LinkedBag data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            LinkedBag<string> bag = new LinkedBag<string>();
            while (!StdIn.IsEmpty())
            {
                string item = StdIn.ReadString();
                bag.Add(item);
            }

            StdOut.PrintLn("size of bag = " + bag.Size());
            foreach (string s in bag)
            {
                StdOut.PrintLn(s);
            }
        }
    }
}