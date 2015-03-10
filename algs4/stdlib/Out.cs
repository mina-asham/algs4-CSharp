using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace algs4.stdlib
{
    public class Out
    {
        private readonly TextWriter _output;

        /// <summary>
        /// Create an Out object using an OutputStream.
        /// </summary>
        /// <param name="stream"></param>
        public Out(Stream stream)
        {
            try
            {
                _output = new StreamWriter(stream);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);

            }
        }

        /// <summary>
        /// Create an Out object using standard output.
        /// </summary>
        public Out()
        {
            Console.OutputEncoding = Encoding.UTF8;
            _output = Console.Out;
        }

        /// <summary>
        /// Create an Out object using a Socket.
        /// </summary>
        /// <param name="socket"></param>
        public Out(Socket socket)
        {
            try
            {
                _output = new StreamWriter(new NetworkStream(socket));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);

            }
        }

        /// <summary>
        /// Create an Out object using a file specified by the given name.
        /// </summary>
        /// <param name="s"></param>
        public Out(string s)
        {
            try
            {
                _output = new StreamWriter(s);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// Close the output stream.
        /// </summary>
        public void Close()
        {
            _output.Close();
        }

        /// <summary>
        /// Terminate the line.
        /// </summary>
        public void PrintLn()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Print an object and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(Object x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print a boolean and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(bool x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print a char and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(char x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print an double and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(double x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print a float and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(float x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print an int and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(int x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print a long and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(long x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Print a byte and then terminate the line.
        /// </summary>
        /// <param name="x"></param>
        public void PrintLn(byte x)
        {
            Console.WriteLine(x);
        }

        /// <summary>
        /// Flush the output stream.
        /// </summary>
        public void Print()
        {
            _output.Flush();
        }

        /// <summary>
        /// Print an object and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(Object x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print an boolean and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(bool x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print an char and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(char x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print an double and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(double x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print a float and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(float x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print an int and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(int x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print a long and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(long x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print a byte and then flush the output stream.
        /// </summary>
        /// <param name="x"></param>
        public void Print(byte x)
        {
            Console.Write(x);
            _output.Flush();
        }

        /// <summary>
        /// Print a formatted string using the specified format string and arguments,
        /// and then flush the output stream.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void PrintF(string format, params object[] args)
        {
            _output.Write(format, args);
            _output.Flush();
        }

        /// <summary>
        /// A test client.
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            Out output;

            // write to stdout
            output = new Out();
            Console.WriteLine("Test 1");
            output.Close();

            // write to a file
            output = new Out("test.txt");
            Console.WriteLine("Test 2");
            output.Close();
        }
    }
}
