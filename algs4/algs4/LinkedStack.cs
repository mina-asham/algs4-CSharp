using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class LinkedStack<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Size of the stack
        /// </summary>
        private int _n;

        /// <summary>
        /// Top of stack
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
        /// Initializes an empty stack.
        /// </summary>
        public LinkedStack()
        {
            _first = null;
            _n = 0;
            Debug.Assert(Check());
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
        /// <param name="item">the item to add</param>
        public void Push(TItem item)
        {
            Node oldfirst = _first;
            _first = new Node();
            _first.Item = item;
            _first.Next = oldfirst;
            _n++;
            Debug.Assert(Check());
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
            TItem item = _first.Item; // save item to return
            _first = _first.Next; // delete first node
            _n--;
            Debug.Assert(Check());
            return item; // return the saved item
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

            public ListIterator(LinkedStack<TItem> linkedStack)
            {
                _first = linkedStack._first;
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
            }
            else if (_n == 1)
            {
                if (_first == null)
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
                if (_first.Next == null)
                {
                    return false;
                }
            }

            // check internal consistency of instance variable N
            int numberOfNodes = 0;
            for (Node x = _first; x != null; x = x.Next)
            {
                numberOfNodes++;
            }
            return numberOfNodes == _n;
        }

        /// <summary>
        /// Unit tests the LinkedStack data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            LinkedStack<string> s = new LinkedStack<string>();
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