using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class SparseVector
    {
        /// <summary>
        /// Length
        /// </summary>
        private readonly int _n;

        /// <summary>
        /// The vector, represented by index-value pairs
        /// </summary>
        private readonly ST<int, double> _st;

        /// <summary>
        /// Initialize the all 0s vector of length N
        /// </summary>
        /// <param name="n"></param>
        public SparseVector(int n)
        {
            _n = n;
            _st = new ST<int, double>();
        }

        /// <summary>
        /// Put st[i] = value
        /// </summary>
        /// <param name="i"></param>
        /// <param name="value"></param>
        public void Put(int i, double value)
        {
            if (i < 0 || i >= _n)
            {
                throw new IndexOutOfRangeException("Illegal index");
            }
            if (Math.Abs(value) < double.Epsilon)
            {
                _st.Delete(i);
            }
            else
            {
                _st.Put(i, value);
            }
        }

        /// <summary>
        /// Return st[i]
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double Get(int i)
        {
            if (i < 0 || i >= _n)
            {
                throw new IndexOutOfRangeException("Illegal index");
            }
            if (_st.Contains(i))
            {
                return _st.Get(i);
            }
            else
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Return the number of nonzero entries
        /// </summary>
        /// <returns></returns>
        public int Nnz()
        {
            return _st.Size();
        }

        /// <summary>
        /// Return the size of the vector
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _n;
        }

        /// <summary>
        /// Return the dot product of this vector with that vector
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public double Dot(SparseVector that)
        {
            if (_n != that._n)
            {
                throw new ArgumentException("Vector lengths disagree");
            }
            double sum = 0.0;

            // iterate over the vector with the fewest nonzeros
            if (_st.Size() <= that._st.Size())
            {
                foreach (int i in _st.Keys())
                {
                    if (that._st.Contains(i))
                    {
                        sum += Get(i) * that.Get(i);
                    }
                }
            }
            else
            {
                foreach (int i in that._st.Keys())
                {
                    if (_st.Contains(i))
                    {
                        sum += Get(i) * that.Get(i);
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// Return the dot product of this vector and that array
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public double Dot(double[] that)
        {
            double sum = 0.0;
            foreach (int i in _st.Keys())
            {
                sum += that[i] * Get(i);
            }
            return sum;
        }

        /// <summary>
        /// Return the 2-norm
        /// </summary>
        /// <returns></returns>
        public double Norm()
        {
            SparseVector a = this;
            return Math.Sqrt(a.Dot(a));
        }

        /// <summary>
        /// Return alpha * this
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public SparseVector Scale(double alpha)
        {
            SparseVector c = new SparseVector(_n);
            foreach (int i in _st.Keys())
            {
                c.Put(i, alpha * Get(i));
            }
            return c;
        }

        /// <summary>
        /// Return this + that
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public SparseVector Plus(SparseVector that)
        {
            if (_n != that._n)
            {
                throw new ArgumentException("Vector lengths disagree");
            }
            SparseVector c = new SparseVector(_n);
            // c = this
            foreach (int i in _st.Keys())
            {
                c.Put(i, Get(i));
            }

            // c = c + that
            foreach (int i in that._st.Keys())
            {
                c.Put(i, that.Get(i) + c.Get(i));
            }
            return c;
        }

        /// <summary>
        /// Return a string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            foreach (int i in _st.Keys())
            {
                s += "(" + i + ", " + _st.Get(i) + ") ";
            }
            return s;
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            SparseVector a = new SparseVector(10);
            SparseVector b = new SparseVector(10);
            a.Put(3, 0.50);
            a.Put(9, 0.75);
            a.Put(6, 0.11);
            a.Put(6, 0.00);
            b.Put(3, 0.60);
            b.Put(4, 0.90);
            StdOut.PrintLn("a = " + a);
            StdOut.PrintLn("b = " + b);
            StdOut.PrintLn("a dot b = " + a.Dot(b));
            StdOut.PrintLn("a + b   = " + a.Plus(b));
        }
    }
}