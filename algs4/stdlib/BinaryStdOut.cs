using System;
using System.IO;

namespace algs4.stdlib
{
    public static class BinaryStdOut
    {
        /// <summary>
        /// The output stream
        /// </summary>
        private static readonly BinaryWriter Output;

        static BinaryStdOut()
        {
            Output = new BinaryWriter(Console.OpenStandardOutput());
        }

        /// <summary>
        /// Flush standard output stream, padding 0s if number of bits written so far
        /// is not a multiple of 8.
        /// </summary>
        public static void Flush()
        {
            try
            {
                Output.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Flush and close standard output. Once standard output is closed, you can no
        /// longer write bits to it.
        /// </summary>
        public static void Close()
        {
            Flush();
            try
            {
                Output.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Write the specified bit to standard output.
        /// </summary>
        /// <param name="x">the boolean to write.</param>
        public static void Write(bool x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 8-bit byte to standard output.
        /// </summary>
        /// <param name="x">the byte to write.</param>
        public static void Write(byte x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 32-bit int to standard output.
        /// </summary>
        /// <param name="x">the int to write.</param>
        public static void Write(int x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 64-bit double to standard output.
        /// </summary>
        /// <param name="x">the double to write.</param>
        public static void Write(double x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 64-bit long to standard output.
        /// </summary>
        /// <param name="x">the long to write.</param>
        public static void Write(long x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 32-bit float to standard output.
        /// </summary>
        /// <param name="x">the float to write.</param>
        public static void Write(float x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 16-bit int to standard output.
        /// </summary>
        /// <param name="x">the short to write.</param>
        public static void Write(short x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the 8-bit char to standard output.
        /// </summary>
        /// <param name="x">the char to write.</param>
        public static void Write(char x)
        {
            Output.Write(x);
        }

        /// <summary>
        /// Write the string of 8-bit characters to standard output.
        /// </summary>
        /// <param name="s">the string to write.</param>
        public static void Write(string s)
        {
            Output.Write(s);
        }


        /// <summary>
        /// Test client.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            int T = int.Parse(args[0]);
            // write to standard output
            for (int i = 0; i < T; i++)
            {
                Write(i);
            }
            Flush();
        }
    }
}
