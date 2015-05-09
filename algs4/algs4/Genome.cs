using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Genome
    {
        public static void Compress()
        {
            Alphabet dna = new Alphabet("ACTG");
            string s = BinaryStdIn.ReadString();
            int n = s.Length;
            BinaryStdOut.Write(n);

            // Write two-bit code for char. 
            for (int i = 0; i < n; i++)
            {
                byte d = (byte)dna.ToIndex(s[i]);
                BinaryStdOut.Write(d);
            }
            BinaryStdOut.Close();
        }

        public static void Expand()
        {
            Alphabet dna = new Alphabet("ACTG");
            int n = BinaryStdIn.ReadInt();
            // Read two bits; write char. 
            for (int i = 0; i < n; i++)
            {
                byte c = BinaryStdIn.ReadByte();
                BinaryStdOut.Write(dna.ToChar(c));
            }
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