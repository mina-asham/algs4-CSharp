using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Stack<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Size of the stack
        /// </summary>
        private int _n;

        /// <summary>
        /// Top of stack
        /// </summary>
        private Node<TItem> _first;

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
        /// Initializes an empty stack.
        /// </summary>
        public Stack()
        {
            _first = null;
            _n = 0;
        }

        /// <summary>
        /// Is this stack empty?
        /// </summary>
        /// <returns>true if this stack is empty; false otherwise</returns>
        public bool IsEmpty()
        {
            return _first == null;
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
        /// Adds the item to this stack.
        /// </summary>
        /// <param name="item">item the item to add</param>
        public void Push(TItem item)
        {
            Node<TItem> oldfirst = _first;
            _first = new Node<TItem>();
            _first.Item = item;
            _first.Next = oldfirst;
            _n++;
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

            // save item to return
            TItem item = _first.Item;

            // delete first node
            _first = _first.Next;
            _n--;

            // return the saved item
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
            return _first.Item;
        }

        /// <summary>
        /// Returns a string representation of this stack.
        /// </summary>
        /// <returns>the sequence of items in the stack in LIFO order, separated by spaces</returns>
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
        /// Returns an iterator to this stack that iterates through the items in LIFO order.
        /// </summary>
        /// <returns>an iterator to this stack that iterates through the items in LIFO order.</returns>
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
        /// Unit tests the <tt>Stack</tt> data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Stack<string> s = new Stack<string>();
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
