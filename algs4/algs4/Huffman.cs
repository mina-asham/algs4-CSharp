using System;
using System.Diagnostics;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Huffman
    {
        /// <summary>
        /// alphabet size of extended ASCII
        /// </summary>
        private const int R = 256;

        /// <summary>
        /// Huffman trie node
        /// </summary>
        private class Node : IComparable<Node>
        {
            public char Ch { get; private set; }
            public int Freq { get; private set; }
            public Node Left { get; private set; }
            public Node Right { get; private set; }

            public Node(char ch, int freq, Node left, Node right)
            {
                Ch = ch;
                Freq = freq;
                Left = left;
                Right = right;
            }

            // is the node a leaf node?
            public bool IsLeaf()
            {
                Debug.Assert((Left == null && Right == null) || (Left != null && Right != null));
                return (Left == null && Right == null);
            }

            // compare, based on frequency
            public int CompareTo(Node that)
            {
                return Freq - that.Freq;
            }
        }

        /// <summary>
        /// Compress bytes from standard input and write to standard output
        /// </summary>
        public static void Compress()
        {
            // read the input
            string s = BinaryStdIn.ReadString();
            char[] input = s.ToCharArray();

            // tabulate frequency counts
            int[] freq = new int[R];
            for (int i = 0; i < input.Length; i++)
            {
                freq[input[i]]++;
            }

            // build Huffman trie
            Node root = BuildTrie(freq);

            // build code table
            string[] st = new string[R];
            BuildCode(st, root, "");

            // Print trie for decoder
            WriteTrie(root);

            // Print number of bytes in original uncompressed message
            BinaryStdOut.Write(input.Length);

            // use Huffman code to encode input
            for (int i = 0; i < input.Length; i++)
            {
                string code = st[input[i]];
                for (int j = 0; j < code.Length; j++)
                {
                    if (code[j] == '0')
                    {
                        BinaryStdOut.Write(false);
                    }
                    else if (code[j] == '1')
                    {
                        BinaryStdOut.Write(true);
                    }
                    else
                    {
                        throw new InvalidOperationException("Illegal state");
                    }
                }
            }

            // close output stream
            BinaryStdOut.Close();
        }

        /// <summary>
        /// Build the Huffman trie given frequencies
        /// </summary>
        /// <param name="freq"></param>
        /// <returns></returns>
        private static Node BuildTrie(int[] freq)
        {
            // initialze priority queue with singleton trees
            MinPQ<Node> pq = new MinPQ<Node>();
            for (char i = (char)0; i < R; i++)
            {
                if (freq[i] > 0)
                {
                    pq.Insert(new Node(i, freq[i], null, null));
                }
            }

            // special case in case there is only one character with a nonzero frequency
            if (pq.Size() == 1)
            {
                if (freq['\0'] == 0)
                {
                    pq.Insert(new Node('\0', 0, null, null));
                }
                else
                {
                    pq.Insert(new Node('\u0001', 0, null, null));
                }
            }

            // merge two smallest trees
            while (pq.Size() > 1)
            {
                Node left = pq.DelMin();
                Node right = pq.DelMin();
                Node parent = new Node('\0', left.Freq + right.Freq, left, right);
                pq.Insert(parent);
            }
            return pq.DelMin();
        }

        /// <summary>
        /// Write bitstring-encoded trie to standard output
        /// </summary>
        /// <param name="x"></param>
        private static void WriteTrie(Node x)
        {
            if (x.IsLeaf())
            {
                BinaryStdOut.Write(true);
                BinaryStdOut.Write(x.Ch);
                return;
            }
            BinaryStdOut.Write(false);
            WriteTrie(x.Left);
            WriteTrie(x.Right);
        }

        /// <summary>
        /// Make a lookup table from symbols and their encodings
        /// </summary>
        /// <param name="st"></param>
        /// <param name="x"></param>
        /// <param name="s"></param>
        private static void BuildCode(string[] st, Node x, string s)
        {
            if (!x.IsLeaf())
            {
                BuildCode(st, x.Left, s + '0');
                BuildCode(st, x.Right, s + '1');
            }
            else
            {
                st[x.Ch] = s;
            }
        }

        /// <summary>
        /// Expand Huffman-encoded input from standard input and write to standard output
        /// </summary>
        public static void Expand()
        {
            // read in Huffman trie from input stream
            Node root = ReadTrie();

            // number of bytes to write
            int length = BinaryStdIn.ReadInt();

            // decode using the Huffman trie
            for (int i = 0; i < length; i++)
            {
                Node x = root;
                while (!x.IsLeaf())
                {
                    bool bit = BinaryStdIn.ReadBoolean();
                    if (bit)
                    {
                        x = x.Right;
                    }
                    else
                    {
                        x = x.Left;
                    }
                }
                BinaryStdOut.Write(x.Ch);
            }
            BinaryStdOut.Close();
        }

        private static Node ReadTrie()
        {
            bool isLeaf = BinaryStdIn.ReadBoolean();
            if (isLeaf)
            {
                return new Node(BinaryStdIn.ReadChar(), -1, null, null);
            }
            else
            {
                return new Node('\0', -1, ReadTrie(), ReadTrie());
            }
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