using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Date : IComparable<Date>
    {
        private static readonly int[] Days = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// Month (between 1 and 12)
        /// </summary>
        private readonly int _month;

        /// <summary>
        /// Day (between 1 and DAYS[month]
        /// </summary>
        private readonly int _day;

        /// <summary>
        /// Year
        /// </summary>
        private readonly int _year;

        /// <summary>
        /// Initializes a new date from the month, day, and year.
        /// </summary>
        /// <param name="month">the month (between 1 and 12)</param>
        /// <param name="day">the day (between 1 and 28-31, depending on the month)</param>
        /// <param name="year">the year</param>
        public Date(int month, int day, int year)
        {
            if (!IsValid(month, day, year))
            {
                throw new ArgumentException("Invalid date");
            }
            _month = month;
            _day = day;
            _year = year;
        }

        /// <summary>
        /// Initializes new date specified as a string in form MM/DD/YYYY.
        /// </summary>
        /// <param name="date">the string representation of the date</param>
        public Date(string date)
        {
            string[] fields = date.Split('/');
            if (fields.Length != 3)
            {
                throw new ArgumentException("Invalid date");
            }
            _month = int.Parse(fields[0]);
            _day = int.Parse(fields[1]);
            _year = int.Parse(fields[2]);
            if (!IsValid(_month, _day, _year))
            {
                throw new ArgumentException("Invalid date");
            }
        }

        /// <summary>
        /// Return the month.
        /// </summary>
        /// <returns>the month (an integer between 1 and 12)</returns>
        public int Month()
        {
            return _month;
        }

        /// <summary>
        /// Return the day.
        /// </summary>
        /// <returns>the day (an integer between 1 and 31)</returns>
        public int Day()
        {
            return _day;
        }

        /// <summary>
        /// Return the year.
        /// </summary>
        /// <returns>the year</returns>
        public int Year()
        {
            return _year;
        }

        /// <summary>
        /// Is the given date valid?
        /// </summary>
        /// <param name="m"></param>
        /// <param name="d"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static bool IsValid(int m, int d, int y)
        {
            if (m < 1 || m > 12)
            {
                return false;
            }
            if (d < 1 || d > Days[m])
            {
                return false;
            }
            return !(m == 2 && d == 29 && !IsLeapYear(y));
        }

        /// <summary>
        /// Is year y a leap year?
        /// </summary>
        /// <param name="y"></param>
        /// <returns>true if y is a leap year; false otherwise</returns>
        private static bool IsLeapYear(int y)
        {
            if (y % 400 == 0)
            {
                return true;
            }
            if (y % 100 == 0)
            {
                return false;
            }
            return y % 4 == 0;
        }

        /// <summary>
        /// Returns the next date in the calendar.
        /// </summary>
        /// <returns>a date that represents the next day after this day</returns>
        public Date Next()
        {
            if (IsValid(_month, _day + 1, _year))
            {
                return new Date(_month, _day + 1, _year);
            }
            if (IsValid(_month + 1, 1, _year))
            {
                return new Date(_month + 1, 1, _year);
            }
            return new Date(1, 1, _year + 1);
        }

        /// <summary>
        /// Is this date after b?
        /// </summary>
        /// <param name="b"></param>
        /// <returns>true if this date is after date b; false otherwise</returns>
        public bool IsAfter(Date b)
        {
            return CompareTo(b) > 0;
        }

        /// <summary>
        /// Is this date before b?
        /// </summary>
        /// <param name="b"></param>
        /// <returns>true if this date is before date b; false otherwise</returns>
        public bool IsBefore(Date b)
        {
            return CompareTo(b) < 0;
        }

        /// <summary>
        /// Compare this date to that date.
        /// </summary>
        /// <param name="that"></param>
        /// <returns>{ a negative integer, zero, or a positive integer }, depending on whether this date is { before, equal to, after } that date</returns>
        public int CompareTo(Date that)
        {
            if (_year < that._year)
            {
                return -1;
            }
            if (_year > that._year)
            {
                return +1;
            }
            if (_month < that._month)
            {
                return -1;
            }
            if (_month > that._month)
            {
                return +1;
            }
            if (_day < that._day)
            {
                return -1;
            }
            if (_day > that._day)
            {
                return +1;
            }
            return 0;
        }

        /// <summary>
        /// Return a string representation of this date.
        /// </summary>
        /// <returns>the string representation in the foramt MM/DD/YYYY</returns>
        public override string ToString()
        {
            return _month + "/" + _day + "/" + _year;
        }

        /// <summary>
        /// Is this date equal to x?
        /// </summary>
        /// <param name="x"></param>
        /// <returns>true if this date equals x; false otherwise</returns>
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
            Date that = (Date)x;
            return (_month == that._month) && (_day == that._day) && (_year == that._year);
        }

        /// <summary>
        /// Return a hash code.
        /// </summary>
        /// <returns>a hash code for this date</returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = 31 * hash + _month;
            hash = 31 * hash + _day;
            hash = 31 * hash + _year;
            return hash;
        }

        /// <summary>
        /// Unit tests the date data type.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            Date today = new Date(2, 25, 2004);
            StdOut.PrintLn(today);
            for (int i = 0; i < 10; i++)
            {
                today = today.Next();
                StdOut.PrintLn(today);
            }

            StdOut.PrintLn(today.IsAfter(today.Next()));
            StdOut.PrintLn(today.IsAfter(today));
            StdOut.PrintLn(today.Next().IsAfter(today));

            Date birthday = new Date(10, 16, 1971);
            StdOut.PrintLn(birthday);
            for (int i = 0; i < 10; i++)
            {
                birthday = birthday.Next();
                StdOut.PrintLn(birthday);
            }
        }
    }
}