using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Set<T> : IEnumerable<T>
    {
        protected bool Equals(Set<T> other)
        {
            return Equals(_set, other._set);
        }

        public override int GetHashCode()
        {
            return (_set != null ? _set.GetHashCode() : 0);
        }

        private readonly SortedSet<T> _set;

        /// <summary>
        /// Initializes an empty set.
        /// </summary>
        public Set()
        {
            _set = new SortedSet<T>();
        }

        /// <summary>
        /// Adds the key to the set if it is not already present.
        /// </summary>
        /// <param name="key">the key to add</param>
        public void Add(T key)
        {
            if (!typeof(T).IsValueType && Equals(key, default(T)))
            {
                throw new NullReferenceException("called Add() with a null key");
            }
            _set.Add(key);
        }

        /// <summary>
        /// Does the set contain the given key?
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>true if the set Contains key and false otherwise</returns>
        public bool Contains(T key)
        {
            if (!typeof(T).IsValueType && Equals(key, default(T)))
            {
                throw new NullReferenceException("called Contains() with a null key");
            }

            return _set.Contains(key);
        }

        /// <summary>
        /// Removes the key from the set if the key is present.
        /// </summary>
        /// <param name="key">the key</param>
        public void Delete(T key)
        {
            if (!typeof(T).IsValueType && Equals(key, default(T)))
            {
                throw new NullReferenceException("called delete() with a null key");
            }
            _set.Remove(key);
        }

        /// <summary>
        /// Returns the number of keys in the set.
        /// </summary>
        /// <returns>the number of keys in the set</returns>
        public int Size()
        {
            return _set.Count;
        }

        /// <summary>
        /// Is the set empty?
        /// </summary>
        /// <returns>true if the set is empty, and false otherwise</returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Returns all of the keys in the set, as an iterator.
        /// To iterate over all of the keys in a set named set, use the
        /// foreach notation: for (T key : set).
        /// </summary>
        /// <returns>an iterator to all of the keys in the set</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the largest key in the set.
        /// </summary>
        /// <returns>the largest key in the set</returns>
        public T Max()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("called Max() with empty set");
            }
            return _set.Max;
        }

        /// <summary>
        /// Returns the smallest key in the set.
        /// </summary>
        /// <returns>the smallest key in the set</returns>
        public T Min()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("called Min() with empty set");
            }
            return _set.Min;
        }

        /// <summary>
        /// Returns the smallest key in the set greater than or equal to key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the smallest key in the set greater than or equal to key</returns>
        public T Ceiling(T key)
        {
            if (!typeof(T).IsValueType && Equals(key, default(T)))
            {
                throw new NullReferenceException("called Ceiling() with a null key");
            }
            T k = _set.GetViewBetween(key, _set.Max).Min;
            if (Equals(k, default(T)))
            {
                throw new InvalidOperationException("all keys are less than " + key);
            }
            return k;
        }

        /// <summary>
        /// Returns the largest key in the set less than or equal to key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>the largest key in the set table less than or equal to key</returns>
        public T Floor(T key)
        {
            if (!typeof(T).IsValueType && Equals(key, default(T)))
            {
                throw new NullReferenceException("called Floor() with a null key");
            }
            T k = _set.GetViewBetween(_set.Min, key).Max;
            if (Equals(k, default(T)))
            {
                throw new InvalidOperationException("all keys are greater than " + key);
            }
            return k;
        }

        /// <summary>
        /// Returns the union of this set and that set.
        /// </summary>
        /// <param name="that">the other set</param>
        /// <returns>the union of this set and that set</returns>
        public Set<T> Union(Set<T> that)
        {
            if (that == null)
            {
                throw new NullReferenceException("called Union() with a null argument");
            }
            Set<T> c = new Set<T>();
            foreach (T x in this)
            {
                c.Add(x);
            }
            foreach (T x in that)
            {
                c.Add(x);
            }
            return c;
        }

        /// <summary>
        /// Returns the intersection of this set and that set.
        /// </summary>
        /// <param name="that">the other set</param>
        /// <returns>the intersection of this set and that set</returns>
        public Set<T> Intersects(Set<T> that)
        {
            if (that == null)
            {
                throw new NullReferenceException("called Intersects() with a null argument");
            }
            Set<T> c = new Set<T>();
            if (Size() < that.Size())
            {
                foreach (T x in this)
                {
                    if (that.Contains(x))
                    {
                        c.Add(x);
                    }
                }
            }
            else
            {
                foreach (T x in that)
                {
                    if (Contains(x))
                    {
                        c.Add(x);
                    }
                }
            }
            return c;
        }

        /// <summary>
        /// Does this set equal y?
        /// </summary>
        /// <param name="y">the other set</param>
        /// <returns>true if the two sets are equal; false otherwise</returns>
        public override bool Equals(object y)
        {
            if (y == this)
            {
                return true;
            }
            if (y == null)
            {
                return false;
            }
            if (y.GetType() != GetType())
            {
                return false;
            }

            Set<T> that = (Set<T>)y;
            if (Size() != that.Size())
            {
                return false;
            }

            foreach (T k in this)
            {
                if (!that.Contains(k))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a string representation of this set.
        /// </summary>
        /// <returns>a string representation of this set, with the keys separated by single spaces</returns>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            foreach (T key in this)
            {
                s.Append(key + " ");
            }
            return s.ToString();
        }

        /// <summary>
        /// Unit tests the SET data type.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            Set<string> set = new Set<string>();

            // insert some keys
            set.Add("www.cs.princeton.edu");
            set.Add("www.cs.princeton.edu"); // overwrite old value
            set.Add("www.princeton.edu");
            set.Add("www.math.princeton.edu");
            set.Add("www.yale.edu");
            set.Add("www.amazon.com");
            set.Add("www.simpsons.com");
            set.Add("www.stanford.edu");
            set.Add("www.google.com");
            set.Add("www.ibm.com");
            set.Add("www.apple.com");
            set.Add("www.slashdot.com");
            set.Add("www.whitehouse.gov");
            set.Add("www.espn.com");
            set.Add("www.snopes.com");
            set.Add("www.movies.com");
            set.Add("www.cnn.com");
            set.Add("www.iitb.ac.in");

            StdOut.PrintLn(set.Contains("www.cs.princeton.edu"));
            StdOut.PrintLn(!set.Contains("www.harvardsucks.com"));
            StdOut.PrintLn(set.Contains("www.simpsons.com"));
            StdOut.PrintLn();

            StdOut.PrintLn("Ceiling(www.simpsonr.com) = " + set.Ceiling("www.simpsonr.com"));
            StdOut.PrintLn("Ceiling(www.simpsons.com) = " + set.Ceiling("www.simpsons.com"));
            StdOut.PrintLn("Ceiling(www.simpsont.com) = " + set.Ceiling("www.simpsont.com"));
            StdOut.PrintLn("Floor(www.simpsonr.com)   = " + set.Floor("www.simpsonr.com"));
            StdOut.PrintLn("Floor(www.simpsons.com)   = " + set.Floor("www.simpsons.com"));
            StdOut.PrintLn("Floor(www.simpsont.com)   = " + set.Floor("www.simpsont.com"));
            StdOut.PrintLn();

            // print out all keys in the set in lexicographic order
            foreach (string s in set)
            {
                StdOut.PrintLn(s);
            }
        }
    }
}