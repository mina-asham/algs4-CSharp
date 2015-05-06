using System.Collections;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Bag<T> : IEnumerable<T>
    {
        /// <summary>
        /// Number of elements in bag
        /// </summary>
        private int _n;

        /// <summary>
        /// Beginning of bag
        /// </summary>
        private Node<T> _first;

        /// <summary>
        /// Helper linked list class
        /// </summary>
        /// <typeparam name="TInner">Node value type</typeparam>
        private class Node<TInner>
        {
            public TInner Item { get; set; }
            public Node<TInner> Next { get; set; }
        }

        /// <summary>
        /// Initializes an empty bag
        /// </summary>
        public Bag()
        {
            _n = 0;
            _first = null;
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
        /// <returns>the number of items in this bag.</returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Adds the item to this bag.
        /// </summary>
        /// <param name="item">the item to add to this bag</param>
        public void Add(T item)
        {
            Node<T> oldFirst = _first;
            _first = new Node<T> { Item = item, Next = oldFirst };
            _n++;
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in the bag in arbitrary order.
        /// </summary>
        /// <returns>an iterator that iterates over the items in the bag in arbitrary order</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator(_first);
        }

        /// <summary>
        /// An iterator
        /// </summary>
        private class ListEnumerator : IEnumerator<T>
        {
            private Node<T> _first;
            private Node<T> _current;

            public ListEnumerator(Node<T> first)
            {
                _first = first;
                Reset();
            }

            public void Dispose()
            {
                _first = null;
                _current = null;
            }

            public bool MoveNext()
            {
                if (_current != null)
                {
                    _current = _current.Next;
                }
                return _current != null;
            }

            public void Reset()
            {
                _current = new Node<T> { Next = _first };
            }

            public T Current
            {
                get { return _current.Item; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        /// <summary>
        /// Returns an iterator that iterates over the items in the bag in arbitrary order.
        /// </summary>
        /// <returns>an iterator that iterates over the items in the bag in arbitrary order</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Unit tests the Bag data type
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            Bag<string> bag = new Bag<string>();
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