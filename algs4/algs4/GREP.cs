using algs4.stdlib;

namespace algs4.algs4
{
    public class GREP
    {
        public static void RunMain(string[] args)
        {
            string regexp = "(.*" + args[0] + ".*)";
            NFA nfa = new NFA(regexp);
            while (StdIn.HasNextLine())
            {
                string txt = StdIn.ReadLine();
                if (nfa.Recognizes(txt))
                {
                    StdOut.PrintLn(txt);
                }
            }
        }
    }
}