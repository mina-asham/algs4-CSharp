﻿using algs4.stdlib;

namespace algs4.algs4
{
    public static class Cat
    {
        /// <summary>
        /// Reads in a sequence of text files specified as the first command-line
        /// arguments, concatenates them, and writes the results to the file
        /// specified as the last command-line argument.
        /// </summary>
        /// <param name="args">Main arugments</param>
        public static void RunMain(string[] args)
        {
            Out output = new Out(args[args.Length - 1]);
            for (int i = 0; i < args.Length - 1; i++)
            {
                In input = new In(args[i]);
                string s = input.ReadAll();
                output.PrintLn(s);
                input.Close();
            }
            output.Close();
        }
    }
}
