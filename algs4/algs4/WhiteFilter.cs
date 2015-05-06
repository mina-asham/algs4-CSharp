using algs4.stdlib;

namespace algs4.algs4
{
    public class WhiteFilter
    {
        public static void RunMain(string[] args)
        {
            Set<string> set = new Set<string>();

            // read in strings and add to set
            In input = new In(args[0]);
            while (!input.IsEmpty())
            {
                string word = input.ReadString();
                set.Add(word);
            }

            // read in string from standard input, printing out all exceptions
            while (!StdIn.IsEmpty())
            {
                string word = StdIn.ReadString();
                if (set.Contains(word))
                {
                    StdOut.PrintLn(word);
                }
            }
        }
    }
}