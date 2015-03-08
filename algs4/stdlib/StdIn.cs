using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace algs4.stdlib
{
    public static class StdIn
    {
        /// <summary>
        /// Standard input reader
        /// </summary>
        private static readonly TextReader Reader;

        /// <summary>
        /// Run only to initialize
        /// </summary>
        static StdIn()
        {
            Console.InputEncoding = Encoding.UTF8;
            Reader = Console.In;
        }

        /// <summary>
        /// Is the input empty? Use this
        /// to know whether the next call
        /// to read[Type] will succeed.
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty()
        {
            return Reader.Peek() < 0;
        }

        /// <summary>
        /// Does the input have a next line?
        /// </summary>
        /// <returns>true if standard input is empty, and false otherwise</returns>
        public static bool HasNextLine()
        {
            return IsEmpty();
        }

        /// <summary>
        /// Is the input empty?
        /// </summary>
        /// <returns>true if standard input is empty, and false otherwise</returns>
        public static bool HasNextChar()
        {
            return IsEmpty();
        }

        /// <summary>
        /// Reads and returns the next line, excluding the line separator if present.
        /// </summary>
        /// <returns>the next line, excluding the line separator if present</returns>
        public static String ReadLine()
        {
            return Reader.ReadLine();
        }

        /// <summary>
        /// Reads and returns the next character.
        /// </summary>
        /// <returns>the next character</returns>
        public static char ReadChar()
        {
            return (char)Reader.Read();
        }

        /// <summary>
        /// Reads and returns the remainder of the input, as a string.
        /// </summary>
        /// <returns>the remainder of the input, as a string</returns>
        public static String ReadAll()
        {
            return Reader.ReadToEnd();
        }

        /// <summary>
        /// Reads the next token  and returns the String.
        /// </summary>
        /// <returns>the next String</returns>
        public static String ReadString()
        {
            StringBuilder builder = new StringBuilder();
            SkipWhitespace();
            ReadCharactersWhilePossible(builder);
            SkipWhitespace();
            return builder.ToString();
        }

        /// <summary>
        /// Helper for ReadString method, to read characters while possible
        /// </summary>
        /// <param name="builder"></param>
        private static void ReadCharactersWhilePossible(StringBuilder builder)
        {
            while (!char.IsWhiteSpace((char)Reader.Peek()))
            {
                int character = Reader.Read();
                if (character < 0)
                {
                    break;
                }
                builder.Append((char)character);
            }
        }

        /// <summary>
        /// Helper for ReadString method, to skip whitespace characters
        /// </summary>
        private static void SkipWhitespace()
        {
            while (!IsEmpty() && char.IsWhiteSpace((char)Reader.Peek()))
            {
                Reader.Read();
            }
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as an integer, and returns the integer.
        /// </summary>
        /// <returns>the next integer on standard input</returns>
        public static int ReadInt()
        {
            return int.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a double, and returns the double.
        /// </summary>
        /// <returns>the next double on standard input</returns>
        public static double ReadDouble()
        {
            return double.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a float, and returns the float.
        /// </summary>
        /// <returns>the next float on standard input</returns>
        public static float ReadFloat()
        {
            return float.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a long integer, and returns the long integer.
        /// </summary>
        /// <returns>the next long integer on standard input</returns>
        public static long ReadLong()
        {
            return long.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a short integer, and returns the short integer.
        /// </summary>
        /// <returns>the next short integer on standard input</returns>
        public static short ReadShort()
        {
            return short.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a byte, and returns the byte.
        /// </summary>
        /// <returns>the next byte on standard input</returns>
        public static byte ReadByte()
        {
            return byte.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a boolean,
        /// and returns the boolean.
        /// </summary>
        /// <returns>the next boolean on standard input</returns>
        public static bool ReadBoolean()
        {
            String s = ReadString().ToLower();
            if (s.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase) || s == "1")
            {
                return true;
            }
            if (s.Equals(bool.FalseString, StringComparison.InvariantCultureIgnoreCase) || s == "0")
            {
                return false;
            }
            throw new FormatException();
        }

        /// <summary>
        /// Reads all remaining tokens from standard input and returns them as an array of strings.
        /// </summary>
        /// <returns>all remaining tokens on standard input, as an array of strings</returns>
        public static String[] ReadAllStrings()
        {
            return ReadAll().Trim().Split();
        }

        /// <summary>
        /// Reads all remaining lines from standard input and returns them as an array of strings.
        /// </summary>
        /// <returns>all remaining lines on standard input, as an array of strings</returns>
        public static String[] ReadAllLines()
        {
            List<String> lines = new List<String>();
            while (HasNextLine())
            {
                lines.Add(ReadLine());
            }
            return lines.ToArray();
        }

        /// <summary>
        /// Reads all remaining tokens from standard input, parses them as integers, and returns
        /// them as an array of integers.
        /// </summary>
        /// <returns>all remaining integers on standard input, as an array</returns>
        public static int[] ReadAllInts()
        {
            String[] fields = ReadAllStrings();
            int[] vals = new int[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                vals[i] = int.Parse(fields[i]);
            return vals;
        }

        /// <summary>
        /// Reads all remaining tokens from standard input, parses them as doubles, and returns
        /// them as an array of doubles.
        /// </summary>
        /// <returns>all remaining doubles on standard input, as an array</returns>
        public static double[] ReadAllDoubles()
        {
            String[] fields = ReadAllStrings();
            double[] vals = new double[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                vals[i] = double.Parse(fields[i]);
            return vals;
        }

        /// <summary>
        /// Reads all remaining tokens, parses them as integers, and returns
        /// them as an array of integers.
        /// </summary>
        /// <returns>all remaining integers, as an array</returns>
        public static int[] ReadInts()
        {
            return ReadAllInts();
        }

        /// <summary>
        /// Reads all remaining tokens, parses them as doubles, and returns
        /// them as an array of doubles.</summary>
        /// <returns>all remaining doubles, as an array</returns>
        public static double[] ReadDoubles()
        {
            return ReadAllDoubles();
        }

        /// <summary>
        /// Reads all remaining tokens and returns them as an array of strings.
        /// </summary>
        /// <returns>all remaining tokens, as an array of strings</returns>
        public static String[] ReadStrings()
        {
            return ReadAllStrings();
        }

        /// <summary>
        /// Interactive test of basic functionality.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(String[] args)
        {
            Console.WriteLine("Type a string: ");
            String s = ReadString();
            Console.WriteLine("Your string was: " + s);
            Console.WriteLine();

            Console.WriteLine("Type an int: ");
            int a = ReadInt();
            Console.WriteLine("Your int was: " + a);
            Console.WriteLine();

            Console.WriteLine("Type a boolean: ");
            bool b = ReadBoolean();
            Console.WriteLine("Your boolean was: " + b);
            Console.WriteLine();

            Console.WriteLine("Type a double: ");
            double c = ReadDouble();
            Console.WriteLine("Your double was: " + c);
            Console.WriteLine();
        }
    }
}
