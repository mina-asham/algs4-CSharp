using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace algs4.stdlib
{
    public class BinaryIn
    {
        /// <summary>
        /// The input stream
        /// </summary>
        private readonly BinaryReader _input;

        /// <summary>
        /// Create a binary input stream from standard input.
        /// </summary>
        public BinaryIn()
        {
            _input = new BinaryReader(Console.OpenStandardInput());
        }

        /// <summary>
        /// Create a binary input stream from an InputStream.
        /// </summary>
        /// <param name="stream"></param>
        public BinaryIn(Stream stream)
        {
            _input = new BinaryReader(stream);
        }

        /// <summary>
        /// Create a binary input stream from a socket.
        /// </summary>
        /// <param name="socket"></param>
        public BinaryIn(Socket socket)
        {
            try
            {
                _input = new BinaryReader(new NetworkStream(socket));
            }
            catch (IOException)
            {
                Console.WriteLine("Could not open " + socket);
            }
        }

        /// <summary>
        /// Create a binary input stream from a URL.
        /// </summary>
        /// <param name="uri"></param>
        public BinaryIn(Uri uri)
        {
            try
            {
                WebRequest request = WebRequest.Create(uri);
                Stream stream = request.GetResponse().GetResponseStream();
                if (stream == null)
                {
                    throw new IOException();
                }
                _input = new BinaryReader(stream);
            }
            catch (IOException)
            {
                Console.WriteLine("Could not open " + uri);
            }
        }

        /// <summary>
        /// Create a binary input stream from a filename or URL name.
        /// </summary>
        /// <param name="s"></param>
        public BinaryIn(string s)
        {
            try
            {
                // first try to read file from local file system
                // OR
                // next try for files included input DLL
                if (File.Exists(s))
                {
                    _input = new BinaryReader(File.OpenRead(s));
                    return;
                }

                // or URL from web
                WebRequest request = WebRequest.Create(s);
                Stream stream = request.GetResponse().GetResponseStream();
                if (stream == null)
                {
                    throw new IOException();
                }
                _input = new BinaryReader(stream, Encoding.UTF8);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Could not open " + s);
            }
        }

        /// <summary>
        /// Does the binary input stream exist?
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return _input != null;
        }

        /// <summary>
        /// Returns true if the binary input stream is empty.
        /// </summary>
        /// <returns>true if and only if the binary input stream is empty</returns>
        public bool IsEmpty()
        {
            return _input.BaseStream.Position != _input.BaseStream.Length;
        }

        /// <summary>
        /// Read the next bit of data from the binary input stream and return as a boolean.
        /// </summary>
        /// <returns>the next bit of data from the binary input stream as a <tt>boolean</tt></returns>
        public bool ReadBoolean()
        {
            return _input.ReadBoolean();
        }

        /// <summary>
        /// Read the next 8 bits from the binary input stream and return as an 8-bit char.
        /// </summary>
        /// <returns>the next 8 bits of data from the binary input stream as a <tt>char</tt></returns>
        public char ReadChar()
        {
            return _input.ReadChar();
        }


        /// <summary>
        /// Read the remaining bytes of data from the binary input stream and return as a string.
        /// </summary>
        /// <returns>the remaining bytes of data from the binary input stream as a <tt>string</tt></returns>
        public string Readstring()
        {
            return _input.ReadString();
        }


        /// <summary>
        /// Read the next 16 bits from the binary input stream and return as a 16-bit short.
        /// </summary>
        /// <returns>the next 16 bits of data from the binary standard input as a <tt>short</tt></returns>
        public short ReadShort()
        {
            return _input.ReadInt16();
        }

        /// <summary>
        /// Read the next 32 bits from the binary input stream and return as a 32-bit int.
        /// </summary>
        /// <returns>the next 32 bits of data from the binary input stream as a <tt>int</tt></returns>
        public int ReadInt()
        {
            return _input.ReadInt32();
        }


        /// <summary>
        /// Read the next 64 bits from the binary input stream and return as a 64-bit long.
        /// </summary>
        /// <returns>the next 64 bits of data from the binary input stream as a <tt>long</tt></returns>
        public long ReadLong()
        {
            return _input.ReadInt64();
        }

        /// <summary>
        /// Read the next 64 bits from the binary input stream and return as a 64-bit double.
        /// </summary>
        /// <returns>the next 64 bits of data from the binary input stream as a <tt>double</tt></returns>
        public double ReadDouble()
        {
            return _input.ReadDouble();
        }

        /// <summary>
        /// Read the next 32 bits from standard input and return as a 32-bit float.
        /// </summary>
        /// <returns>the next 32 bits of data from standard input as a <tt>float</tt></returns>
        public float ReadFloat()
        {
            return _input.ReadSingle();
        }


        /// <summary>
        /// Read the next 8 bits from the binary input stream and return as an 8-bit byte.
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return _input.ReadByte();
        }

        /// <summary>
        /// Test client. Reads input the name of a file or uri (first command-line
        /// argument) and writes it to a file (second command-line argument).
        /// </summary>
        /// <param name="args">Main arguments</param>
        public static void RunMain(string[] args)
        {
            BinaryIn input = new BinaryIn(args[0]);
            BinaryOut output = new BinaryOut(args[1]);

            // read one 8-bit char at a time
            while (!input.IsEmpty())
            {
                char c = input.ReadChar();
                output.Write(c);
            }
            output.Flush();
        }
    }
}
