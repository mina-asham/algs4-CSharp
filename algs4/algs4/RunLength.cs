using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class RunLength
    {
        private const int R = 256;

        public static void Expand()
        {
            bool b = false;
            while (!BinaryStdIn.IsEmpty())
            {
                int run = BinaryStdIn.ReadByte();
                for (int i = 0; i < run; i++)
                {
                    BinaryStdOut.Write(b);
                }
                b = !b;
            }
            BinaryStdOut.Close();
        }

        public static void Compress()
        {
            byte run = 0;
            bool old = false;
            while (!BinaryStdIn.IsEmpty())
            {
                bool b = BinaryStdIn.ReadBoolean();
                if (b != old)
                {
                    BinaryStdOut.Write(run);
                    run = 1;
                    old = !old;
                }
                else
                {
                    if (run == R - 1)
                    {
                        BinaryStdOut.Write(run);
                        run = 0;
                        BinaryStdOut.Write(run);
                    }
                    run++;
                }
            }
            BinaryStdOut.Write(run);
            BinaryStdOut.Close();
        }

        public static void RunMain(string[] args)
        {
            if (args[0] == "-")
            {
                Compress();
            }
            else if (args[0] == "+")
            {
                Expand();
            }
            else
            {
                throw new ArgumentException("Illegal command line argument");
            }
        }
    }
}