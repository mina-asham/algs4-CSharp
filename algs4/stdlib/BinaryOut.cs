using System;
using System.IO;
using System.Net.Sockets;

namespace algs4.stdlib
{
    public class BinaryOut
    {
        /// <summary>
        /// The output stream
        /// </summary>
        private readonly BinaryWriter _output;

        /// <summary>
        /// Create a binary output stream from an OutputStream.
        /// </summary>
        /// <param name="stream"></param>
        public BinaryOut(Stream stream)
        {
            _output = new BinaryWriter(stream);
        }

        /// <summary>
        /// Create a binary output stream from standard output.
        /// </summary>
        public BinaryOut()
        {
            _output = new BinaryWriter(Console.OpenStandardOutput());
        }

        /// <summary>
        /// Create a binary output stream from a filename.
        /// </summary>
        /// <param name="s"></param>
        public BinaryOut(string s)
        {
            try
            {
                _output = new BinaryWriter(File.OpenWrite(s));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Create a binary output stream from a Socket.
        /// </summary>
        /// <param name="socket"></param>
        public BinaryOut(Socket socket)
        {
            try
            {
                _output = new BinaryWriter(new NetworkStream(socket));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Flush the binary output stream, padding 0s if number of bits written so far
        /// is not a multiple of 8.
        /// </summary>
        public void Flush()
        {
            try
            {
                _output.Flush();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Close and flush the binary output stream. Once it is closed, you can no longer write bits.
        /// </summary>
        public void Close()
        {
            Flush();
            try
            {
                _output.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Write the specified bit to the binary output stream.
        /// </summary>
        /// <param name="x">the boolean to write.</param>
        public void Write(bool x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 8-bit byte to the binary output stream.
        /// </summary>
        /// <param name="x">the byte to write.</param>
        public void Write(byte x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 32-bit int to the binary output stream.
        /// </summary>
        /// <param name="x">the int to write.</param>
        public void Write(int x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 64-bit double to the binary output stream.
        /// </summary>
        /// <param name="x">the double to write.</param>
        public void Write(double x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 64-bit long to the binary output stream.
        /// </summary>
        /// <param name="x">the long to write.</param>
        public void Write(long x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 32-bit float to the binary output stream.
        /// </summary>
        /// <param name="x">the float to write.</param>
        public void Write(float x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 16-bit int to the binary output stream.
        /// </summary>
        /// <param name="x">the short to write.</param>
        public void Write(short x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the 8-bit char to the binary output stream.
        /// </summary>
        /// <param name="x">the char to write.</param>
        public void Write(char x)
        {
            _output.Write(x);
        }

        /// <summary>
        /// Write the string of 8-bit characters to the binary output stream.
        /// </summary>
        /// <param name="s">the string to write.</param>
        public void Write(string s)
        {
            _output.Write(s);
        }

        /// <summary>
        /// Test client. Read bits from standard input and write to the file
        /// specified on command line.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            // create binary output stream to write to file
            string filename = args[0];
            BinaryOut output = new BinaryOut(filename);
            BinaryIn input = new BinaryIn();

            // read from standard input and write to file
            while (!input.IsEmpty())
            {
                char c = input.ReadChar();
                output.Write(c);
            }
            output.Flush();
        }
    }
}
