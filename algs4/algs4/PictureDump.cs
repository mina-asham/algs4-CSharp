using System;
using System.Drawing;
using algs4.stdlib;

namespace algs4.algs4
{
    public class PictureDump
    {
        public static void RunMain(string[] args)
        {
            int width = int.Parse(args[0]);
            int height = int.Parse(args[1]);
            Picture pic = new Picture(width, height);
            int count = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    pic.Set(j, i, Color.Red);
                    if (!BinaryStdIn.IsEmpty())
                    {
                        count++;
                        bool bit = BinaryStdIn.ReadBoolean();
                        if (bit) pic.Set(j, i, Color.Black);
                        else pic.Set(j, i, Color.White);
                    }
                }
            }
            pic.Show();
            StdOut.PrintLn(count + " bits");
        }
    }
}
