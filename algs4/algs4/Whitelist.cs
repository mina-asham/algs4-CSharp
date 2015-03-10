using algs4.stdlib;

namespace algs4.algs4
{
    public class Whitelist
    {
        /// <summary>
        /// Reads in a sequence of integers from the whitelist file, specified as
        /// a command-line argument. Reads in integers from standard input and
        /// prints to standard output those integers that are not in the file.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            In input = new In(args[0]);
            int[] white = input.ReadAllInts();
            StaticSETofInts set = new StaticSETofInts(white);

            // Read key, print if not in whitelist.
            while (!StdIn.IsEmpty())
            {
                int key = StdIn.ReadInt();
                if (!set.Contains(key))
                    StdOut.PrintLn(key);
            }
        }
    }
}
