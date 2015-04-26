namespace algs4.stdlib
{
    public static class Test
    {
        public static void RunMain(string[] args)
        {
            string[] lines = StdIn.ReadAllLines();
            for (int i = 0; i < lines.Length; i++)
            {
                StdOut.PrintLn(lines[i]);
            }
        }

    }
}
