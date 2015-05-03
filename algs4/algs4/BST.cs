using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class BST<TKey, TValue> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Root of BST
        /// </summary>
        private Node _root;

        private class Node
        {
            /// <summary>
            /// Sorted by key
            /// </summary>
            public TKey Key { get; private set; }

            /// <summary>
            /// Associated data
            /// </summary>
            public TValue Val { get; set; }

            /// <summary>
            /// Left subtree
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// Right subtree
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// Number of nodes in subtree
            /// </summary>
            public int N;

            public Node(TKey key, TValue val, int n)
            {
                Key = key;
                Val = val;
                N = n;
            }
        }

        /// <summary>
        /// Is the symbol table empty?
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Return number of key-value pairs in BST
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return Size(_root);
        }

        /// <summary>
        /// Return number of key-value pairs in BST rooted at x
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

        #region Search BST for given key, and return associated value if found, return null if not found

        /// <summary>
        /// Does there exist a key-value pair with given key?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(TKey key)
        {
            bool exists;
            Get(_root, key, out exists);
            return exists;
        }

        /// <summary>
        /// Return value associated with the given key, or null if no such key exists
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            bool exists;
            return Get(_root, key, out exists);
        }

        private TValue Get(Node x, TKey key, out bool exists)
        {
            if (x == null)
            {
                exists = false;
                return default(TValue);
            }
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                return Get(x.Left, key, out exists);
            }
            if (cmp > 0)
            {
                return Get(x.Right, key, out exists);
            }
            exists = true;
            return x.Val;
        }

        #endregion

        /// <summary>
        /// Insert key-value pair into BST
        /// If key already exists, update with new value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Put(TKey key, TValue val)
        {
            if (!typeof(TValue).IsValueType && Equals(val, default(TValue)))
            {
                Delete(key);
                return;
            }
            _root = Put(_root, key, val);
        }

        private Node Put(Node x, TKey key, TValue val)
        {
            if (x == null)
            {
                return new Node(key, val, 1);
            }
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                x.Left = Put(x.Left, key, val);
            }
            else if (cmp > 0)
            {
                x.Right = Put(x.Right, key, val);
            }
            else
            {
                x.Val = val;
            }
            x.N = 1 + Size(x.Left) + Size(x.Right);
            return x;
        }

        #region Delete

        public void DeleteMin()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Symbol table underflow");
            }
            _root = DeleteMin(_root);
        }

        private Node DeleteMin(Node x)
        {
            if (x.Left == null)
            {
                return x.Right;
            }
            x.Left = DeleteMin(x.Left);
            x.N = Size(x.Left) + Size(x.Right) + 1;
            return x;
        }

        public void DeleteMax()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Symbol table underflow");
            }
            _root = DeleteMax(_root);

        }

        private Node DeleteMax(Node x)
        {
            if (x.Right == null)
            {
                return x.Left;
            }
            x.Right = DeleteMax(x.Right);
            x.N = Size(x.Left) + Size(x.Right) + 1;
            return x;
        }

        public void Delete(TKey key)
        {
            _root = Delete(_root, key);
        }

        private Node Delete(Node x, TKey key)
        {
            if (x == null)
            {
                return null;
            }
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                x.Left = Delete(x.Left, key);
            }
            else if (cmp > 0)
            {
                x.Right = Delete(x.Right, key);
            }
            else
            {
                if (x.Right == null)
                {
                    return x.Left;
                }
                if (x.Left == null)
                {
                    return x.Right;
                }
                Node t = x;
                x = Min(t.Right);
                x.Right = DeleteMin(t.Right);
                x.Left = t.Left;
            }
            x.N = Size(x.Left) + Size(x.Right) + 1;
            return x;
        }

        #endregion

        #region Min, max, floor, and ceiling

        public TKey Min()
        {
            if (IsEmpty())
            {
                return default(TKey);
            }
            return Min(_root).Key;
        }

        private Node Min(Node x)
        {
            if (x.Left == null)
            {
                return x;
            }
            return Min(x.Left);
        }

        public TKey Max()
        {
            if (IsEmpty())
            {
                return default(TKey);
            }
            return Max(_root).Key;
        }

        private Node Max(Node x)
        {
            if (x.Right == null)
            {
                return x;
            }
            return Max(x.Right);
        }

        public TKey Floor(TKey key)
        {
            Node x = Floor(_root, key);
            if (x == null)
            {
                return default(TKey);
            }
            return x.Key;
        }

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

        public TKey Ceiling(TKey key)
        {
            Node x = Ceiling(_root, key);
            if (x == null)
            {
                return default(TKey);
            }
            return x.Key;
        }

        private Node Ceiling(Node x, TKey key)
        {
            if (x == null) return null;
            int cmp = key.CompareTo(x.Key);
            if (cmp == 0)
            {
                return x;
            }
            if (cmp < 0)
            {
                Node t = Ceiling(x.Left, key);
                if (t != null)
                {
                    return t;
                }
                return x;
            }
            return Ceiling(x.Right, key);
        }

        #endregion

        #region Rank and selection

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
        /// Return key of rank k.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private Node Select(Node x, int k)
        {
            if (x == null)
            {
                return null;
            }
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

        public int Rank(TKey key)
        {
            return Rank(key, _root);
        }

        /// <summary>
        /// Number of keys in the subtree less than key.
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

        public IEnumerable<TKey> Keys()
        {
            return Keys(Min(), Max());
        }

        public IEnumerable<TKey> Keys(TKey lo, TKey hi)
        {
            Queue<TKey> queue = new Queue<TKey>();
            Keys(_root, queue, lo, hi);
            return queue;
        }

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

        /// <summary>
        /// Height of this BST (one-node tree has height 0)
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

        /// <summary>
        /// Level order traversal
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKey> LevelOrder()
        {
            Queue<TKey> keys = new Queue<TKey>();
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(_root);
            while (queue.Count != 0)
            {
                Node x = queue.Dequeue();
                if (x == null)
                {
                    continue;
                }
                keys.Enqueue(x.Key);
                queue.Enqueue(x.Left);
                queue.Enqueue(x.Right);
            }
            return keys;
        }

        #endregion

        #region Check integrity of BST data structure

        public bool Check()
        {
            if (!IsBST())
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
            return IsBST() && IsSizeConsistent() && IsRankConsistent();
        }

        /// <summary>
        /// Does this binary tree satisfy symmetric order?
        /// Note: this test also ensures that data structure is a binary tree since order is strict
        /// </summary>
        /// <returns></returns>
        private bool IsBST()
        {
            Type type = typeof(TKey);
            TKey min;
            TKey max;
            if (type.IsValueType)
            {
                min = (TKey)type.GetField("MinValue").GetRawConstantValue();
                max = (TKey)type.GetField("MaxValue").GetRawConstantValue();
            }
            else
            {
                min = default(TKey);
                max = default(TKey);
            }

            return IsBST(_root, min, max);
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
        private bool IsBST(Node x, TKey min, TKey max)
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
            return IsBST(x.Left, min, x.Key) && IsBST(x.Right, x.Key, max);
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

        #endregion

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            BST<string, int> st = new BST<string, int>();
            for (int i = 0; !StdIn.IsEmpty(); i++)
            {
                string key = StdIn.ReadString();
                st.Put(key, i);
            }

            foreach (string s in st.LevelOrder())
            {
                StdOut.PrintLn(s + " " + st.Get(s));
            }

            StdOut.PrintLn();

            foreach (string s in st.Keys())
            {
                StdOut.PrintLn(s + " " + st.Get(s));
            }
        }
    }

}
