using algs4.stdlib;

namespace algs4.algs4
{
    public class HexDump
    {
        public static void RunMain(string[] args)
        {
            int bytesPerLine = 16;
            if (args.Length == 1)
            {
                bytesPerLine = int.Parse(args[0]);
            }

            int i;
            for (i = 0; !BinaryStdIn.IsEmpty(); i++)
            {
                if (bytesPerLine == 0)
                {
                    BinaryStdIn.ReadChar();
                    continue;
                }
                if (i == 0)
                {
                    StdOut.PrintF("");
                }
                else if (i % bytesPerLine == 0)
                {
                    StdOut.PrintF("\n", i);
                }
                else
                {
                    StdOut.Print(" ");
                }
                char c = BinaryStdIn.ReadChar();
                StdOut.PrintF("{0:X2}", c & 0xff);
            }
            if (bytesPerLine != 0)
            {
                StdOut.PrintLn();
            }
            StdOut.PrintLn((i * 8) + " bits");
        }
    }
}