using System;

namespace algs4.stdlib
{
    public static class StdArrayIO
    {
        /// <summary>
        /// Read in and return an array of doubles from standard input.
        /// </summary>
        /// <returns></returns>
        public static double[] ReadDouble1D()
        {
            int n = StdIn.ReadInt();
            double[] a = new double[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = StdIn.ReadDouble();
            }
            return a;
        }

        /// <summary>
        /// Print an array of doubles to standard output.
        /// </summary>
        /// <param name="a"></param>
        public static void Print(double[] a)
        {
            int n = a.Length;
            StdOut.PrintLn(n);
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintF("{0:000000000.00000} ", a[i]);
            }
            StdOut.PrintLn();
        }


        /// <summary>
        /// Read in and return an M-by-N array of doubles from standard input.
        /// </summary>
        /// <returns></returns>
        public static double[][] ReadDouble2D()
        {
            int m = StdIn.ReadInt();
            int n = StdIn.ReadInt();
            double[][] a = new double[m][];
            for (int i = 0; i < m; i++)
            {
                a[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = StdIn.ReadDouble();
                }
            }
            return a;
        }

        /// <summary>
        /// Print the M-by-N array of doubles to standard output.
        /// </summary>
        /// <param name="a"></param>
        public static void Print(double[][] a)
        {
            int m = a.Length;
            int n = a[0].Length;
            StdOut.PrintLn(m + " " + n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    StdOut.PrintF("{0:000000000:00000} ", a[i][j]);
                }
                StdOut.PrintLn();
            }
        }

        /// <summary>
        /// Read in and return an array of ints from standard input.
        /// </summary>
        /// <returns></returns>
        public static int[] ReadInt1D()
        {
            int n = StdIn.ReadInt();
            int[] a = new int[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = StdIn.ReadInt();
            }
            return a;
        }

        /// <summary>
        /// Print an array of ints to standard output.
        /// </summary>
        /// <param name="a"></param>
        public static void Print(int[] a)
        {
            int n = a.Length;
            StdOut.PrintLn(n);
            for (int i = 0; i < n; i++)
            {
                StdOut.PrintF("{0:000000000} ", a[i]);
            }
            StdOut.PrintLn();
        }

        /// <summary>
        /// Read in and return an M-by-N array of ints from standard input.
        /// </summary>
        /// <returns></returns>
        public static int[][] ReadInt2D()
        {
            int m = StdIn.ReadInt();
            int n = StdIn.ReadInt();
            int[][] a = new int[m][];
            for (int i = 0; i < m; i++)
            {
                a[i] = new int[n];
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = StdIn.ReadInt();
                }
            }
            return a;
        }

        /// <summary>
        /// Print the M-by-N array of ints to standard output.
        /// </summary>
        /// <param name="a"></param>
        public static void Print(int[][] a)
        {
            int m = a.Length;
            int n = a[0].Length;
            StdOut.PrintLn(m + " " + n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    StdOut.PrintF("{0:000000000} ", a[i][j]);
                }
                StdOut.PrintLn();
            }
        }

        /// <summary>
        /// Read in and return an array of booleans from standard input.
        /// </summary>
        /// <returns></returns>
        public static bool[] ReadBoolean1D()
        {
            int n = StdIn.ReadInt();
            bool[] a = new bool[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = StdIn.ReadBoolean();
            }
            return a;
        }

        /// <summary>
        /// Print an array of booleans to standard output.
        /// </summary>
        /// <param name="a"></param>
        public static void Print(bool[] a)
        {
            int n = a.Length;
            StdOut.PrintLn(n);
            for (int i = 0; i < n; i++)
            {
                if (a[i])
                {
                    StdOut.Print("1 ");
                }
                else
                {
                    StdOut.Print("0 ");
                }
            }
            StdOut.PrintLn();
        }

        /// <summary>
        /// Read in and return an M-by-N array of booleans from standard input.
        /// </summary>
        /// <returns></returns>
        public static bool[][] ReadBoolean2D()
        {
            int m = StdIn.ReadInt();
            int n = StdIn.ReadInt();
            bool[][] a = new bool[m][];
            for (int i = 0; i < m; i++)
            {
                a[0] = new bool[n];
                for (int j = 0; j < n; j++)
                {
                    a[i][j] = StdIn.ReadBoolean();
                }
            }
            return a;
        }

        /// <summary>
        /// Print the  M-by-N array of booleans to standard output.
        /// </summary>
        /// <param name="a"></param>
        public static void Print(bool[][] a)
        {
            int m = a.Length;
            int n = a[0].Length;
            StdOut.PrintLn(m + " " + n);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (a[i][j])
                    {
                        StdOut.Print("1 ");
                    }
                    else
                    {
                        StdOut.Print("0 ");
                    }
                }
                StdOut.PrintLn();
            }
        }

        /// <summary>
        /// Test client.
        /// </summary>
        /// <param name="args"></param>
        public static void RunMain(string[] args)
        {
            // read and print an array of doubles
            double[] a = ReadDouble1D();
            Print(a);
            StdOut.PrintLn();

            // read and print a matrix of doubles
            double[][] b = ReadDouble2D();
            Print(b);
            StdOut.PrintLn();

            // read and print a matrix of doubles
            bool[][] d = ReadBoolean2D();
            Print(d);
            StdOut.PrintLn();
        }
    }
}
