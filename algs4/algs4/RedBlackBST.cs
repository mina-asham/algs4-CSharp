using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class RedBlackBST<TKey, TValue> where TKey : IComparable<TKey>
    {
        private const bool Red = true;
        private const bool Black = false;

        /// <summary>
        /// Root of the BST
        /// </summary>
        private Node _root;

        /// <summary>
        /// BST helper node data type
        /// </summary>
        private class Node
        {
            /// <summary>
            /// Key
            /// </summary>
            public TKey Key { get; set; }

            /// <summary>
            /// Associated data
            /// </summary>
            public TValue Val { get; set; }

            /// <summary>
            /// Link to left subtree
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// Link to right subtree
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// Color of parent link
            /// </summary>
            public bool Color;

            /// <summary>
            /// Subtree count
            /// </summary>
            public int N;

            public Node(TKey key, TValue val, bool color, int n)
            {
                Key = key;
                Val = val;
                Color = color;
                N = n;
            }
        }

        #region Node helper methods

        /// <summary>
        /// Is node x red; false if x is null ?
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static bool IsRed(Node x)
        {
            if (x == null)
            {
                return false;
            }
            return (x.Color == Red);
        }

        /// <summary>
        /// Number of node in subtree rooted at x; 0 if x is null
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int Size(Node x)
        {
            if (x == null)
            {
                return 0;
            }
            return x.N;
        }

        #endregion

        #region Size methods

        /// <summary>
        /// Return number of key-value pairs in this symbol table
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return Size(_root);
        }

        /// <summary>
        /// Is this symbol table empty?
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _root == null;
        }

        #endregion

        #region Standard BST search

        /// <summary>
        /// Value associated with the given key; null if no such key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            bool exists;
            return Get(_root, key, out exists);
        }

        /// <summary>
        /// Value associated with the given key in subtree rooted at x; null if no such key
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <param name="exists"></param>
        /// <returns></returns>
        private TValue Get(Node x, TKey key, out bool exists)
        {
            while (x != null)
            {
                int cmp = key.CompareTo(x.Key);
                if (cmp < 0)
                {
                    x = x.Left;
                }
                else if (cmp > 0)
                {
                    x = x.Right;
                }
                else
                {
                    exists = true;
                    return x.Val;
                }
            }
            exists = false;
            return default(TValue);
        }

        /// <summary>
        /// Is there a key-value pair with the given key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            bool exists;
            Get(_root, key, out exists);
            return exists;
        }

        #endregion

        #region Red-black insertion

        /// <summary>
        /// Insert the key-value pair; overwrite the old value with the new value
        /// if the key is already present
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Put(TKey key, TValue val)
        {
            _root = Put(_root, key, val);
            _root.Color = Black;
        }

        /// <summary>
        /// Insert the key-value pair in the subtree rooted at h
        /// </summary>
        /// <param name="h"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private Node Put(Node h, TKey key, TValue val)
        {
            if (h == null)
            {
                return new Node(key, val, Red, 1);
            }

            int cmp = key.CompareTo(h.Key);
            if (cmp < 0)
            {
                h.Left = Put(h.Left, key, val);
            }
            else if (cmp > 0)
            {
                h.Right = Put(h.Right, key, val);
            }
            else
            {
                h.Val = val;
            }

            // fix-up any right-leaning links
            if (IsRed(h.Right) && !IsRed(h.Left))
            {
                h = RotateLeft(h);
            }
            if (IsRed(h.Left) && IsRed(h.Left.Left))
            {
                h = RotateRight(h);
            }
            if (IsRed(h.Left) && IsRed(h.Right))
            {
                FlipColors(h);
            }
            h.N = Size(h.Left) + Size(h.Right) + 1;

            return h;
        }

        #endregion

        #region Red-black deletion

        /// <summary>
        /// Delete the key-value pair with the minimum key
        /// </summary>
        public void DeleteMin()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("BST underflow");
            }

            // if both children of root are black, set root to red
            if (!IsRed(_root.Left) && !IsRed(_root.Right))
            {
                _root.Color = Red;
            }

            _root = DeleteMin(_root);
            if (!IsEmpty())
            {
                _root.Color = Black;
            }
        }

        /// <summary>
        /// Delete the key-value pair with the minimum key rooted at h
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node DeleteMin(Node h)
        {
            if (h.Left == null)
            {
                return null;
            }

            if (!IsRed(h.Left) && !IsRed(h.Left.Left))
            {
                h = MoveRedLeft(h);
            }

            h.Left = DeleteMin(h.Left);
            return Balance(h);
        }

        /// <summary>
        /// Delete the key-value pair with the maximum key
        /// </summary>
        public void DeleteMax()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("BST underflow");
            }

            // if both children of root are black, set root to red
            if (!IsRed(_root.Left) && !IsRed(_root.Right))
            {
                _root.Color = Red;
            }

            _root = DeleteMax(_root);
            if (!IsEmpty())
            {
                _root.Color = Black;
            }
        }

        /// <summary>
        /// Delete the key-value pair with the maximum key rooted at h
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node DeleteMax(Node h)
        {
            if (IsRed(h.Left))
            {
                h = RotateRight(h);
            }

            if (h.Right == null)
            {
                return null;
            }

            if (!IsRed(h.Right) && !IsRed(h.Right.Left))
            {
                h = MoveRedRight(h);
            }

            h.Right = DeleteMax(h.Right);

            return Balance(h);
        }

        /// <summary>
        /// Delete the key-value pair with the given key
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TKey key)
        {
            if (!Contains(key))
            {
                Console.Error.WriteLine("symbol table does not contain " + key);
                return;
            }

            // if both children of root are black, set root to red
            if (!IsRed(_root.Left) && !IsRed(_root.Right))
            {
                _root.Color = Red;
            }

            _root = Delete(_root, key);
            if (!IsEmpty())
            {
                _root.Color = Black;
            }
        }

        /// <summary>
        /// Delete the key-value pair with the given key rooted at h
        /// </summary>
        /// <param name="h"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node Delete(Node h, TKey key)
        {
            if (key.CompareTo(h.Key) < 0)
            {
                if (!IsRed(h.Left) && !IsRed(h.Left.Left))
                {
                    h = MoveRedLeft(h);
                }
                h.Left = Delete(h.Left, key);
            }
            else
            {
                if (IsRed(h.Left))
                {
                    h = RotateRight(h);
                }
                if (key.CompareTo(h.Key) == 0 && (h.Right == null))
                {
                    return null;
                }
                if (!IsRed(h.Right) && !IsRed(h.Right.Left))
                {
                    h = MoveRedRight(h);
                }
                if (key.CompareTo(h.Key) == 0)
                {
                    Node x = Min(h.Right);
                    h.Key = x.Key;
                    h.Val = x.Val;
                    h.Right = DeleteMin(h.Right);
                }
                else
                {
                    h.Right = Delete(h.Right, key);
                }
            }
            return Balance(h);
        }

        #endregion

        #region Red-black tree helper functions

        /// <summary>
        /// Make a left-leaning link lean to the right
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node RotateRight(Node h)
        {
            Node x = h.Left;
            h.Left = x.Right;
            x.Right = h;
            x.Color = x.Right.Color;
            x.Right.Color = Red;
            x.N = h.N;
            h.N = Size(h.Left) + Size(h.Right) + 1;
            return x;
        }

        /// <summary>
        /// Make a right-leaning link lean to the left
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node RotateLeft(Node h)
        {
            Node x = h.Right;
            h.Right = x.Left;
            x.Left = h;
            x.Color = x.Left.Color;
            x.Left.Color = Red;
            x.N = h.N;
            h.N = Size(h.Left) + Size(h.Right) + 1;
            return x;
        }

        /// <summary>
        /// Flip the colors of a node and its two children
        /// </summary>
        /// <param name="h"></param>
        private void FlipColors(Node h)
        {
            h.Color = !h.Color;
            h.Left.Color = !h.Left.Color;
            h.Right.Color = !h.Right.Color;
        }

        /// <summary>
        /// Assuming that h is red and both h.left and h.left.left
        /// are black, make h.left or one of its children red.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node MoveRedLeft(Node h)
        {
            FlipColors(h);
            if (IsRed(h.Right.Left))
            {
                h.Right = RotateRight(h.Right);
                h = RotateLeft(h);
                FlipColors(h);
            }
            return h;
        }

        /// <summary>
        /// Assuming that h is red and both h.right and h.right.left
        /// are black, make h.right or one of its children red.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node MoveRedRight(Node h)
        {
            FlipColors(h);
            if (IsRed(h.Left.Left))
            {
                h = RotateRight(h);
                FlipColors(h);
            }
            return h;
        }

        /// <summary>
        /// Restore red-black tree invariant
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node Balance(Node h)
        {
            if (IsRed(h.Right))
            {
                h = RotateLeft(h);
            }
            if (IsRed(h.Left) && IsRed(h.Left.Left))
            {
                h = RotateRight(h);
            }
            if (IsRed(h.Left) && IsRed(h.Right))
            {
                FlipColors(h);
            }

            h.N = Size(h.Left) + Size(h.Right) + 1;
            return h;
        }

        #endregion

        #region Utility functions

        /// <summary>
        /// Height of tree (1-node tree has height 0)
        /// </summary>
        /// <returns></returns>
        public int Height()
        {
            return Height(_root);
        }

        private int Height(Node x)
        {
            if (x == null)
            {
                return -1;
            }
            return 1 + Math.Max(Height(x.Left), Height(x.Right));
        }

        #endregion

        #region Ordered symbol table methods.

        /// <summary>
        /// The smallest key; null if no such key
        /// </summary>
        /// <returns></returns>
        public TKey Min()
        {
            if (IsEmpty())
            {
                return default(TKey);
            }
            return Min(_root).Key;
        }

        /// <summary>
        /// The smallest key in subtree rooted at x; null if no such key
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private Node Min(Node x)
        {
            if (x.Left == null)
            {
                return x;
            }
            return Min(x.Left);
        }

        /// <summary>
        /// The largest key; null if no such key
        /// </summary>
        /// <returns></returns>
        public TKey Max()
        {
            if (IsEmpty())
            {
                return default(TKey);
            }
            return Max(_root).Key;
        }

        /// <summary>
        /// The largest key in the subtree rooted at x; null if no such key
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private Node Max(Node x)
        {
            if (x.Right == null)
            {
                return x;
            }
            return Max(x.Right);
        }

        /// <summary>
        /// The largest key less than or equal to the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TKey Floor(TKey key)
        {
            Node x = Floor(_root, key);
            if (x == null)
            {
                return default(TKey);
            }
            return x.Key;
        }

        /// <summary>
        /// The largest key in the subtree rooted at x less than or equal to the given key
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node Floor(Node x, TKey key)
        {
            if (x == null)
            {
                return null;
            }
            int cmp = key.CompareTo(x.Key);
            if (cmp == 0)
            {
                return x;
            }
            if (cmp < 0)
            {
                return Floor(x.Left, key);
            }
            Node t = Floor(x.Right, key);
            if (t != null)
            {
                return t;
            }
            return x;
        }

        /// <summary>
        /// The smallest key greater than or equal to the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TKey Ceiling(TKey key)
        {
            Node x = Ceiling(_root, key);
            if (x == null)
            {
                return default(TKey);
            }
            return x.Key;
        }

        /// <summary>
        /// The smallest key in the subtree rooted at x greater than or equal to the given key
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node Ceiling(Node x, TKey key)
        {
            if (x == null)
            {
                return null;
            }
            int cmp = key.CompareTo(x.Key);
            if (cmp == 0)
            {
                return x;
            }
            if (cmp > 0)
            {
                return Ceiling(x.Right, key);
            }
            Node t = Ceiling(x.Left, key);
            if (t != null)
            {
                return t;
            }
            return x;
        }

        /// <summary>
        /// The key of rank k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public TKey Select(int k)
        {
            if (k < 0 || k >= Size())
            {
                return default(TKey);
            }
            Node x = Select(_root, k);
            return x.Key;
        }

        /// <summary>
        /// The key of rank k in the subtree rooted at x
        /// </summary>
        /// <param name="x"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private Node Select(Node x, int k)
        {
            int t = Size(x.Left);
            if (t > k)
            {
                return Select(x.Left, k);
            }
            if (t < k)
            {
                return Select(x.Right, k - t - 1);
            }
            return x;
        }

        /// <summary>
        /// Number of keys less than key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Rank(TKey key)
        {
            return Rank(key, _root);
        }

        /// <summary>
        /// Number of keys less than key in the subtree rooted at x
        /// </summary>
        /// <param name="key"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private int Rank(TKey key, Node x)
        {
            if (x == null)
            {
                return 0;
            }
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                return Rank(key, x.Left);
            }
            if (cmp > 0)
            {
                return 1 + Size(x.Left) + Rank(key, x.Right);
            }
            return Size(x.Left);
        }

        #endregion

        #region Range count and range search.

        /// <summary>
        /// All of the keys, as an IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> Keys()
        {
            return Keys(Min(), Max());
        }

        /// <summary>
        /// The keys between lo and hi, as an IEnumerable
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public IEnumerable<TKey> Keys(TKey lo, TKey hi)
        {
            Queue<TKey> queue = new Queue<TKey>();
            Keys(_root, queue, lo, hi);
            return queue;
        }

        /// <summary>
        /// Add the keys between lo and hi in the subtree rooted at x
        /// to the queue
        /// </summary>
        /// <param name="x"></param>
        /// <param name="queue"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        private void Keys(Node x, Queue<TKey> queue, TKey lo, TKey hi)
        {
            if (x == null)
            {
                return;
            }
            int cmplo = lo.CompareTo(x.Key);
            int cmphi = hi.CompareTo(x.Key);
            if (cmplo < 0)
            {
                Keys(x.Left, queue, lo, hi);
            }
            if (cmplo <= 0 && cmphi >= 0)
            {
                queue.Enqueue(x.Key);
            }
            if (cmphi > 0)
            {
                Keys(x.Right, queue, lo, hi);
            }
        }

        /// <summary>
        /// Number keys between lo and hi
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        public int Size(TKey lo, TKey hi)
        {
            if (lo.CompareTo(hi) > 0)
            {
                return 0;
            }
            if (Contains(hi))
            {
                return Rank(hi) - Rank(lo) + 1;
            }
            return Rank(hi) - Rank(lo);
        }

        #endregion

        /// <summary>
        /// Check integrity of red-black BST data structure
        /// </summary>
        /// <returns></returns>
        public bool Check()
        {
            if (!IsBst())
            {
                StdOut.PrintLn("Not in symmetric order");
            }
            if (!IsSizeConsistent())
            {
                StdOut.PrintLn("Subtree counts not consistent");
            }
            if (!IsRankConsistent())
            {
                StdOut.PrintLn("Ranks not consistent");
            }
            if (!Is23())
            {
                StdOut.PrintLn("Not a 2-3 tree");
            }
            if (!IsBalanced())
            {
                StdOut.PrintLn("Not balanced");
            }
            return IsBst() && IsSizeConsistent() && IsRankConsistent() && Is23() && IsBalanced();
        }

        /// <summary>
        /// Does this binary tree satisfy symmetric order?
        /// Note: this test also ensures that data structure is a binary tree since order is strict
        /// </summary>
        /// <returns></returns>
        private bool IsBst()
        {
            return IsBst(_root, default(TKey), default(TKey));
        }

        /// <summary>
        /// Is the tree rooted at x a BST with all keys strictly between min and max
        /// (if min or max is null, treat as empty constraint)
        /// Credit: Bob Dondero's elegant solution
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static bool IsBst(Node x, TKey min, TKey max)
        {
            if (x == null)
            {
                return true;
            }
            if (!typeof(TKey).IsValueType && !Equals(min, default(TKey)) && x.Key.CompareTo(min) <= 0)
            {
                return false;
            }
            if (!typeof(TKey).IsValueType && !Equals(max, default(TKey)) && x.Key.CompareTo(max) >= 0)
            {
                return false;
            }
            return IsBst(x.Left, min, x.Key) && IsBst(x.Right, x.Key, max);
        }

        /// <summary>
        /// Are the size fields correct?
        /// </summary>
        /// <returns></returns>
        private bool IsSizeConsistent()
        {
            return IsSizeConsistent(_root);
        }

        private bool IsSizeConsistent(Node x)
        {
            if (x == null)
            {
                return true;
            }
            if (x.N != Size(x.Left) + Size(x.Right) + 1)
            {
                return false;
            }
            return IsSizeConsistent(x.Left) && IsSizeConsistent(x.Right);
        }

        /// <summary>
        /// Check that ranks are consistent
        /// </summary>
        /// <returns></returns>
        private bool IsRankConsistent()
        {
            for (int i = 0; i < Size(); i++)
            {
                if (i != Rank(Select(i)))
                {
                    return false;
                }
            }
            foreach (TKey key in Keys())
            {
                if (key.CompareTo(Select(Rank(key))) != 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Does the tree have no red right links, and at most one (left)
        /// red links in a row on any path?
        /// </summary>
        /// <returns></returns>
        private bool Is23()
        {
            return Is23(_root);
        }

        private bool Is23(Node x)
        {
            if (x == null)
            {
                return true;
            }
            if (IsRed(x.Right))
            {
                return false;
            }
            if (x != _root && IsRed(x) && IsRed(x.Left))
            {
                return false;
            }
            return Is23(x.Left) && Is23(x.Right);
        }

        /// <summary>
        /// Do all paths from root to leaf have same number of black edges?
        /// </summary>
        /// <returns></returns>
        private bool IsBalanced()
        {
            // number of black links on path from root to min
            int black = 0;
            Node x = _root;
            while (x != null)
            {
                if (!IsRed(x))
                {
                    black++;
                }
                x = x.Left;
            }
            return IsBalanced(_root, black);
        }

        /// <summary>
        /// Does every path from the root to a leaf have the given number of black links?
        /// </summary>
        /// <param name="x"></param>
        /// <param name="black"></param>
        /// <returns></returns>
        private bool IsBalanced(Node x, int black)
        {
            if (x == null)
            {
                return black == 0;
            }
            if (!IsRed(x))
            {
                black--;
            }
            return IsBalanced(x.Left, black) && IsBalanced(x.Right, black);
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            RedBlackBST<string, int> st = new RedBlackBST<string, int>();
            for (int i = 0; !StdIn.IsEmpty(); i++)
            {
                string key = StdIn.ReadString();
                st.Put(key, i);
            }
            foreach (string s in st.Keys())
            {
                StdOut.PrintLn(s + " " + st.Get(s));
            }
            StdOut.PrintLn();
        }
    }
}