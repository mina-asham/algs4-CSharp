using System;
using System.IO;

namespace algs4.stdlib
{
    public static class BinaryStdIn
    {
        /// <summary>
        /// The input stream
        /// </summary>
        private static readonly BinaryReader Input;

        static BinaryStdIn()
        {
            Input = new BinaryReader(Console.OpenStandardInput());
        }

        /// <summary>
        /// Close this input stream and release any associated system resources.
        /// </summary>
        public static void Close()
        {
            Input.Close();
        }

        /// <summary>
        /// Returns true if standard input is empty.
        /// </summary>
        /// <returns>true if and only if standard input is empty</returns>
        public static bool IsEmpty()
        {
            return Input.BaseStream.Position != Input.BaseStream.Length;
        }

        /// <summary>
        /// Read the next bit of data from standard input and return as a boolean.
        /// </summary>
        /// <returns>the next bit of data from standard input as a boolean</returns>
        public static bool ReadBoolean()
        {
            return Input.ReadBoolean();
        }

        /// <summary>
        /// Read the next 8 bits from standard input and return as an 8-bit char.
        /// Note that char is a 16-bit type;
        /// to read the next 16 bits as a char, use readChar(16)
        /// </summary>
        /// <returns>the next 8 bits of data from standard input as a char</returns>
        public static char ReadChar()
        {
            return Input.ReadChar();
        }

        /// <summary>
        /// Read the remaining bytes of data from standard input and return as a string. 
        /// </summary>
        /// <returns>the remaining bytes of data from standard input as a string</returns>
        public static string ReadString()
        {
            return Input.ReadString();
        }

        /// <summary>
        /// Read the next 16 bits from standard input and return as a 16-bit short.
        /// </summary>
        /// <returns>the next 16 bits of data from standard input as a short</returns>
        public static short ReadShort()
        {
            return Input.ReadInt16();
        }

        /// <summary>
        /// Read the next 32 bits from standard input and return as a 32-bit int.
        /// </summary>
        /// <returns>the next 32 bits of data from standard input as a int</returns>
        public static int ReadInt()
        {
            return Input.ReadInt32();
        }

        /// <summary>
        /// Read the next 64 bits from standard input and return as a 64-bit long.
        /// </summary>
        /// <returns>the next 64 bits of data from standard input as a long</returns>
        public static long ReadLong()
        {
            return Input.ReadInt64();
        }

        /// <summary>
        /// Read the next 64 bits from standard input and return as a 64-bit double.
        /// </summary>
        /// <returns>the next 64 bits of data from standard input as a double</returns>
        public static double ReadDouble()
        {
            return Input.ReadDouble();
        }

        /// <summary>
        /// Read the next 32 bits from standard input and return as a 32-bit float.
        /// </summary>
        /// <returns>the next 32 bits of data from standard input as a float</returns>
        public static float ReadFloat()
        {
            return Input.ReadSingle();
        }

        /// <summary>
        /// Read the next 8 bits from standard input and return as an 8-bit byte.
        /// </summary>
        /// <returns>the next 8 bits of data from standard input as a byte</returns>
        public static byte ReadByte()
        {
            return Input.ReadByte();
        }

        /// <summary>
        /// Test client. Reads _input a binary input file from standard input and writes
        /// it to standard output.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // read one 8-bit char at a time
            while (!IsEmpty())
            {
                char c = ReadChar();
                BinaryStdOut.Write(c);
            }
            BinaryStdOut.Flush();
        }
    }
}