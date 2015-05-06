using System;
using System.Collections.Generic;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Transaction : IComparable<Transaction>
    {
        /// <summary>
        /// Customer
        /// </summary>
        private readonly string _who;

        /// <summary>
        /// DateTime
        /// </summary>
        private readonly DateTime _when;

        /// <summary>
        /// Amount
        /// </summary>
        private readonly double _amount;

        /// <summary>
        /// Initializes a new transaction from the given arguments.
        /// </summary>
        /// <param name="who">the person involved in the transaction</param>
        /// <param name="when">the DateTime of the transaction</param>
        /// <param name="amount">the amount of the transaction</param>
        public Transaction(string who, DateTime when, double amount)
        {
            if (double.IsNaN(amount) || double.IsInfinity(amount))
            {
                throw new ArgumentException("Amount cannot be NaN or infinite");
            }

            _who = who;
            _when = when;
            _amount = amount;
        }

        /// <summary>
        /// Initializes a new transaction by parsing a string of the form NAME DateTime AMOUNT.
        /// </summary>
        /// <param name="transaction">the string to parse</param>
        public Transaction(string transaction)
        {
            string[] a = transaction.Split();
            _who = a[0];
            _when = DateTime.Parse(a[1]);
            _amount = double.Parse(a[2]);

            if (double.IsNaN(_amount) || double.IsInfinity(_amount))
            {
                throw new ArgumentException("Amount cannot be NaN or infinite");
            }
        }

        /// <summary>
        /// Returns the name of the customer involved in the transaction.
        /// </summary>
        /// <returns>the name of the customer involved in the transaction</returns>
        public string Who()
        {
            return _who;
        }

        /// <summary>
        /// Returns the DateTime of the transaction.
        /// </summary>
        /// <returns>the DateTime of the transaction</returns>
        public DateTime When()
        {
            return _when;
        }

        /// <summary>
        /// Returns the amount of the transaction.
        /// </summary>
        /// <returns>the amount of the transaction</returns>
        public double Amount()
        {
            return _amount;
        }

        /// <summary>
        /// Returns a string representation of the transaction.
        /// </summary>
        /// <returns>a string representation of the transaction</returns>
        public override string ToString()
        {
            return string.Format("{0,-10} {1,10} {2:00000000.00}", _who, _when, _amount);
        }

        /// <summary>
        /// Compares this transaction to that transaction.
        /// </summary>
        /// <param name="that"></param>
        /// <returns>{ a negative integer, zero, a positive integer}, depending on whether the amount of this transaction is { less than, equal to, or greater than } the amount of that transaction</returns>
        public int CompareTo(Transaction that)
        {
            if (_amount < that._amount)
            {
                return -1;
            }
            if (_amount > that._amount)
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Is this transaction equal to x?
        /// </summary>
        /// <param name="x">the other transaction</param>
        /// <returns>true if this transaction is equal to x; false otherwise</returns>
        public override bool Equals(object x)
        {
            if (x == this)
            {
                return true;
            }
            if (x == null)
            {
                return false;
            }
            if (x.GetType() != GetType())
            {
                return false;
            }

            Transaction that = (Transaction)x;
            return Math.Abs(_amount - that._amount) < double.Epsilon && _who == that._who && _when == that._when;
        }

        /// <summary>
        /// Returns a hash code for this transaction.
        /// </summary>
        /// <returns>a hash code for this transaction</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = 31 * hash + _who.GetHashCode();
            hash = 31 * hash + _when.GetHashCode();
            hash = 31 * hash + _amount.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Compares two transactions by customer name.
        /// </summary>
        public class WhoOrder : IComparer<Transaction>
        {
            public int Compare(Transaction v, Transaction w)
            {
                return string.Compare(v._who, w._who, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Compares two transactions by DateTime.
        /// </summary>
        public class WhenOrder : IComparer<Transaction>
        {
            public int Compare(Transaction v, Transaction w)
            {
                return v._when.CompareTo(w._when);
            }
        }

        /// <summary>
        /// Compares two transactions by amount.
        /// </summary>
        public class HowMuchOrder : IComparer<Transaction>
        {
            public int Compare(Transaction v, Transaction w)
            {
                if (v._amount < w._amount)
                {
                    return -1;
                }
                if (v._amount > w._amount)
                {
                    return +1;
                }
                return 0;
            }
        }

        /// <summary>
        /// Unit tests the transaction data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Transaction[] a = new Transaction[4];
            a[0] = new Transaction("Turing   6/17/1990  644.08");
            a[1] = new Transaction("Tarjan   3/26/2002 4121.85");
            a[2] = new Transaction("Knuth    6/14/1999  288.34");
            a[3] = new Transaction("Dijkstra 8/22/2007 2678.40");

            StdOut.PrintLn("Unsorted");
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Sort by DateTime");
            Array.Sort(a, new WhenOrder());
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Sort by customer");
            Array.Sort(a, new WhoOrder());
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
            StdOut.PrintLn();

            StdOut.PrintLn("Sort by amount");
            Array.Sort(a, new HowMuchOrder());
            for (int i = 0; i < a.Length; i++)
            {
                StdOut.PrintLn(a[i]);
            }
            StdOut.PrintLn();
        }
    }
}