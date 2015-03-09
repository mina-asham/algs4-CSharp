using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace algs4.stdlib
{
    public class In
    {
        private static TextReader _reader;
        /// <summary>
        /// Create an input stream from standard input.
        /// </summary>
        public In()
        {
            Console.InputEncoding = Encoding.UTF8;
            _reader = Console.In;
        }

        /// <summary>
        /// Create an input stream from a socket.
        /// </summary>
        /// <param name="socket"></param>
        public In(Socket socket)
        {
            try
            {
                _reader = new StreamReader(new NetworkStream(socket), Encoding.UTF8);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not open " + socket);
            }
        }

        /// <summary>
        /// Create an input stream from a URL.
        /// </summary>
        /// <param name="uri"></param>
        public In(Uri uri)
        {
            try
            {
                WebRequest request = WebRequest.Create(uri);
                Stream stream = request.GetResponse().GetResponseStream();
                if (stream == null)
                {
                    throw new IOException();
                }
                _reader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not open " + uri);
            }
        }


        /// <summary>
        /// Create an input stream from a file.
        /// </summary>
        /// <param name="file"></param>
        public In(FileStream file)
        {
            try
            {
                _reader = new StreamReader(file, Encoding.UTF8);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not open " + file);
            }
        }


        /// <summary>
        /// Create an input stream from a filename or web page name.
        /// </summary>
        /// <param name="s"></param>
        public In(string s)
        {
            try
            {
                // first try to read file from local file system
                // OR
                // next try for files included input jar
                if (File.Exists(s))
                {
                    _reader = new StreamReader(s, Encoding.UTF8);
                    return;
                }

                // or URL from web
                WebRequest request = WebRequest.Create(s);
                Stream stream = request.GetResponse().GetResponseStream();
                if (stream == null)
                {
                    throw new IOException();
                }
                _reader = new StreamReader(stream, Encoding.UTF8);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not open " + s);
            }
        }

        /// <summary>
        /// Create an input stream from a given Stream source; use with
        /// new StreamReader(Stream) to read from a string.
        /// Note that this does not create a defensive copy, so the
        /// stream will be mutated as you read on. 
        /// </summary>
        /// <param name="stream"></param>
        public In(Stream stream)
        {
            _reader = new StreamReader(stream);
        }

        /// <summary>
        /// Does the input stream exist?
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return _reader != null;
        }

        /// <summary>
        /// Is the input empty? Use this
        /// to know whether the next call
        /// to read[Type] will succeed.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return _reader.Peek() < 0;
        }

        /// <summary>
        /// Does the input have a next line?
        /// </summary>
        /// <returns>true if standard input is empty, and false otherwise</returns>
        public bool HasNextLine()
        {
            return IsEmpty();
        }

        /// <summary>
        /// Is the input empty?
        /// </summary>
        /// <returns>true if standard input is empty, and false otherwise</returns>
        public bool HasNextChar()
        {
            return IsEmpty();
        }

        /// <summary>
        /// Reads and returns the next line, excluding the line separator if present.
        /// </summary>
        /// <returns>the next line, excluding the line separator if present</returns>
        public String ReadLine()
        {
            return _reader.ReadLine();
        }

        /// <summary>
        /// Reads and returns the next character.
        /// </summary>
        /// <returns>the next character</returns>
        public char ReadChar()
        {
            return (char)_reader.Read();
        }

        /// <summary>
        /// Reads and returns the remainder of the input, as a string.
        /// </summary>
        /// <returns>the remainder of the input, as a string</returns>
        public String ReadAll()
        {
            return _reader.ReadToEnd();
        }

        /// <summary>
        /// Reads the next token  and returns the String.
        /// </summary>
        /// <returns>the next String</returns>
        public String ReadString()
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
        private void ReadCharactersWhilePossible(StringBuilder builder)
        {
            while (!char.IsWhiteSpace((char)_reader.Peek()))
            {
                int character = _reader.Read();
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
        private void SkipWhitespace()
        {
            while (!IsEmpty() && char.IsWhiteSpace((char)_reader.Peek()))
            {
                _reader.Read();
            }
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as an integer, and returns the integer.
        /// </summary>
        /// <returns>the next integer on standard input</returns>
        public int ReadInt()
        {
            return int.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a double, and returns the double.
        /// </summary>
        /// <returns>the next double on standard input</returns>
        public double ReadDouble()
        {
            return double.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a float, and returns the float.
        /// </summary>
        /// <returns>the next float on standard input</returns>
        public float ReadFloat()
        {
            return float.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a long integer, and returns the long integer.
        /// </summary>
        /// <returns>the next long integer on standard input</returns>
        public long ReadLong()
        {
            return long.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a short integer, and returns the short integer.
        /// </summary>
        /// <returns>the next short integer on standard input</returns>
        public short ReadShort()
        {
            return short.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a byte, and returns the byte.
        /// </summary>
        /// <returns>the next byte on standard input</returns>
        public byte ReadByte()
        {
            return byte.Parse(ReadString());
        }

        /// <summary>
        /// Reads the next token from standard input, parses it as a boolean,
        /// and returns the boolean.
        /// </summary>
        /// <returns>the next boolean on standard input</returns>
        public bool ReadBoolean()
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
        public String[] ReadAllStrings()
        {
            return ReadAll().Trim().Split();
        }

        /// <summary>
        /// Reads all remaining lines from standard input and returns them as an array of strings.
        /// </summary>
        /// <returns>all remaining lines on standard input, as an array of strings</returns>
        public String[] ReadAllLines()
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
        public int[] ReadAllInts()
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
        public double[] ReadAllDoubles()
        {
            String[] fields = ReadAllStrings();
            double[] vals = new double[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                vals[i] = double.Parse(fields[i]);
            return vals;
        }

        /// <summary>
        /// Close the input stream.
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }

        /// <summary>
        /// Reads all ints from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Obsolete("Clearer to use new In(filename).ReadAllInts()")]
        public static int[] ReadInts(string filename)
        {
            return new In(filename).ReadAllInts();
        }

        /// <summary>
        /// Reads all doubles from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Obsolete("Clearer to use new In(filename).ReadAllDoubles()")]
        public static double[] ReadDoubles(string filename)
        {
            return new In(filename).ReadAllDoubles();
        }

        /// <summary>
        /// Reads all strings from a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [Obsolete("Clearer to use new In(filename).ReadAllStrings()")]
        public static String[] ReadStrings(string filename)
        {
            return new In(filename).ReadAllStrings();
        }

        /// <summary>
        /// Reads all ints from stdin
        /// </summary>
        /// <returns></returns>
        [Obsolete("Clearer to user new In().ReadAllInts()")]
        public static int[] ReadInts()
        {
            return new In().ReadAllInts();
        }

        /// <summary>
        /// Reads all doubles from stdin
        /// </summary>
        /// <returns></returns>
        [Obsolete("Clearer to user new In().ReadAllDoubles()")]
        public static double[] ReadDoubles()
        {
            return new In().ReadAllDoubles();
        }

        /// <summary>
        /// Reads all strings from stdin
        /// </summary>
        /// <returns></returns>
        [Obsolete("Clearer to user new In().ReadAllStrings()")]
        public static String[] ReadStrings()
        {
            return new In().ReadAllStrings();
        }

        /// <summary>
        /// Test client.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(String[] args)
        {
            In input;
            const string urlName = "http://introcs.cs.princeton.edu/stdlib/InTest.txt";

            // read from a URL
            Console.WriteLine("readAll() from URL " + urlName);
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In(urlName);
                Console.WriteLine(input.ReadAll());
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();

            // read one line at a time from URL
            Console.WriteLine("readLine() from URL " + urlName);
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In(urlName);
                while (!input.IsEmpty())
                {
                    string s = input.ReadLine();
                    Console.WriteLine(s);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();

            // read one string at a time from URL
            Console.WriteLine("readString() from URL " + urlName);
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In(urlName);
                while (!input.IsEmpty())
                {
                    string s = input.ReadString();
                    Console.WriteLine(s);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();

            // read one line at a time from file input current directory
            Console.WriteLine("readLine() from current directory");
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In(Path.Combine(".", "stdlib", "InTest.txt"));
                while (!input.IsEmpty())
                {
                    string s = input.ReadLine();
                    Console.WriteLine(s);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();

            // read one line at a time from file using relative path
            Console.WriteLine("readLine() from relative path");
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In(Path.Combine("..", new DirectoryInfo(Directory.GetCurrentDirectory()).Name, "stdlib", "InTest.txt"));
                while (!input.IsEmpty())
                {
                    string s = input.ReadLine();
                    Console.WriteLine(s);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();

            // read one char at a time
            Console.WriteLine("readChar() from file");
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In(Path.Combine("stdlib", "InTest.txt"));
                while (!input.IsEmpty())
                {
                    char c = input.ReadChar();
                    Console.Write(c);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();
            Console.WriteLine();

            // read one line at a time from absolute OS X / Linux path
            Console.WriteLine("readLine() from absolute OS X / Linux path");
            Console.WriteLine("---------------------------------------------------------------------------");
            input = new In("/n/fs/introcs/www/java/stdlib/InTest.txt");
            try
            {
                while (!input.IsEmpty())
                {
                    string s = input.ReadLine();
                    Console.WriteLine(s);
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();

            // read one line at a time from absolute Windows path
            Console.WriteLine("readLine() from absolute Windows path");
            Console.WriteLine("---------------------------------------------------------------------------");
            try
            {
                input = new In("G:\\www\\introcs\\stdlib\\InTest.txt");
                while (!input.IsEmpty())
                {
                    string s = input.ReadLine();
                    Console.WriteLine(s);
                }
                Console.WriteLine();
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine();
        }
    }
}
