using System;
using System.Text;
using algs4.stdlib;

namespace algs4.algs4
{
    public class Alphabet
    {
        public static readonly Alphabet Binary = new Alphabet("01");
        public static readonly Alphabet Octal = new Alphabet("01234567");
        public static readonly Alphabet Decimal = new Alphabet("0123456789");
        public static readonly Alphabet Hexadecimal = new Alphabet("0123456789ABCDEF");
        public static readonly Alphabet DNA = new Alphabet("ACTG");
        public static readonly Alphabet Lowercase = new Alphabet("abcdefghijklmnopqrstuvwxyz");
        public static readonly Alphabet Uppercase = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        public static readonly Alphabet Protein = new Alphabet("ACDEFGHIKLMNPQRSTVWY");
        public static readonly Alphabet Base64 = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/");
        public static readonly Alphabet ASCII = new Alphabet(128);
        public static readonly Alphabet ExtendedASCII = new Alphabet(256);
        public static readonly Alphabet Unicode16 = new Alphabet(65536);

        /// <summary>
        /// The characters in the alphabet
        /// </summary>
        private readonly char[] _alphabet;

        /// <summary>
        /// Indices
        /// </summary>
        private readonly int[] _inverse;

        /// <summary>
        /// The radix of the alphabet
        /// </summary>
        private readonly int _r;

        /// <summary>
        /// Create a new Alphabet from sequence of characters in string.
        /// </summary>
        /// <param name="alpha">Alphabet characters</param>
        public Alphabet(string alpha)
        {
            // check that alphabet contains no duplicate chars
            bool[] unicode = new bool[Char.MaxValue];
            for (int i = 0; i < alpha.Length; i++)
            {
                char c = alpha[i];
                if (unicode[c])
                {
                    throw new ArgumentException("Illegal alphabet: repeated character = '" + c + "'");
                }
                unicode[c] = true;
            }

            _alphabet = alpha.ToCharArray();
            _r = alpha.Length;
            _inverse = new int[Char.MaxValue];
            for (int i = 0; i < _inverse.Length; i++)
            {
                _inverse[i] = -1;
            }

            // can't use char since r can be as big as 65,536
            for (int c = 0; c < _r; c++)
            {
                _inverse[_alphabet[c]] = c;
            }
        }

        /// <summary>
        /// Create a new Alphabet of Unicode chars 0 to r-1
        /// </summary>
        /// <param name="r">Exclusive end</param>
        private Alphabet(int r)
        {
            _alphabet = new char[r];
            _inverse = new int[r];
            _r = r;

            // can't use char since r can be as big as 65,536
            for (int i = 0; i < r; i++)
                _alphabet[i] = (char)i;
            for (int i = 0; i < r; i++)
                _inverse[i] = i;
        }

        /// <summary>
        /// Create a new Alphabet of Unicode chars 0 to 255 (extended ASCII)
        /// </summary>
        public Alphabet()
            : this(256)
        {
        }

        /// <summary>
        /// Is character c in the alphabet?
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool Contains(char c)
        {
            return _inverse[c] != -1;
        }

        /// <summary>
        /// Return radix r
        /// </summary>
        /// <returns></returns>
        public int R()
        {
            return _r;
        }

        /// <summary>
        /// Return number of bits to represent an index
        /// </summary>
        /// <returns></returns>
        public int LgR()
        {
            int lgR = 0;
            for (int t = _r - 1; t >= 1; t /= 2)
            {
                lgR++;
            }
            return lgR;
        }

        /// <summary>
        /// Convert c to index between 0 and r-1.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int ToIndex(char c)
        {
            if (c < 0 || c >= _inverse.Length || _inverse[c] == -1)
            {
                throw new ArgumentException("Character " + c + " not in alphabet");
            }
            return _inverse[c];
        }

        /// <summary>
        /// Convert string s over this alphabet into a base-r integer
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int[] ToIndices(string s)
        {
            char[] source = s.ToCharArray();
            int[] target = new int[s.Length];
            for (int i = 0; i < source.Length; i++)
            {
                target[i] = ToIndex(source[i]);
            }
            return target;
        }

        /// <summary>
        /// Convert an index between 0 and r-1 into a char over this alphabet
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public char ToChar(int index)
        {
            if (index < 0 || index >= _r)
            {
                throw new IndexOutOfRangeException("Alphabet index out of bounds");
            }
            return _alphabet[index];
        }

        /// <summary>
        /// Convert base-r integer into a string over this alphabet
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        public string ToChars(int[] indices)
        {
            StringBuilder s = new StringBuilder(indices.Length);
            for (int i = 0; i < indices.Length; i++)
            {
                s.Append(ToChar(indices[i]));
            }
            return s.ToString();
        }

        public static void RunMain(string[] args)
        {
            int[] encoded1 = Base64.ToIndices("NowIsTheTimeForAllGoodMen");
            string decoded1 = Base64.ToChars(encoded1);
            StdOut.PrintLn(decoded1);

            int[] encoded2 = DNA.ToIndices("AACGAACGGTTTACCCCG");
            string decoded2 = DNA.ToChars(encoded2);
            StdOut.PrintLn(decoded2);

            int[] encoded3 = Decimal.ToIndices("01234567890123456789");
            string decoded3 = Decimal.ToChars(encoded3);
            StdOut.PrintLn(decoded3);
        }
    }
}
