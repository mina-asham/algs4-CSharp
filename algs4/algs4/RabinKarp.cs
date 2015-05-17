using System.Numerics;
using System.Security.Cryptography;
using algs4.stdlib;

namespace algs4.algs4
{
    public class RabinKarp
    {
        /// <summary>
        /// The pattern // needed only for Las Vegas
        /// </summary>
        private readonly string _pat;

        /// <summary>
        /// Pattern hash value
        /// </summary>
        private readonly long _patHash;

        /// <summary>
        /// Pattern length
        /// </summary>
        private readonly int _m;

        /// <summary>
        /// A large prime, small enough to avoid long overflow
        /// </summary>
        private readonly long _q;

        /// <summary>
        /// Radix
        /// </summary>
        private readonly int _r;

        /// <summary>
        /// R^(M-1) % Q
        /// </summary>
        private readonly long _rm;

        public RabinKarp(string pat)
        {
            _pat = pat; // save pattern (needed only for Las Vegas)
            _r = 256;
            _m = pat.Length;
            _q = LongRandomPrime();

            // precompute R^(M-1) % Q for use in removing leading digit
            _rm = 1;
            for (int i = 1; i <= _m - 1; i++)
            {
                _rm = (_r * _rm) % _q;
            }
            _patHash = Hash(pat, _m);
        }

        /// <summary>
        /// Compute hash for key[0..M-1]. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private long Hash(string key, int m)
        {
            long h = 0;
            for (int j = 0; j < m; j++)
            {
                h = (_r * h + key[j]) % _q;
            }
            return h;
        }

        /// <summary>
        /// Las Vegas version: does pat[] match txt[i..i-M+1] ?
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool Check(string txt, int i)
        {
            for (int j = 0; j < _m; j++)
            {
                if (_pat[j] != txt[i + j])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Check for exact match
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public int Search(string txt)
        {
            int n = txt.Length;
            if (n < _m)
            {
                return n;
            }
            long txtHash = Hash(txt, _m);

            // Check for match at offset 0
            if ((_patHash == txtHash) && Check(txt, 0))
            {
                return 0;
            }

            // Check for hash match; if hash match, Check for exact match
            for (int i = _m; i < n; i++)
            {
                // Remove leading digit, add trailing digit, Check for match. 
                txtHash = (txtHash + _q - _rm * txt[i - _m] % _q) % _q;
                txtHash = (txtHash * _r + txt[i]) % _q;

                // match
                int offset = i - _m + 1;
                if ((_patHash == txtHash) && Check(txt, offset))
                {
                    return offset;
                }
            }

            // no match
            return n;
        }

        /// <summary>
        /// A random 31-bit prime
        /// </summary>
        /// <returns></returns>
        private static long LongRandomPrime()
        {
            BigInteger prime = RandomBigInteger(31);
            return (long)prime;
        }

        /// <summary>
        /// Replacement for Java's BigInteger.probablePrime
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        private static BigInteger RandomBigInteger(int bits)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[bits / 8];
            rng.GetBytes(bytes);

            return new BigInteger(bytes);
        }

        /// <summary>
        /// Test client
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            string pat = args[0];
            string txt = args[1];

            RabinKarp searcher = new RabinKarp(pat);
            int offset = searcher.Search(txt);

            // Print results
            StdOut.PrintLn("text:    " + txt);

            // from brute force search method 1
            StdOut.Print("pattern: ");
            for (int i = 0; i < offset; i++)
            {
                StdOut.Print(" ");
            }
            StdOut.PrintLn(pat);
        }
    }
}