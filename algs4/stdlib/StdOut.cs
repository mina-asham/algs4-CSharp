using System;
using System.IO;
using System.Text;

namespace algs4.stdlib
{
    public static class StdOut
    {
        /// <summary>
        /// Standard input reader
        /// </summary>
        private static readonly TextWriter Writer;

        /// <summary>
        /// Run only to initialize
        /// </summary>
        static StdOut()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Writer = Console.Out;
        }

        /// <summary>
        /// Close standard output.
        /// </summary>
        public static void Close()
        {
            Writer.Close();
        }

        /// <summary>
        /// Terminate the current line by Writeing the line separator string.
        /// </summary>
        public static void PrintLn()
        {
            Writer.WriteLine();
        }

        /// <summary>
        /// Write an object to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(object x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a bool to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(bool x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a char to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(char x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a double to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(double x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a float to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(float x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write an int to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(int x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a long to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(long x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a short to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(short x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Write a byte to standard output and then terminate the line.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void PrintLn(byte x)
        {
            Writer.WriteLine(x);
        }

        /// <summary>
        /// Flush standard output.
        /// </summary>
        public static void Print()
        {
            Writer.Flush();
        }

        /// <summary>
        /// Write an object to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(object x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a bool to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(bool x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a char to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(char x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a double to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(double x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a float to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(float x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write an int to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(int x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a long to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(long x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a short to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(short x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a byte to standard output and flush standard output.
        /// </summary>
        /// <param name="x">object to print</param>
        public static void Print(byte x)
        {
            Writer.Write(x);
            Writer.Flush();
        }

        /// <summary>
        /// Write a formatted string to standard output using the specified
        /// format string and arguments, and flush standard output.
        /// </summary>
        /// <param name="format">string format</param>
        /// <param name="args">arguments to fill in string format</param>
        public static void PrintF(string format, params object[] args)
        {
            Writer.Write(format, args);
            Writer.Flush();
        }

        /// <summary>
        /// This method is just here to test the class
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            // write to stdout
            PrintLn("Test");
            PrintLn(17);
            PrintLn(true);
            PrintF("{0:0.000000}\n", 1.0 / 7.0);
        }
    }
}
