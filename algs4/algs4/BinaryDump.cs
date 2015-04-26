using algs4.stdlib;

namespace algs4.algs4
{
    public class BinaryDump
    {
        public static void RunMain(string[] args)
        {
            int bitsPerLine = 16;
            if (args.Length == 1)
            {
                bitsPerLine = int.Parse(args[0]);
            }

            int count;
            for (count = 0; !BinaryStdIn.IsEmpty(); count++)
            {
                if (bitsPerLine == 0)
                {
                    BinaryStdIn.ReadBoolean();
                    continue;
                }

                if (count != 0 && count % bitsPerLine == 0)
                {
                    StdOut.PrintLn();
                }

                if (BinaryStdIn.ReadBoolean())
                {
                    StdOut.Print(1);
                }
                else
                {
                    StdOut.Print(0);
                }
            }
            if (bitsPerLine != 0)
            {
                StdOut.PrintLn();
            }
            StdOut.PrintLn(count + " bits");
        }
    }
}
