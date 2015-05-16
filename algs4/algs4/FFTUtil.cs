using System;
using algs4.stdlib;

namespace algs4.algs4
{
    public static class FFTUtil
    {
        /// <summary>
        /// Compute the FFT of x[], assuming its length is a power of 2
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Complex[] FFT(Complex[] x)
        {
            int n = x.Length;

            // base case
            if (n == 1)
            {
                return new[] { x[0] };
            }

            // radix 2 Cooley-Tukey FFT
            if (n % 2 != 0)
            {
                throw new ArgumentException("N is not a power of 2");
            }

            // FFT of even terms
            Complex[] even = new Complex[n / 2];
            for (int k = 0; k < n / 2; k++)
            {
                even[k] = x[2 * k];
            }
            Complex[] q = FFT(even);

            // FFT of odd terms
            Complex[] odd = even; // reuse the array
            for (int k = 0; k < n / 2; k++)
            {
                odd[k] = x[2 * k + 1];
            }
            Complex[] r = FFT(odd);

            // combine
            Complex[] y = new Complex[n];
            for (int k = 0; k < n / 2; k++)
            {
                double kth = -2 * k * Math.PI / n;
                Complex wk = new Complex(Math.Cos(kth), Math.Sin(kth));
                y[k] = q[k].Plus(wk.Times(r[k]));
                y[k + n / 2] = q[k].Minus(wk.Times(r[k]));
            }
            return y;
        }

        /// <summary>
        /// Compute the inverse FFT of x[], assuming its length is a power of 2
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Complex[] IFFT(Complex[] x)
        {
            int n = x.Length;
            Complex[] y = new Complex[n];

            // take conjugate
            for (int i = 0; i < n; i++)
            {
                y[i] = x[i].Conjugate();
            }

            // compute forward FFT
            y = FFT(y);

            // take conjugate again
            for (int i = 0; i < n; i++)
            {
                y[i] = y[i].Conjugate();
            }

            // divide by N
            for (int i = 0; i < n; i++)
            {
                y[i] = y[i].Times(1.0 / n);
            }

            return y;
        }

        /// <summary>
        /// Compute the circular convolution of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Complex[] CConvolve(Complex[] x, Complex[] y)
        {
            // should probably pad x and y with 0s so that they have same length
            // and are powers of 2
            if (x.Length != y.Length)
            {
                throw new ArgumentException("Dimensions don't agree");
            }

            int n = x.Length;

            // compute FFT of each sequence
            Complex[] a = FFT(x);
            Complex[] b = FFT(y);

            // point-wise multiply
            Complex[] c = new Complex[n];
            for (int i = 0; i < n; i++)
            {
                c[i] = a[i].Times(b[i]);
            }

            // compute inverse FFT
            return IFFT(c);
        }

        /// <summary>
        /// Compute the linear convolution of x and y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Complex[] Convolve(Complex[] x, Complex[] y)
        {
            Complex zero = new Complex(0, 0);

            Complex[] a = new Complex[2 * x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                a[i] = x[i];
            }
            for (int i = x.Length; i < 2 * x.Length; i++)
            {
                a[i] = zero;
            }

            Complex[] b = new Complex[2 * y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                b[i] = y[i];
            }
            for (int i = y.Length; i < 2 * y.Length; i++)
            {
                b[i] = zero;
            }

            return CConvolve(a, b);
        }

        /// <summary>
        /// Display an array of Complex numbers to standard output
        /// </summary>
        /// <param name="x"></param>
        /// <param name="title"></param>
        public static void Show(Complex[] x, string title)
        {
            StdOut.PrintLn(title);
            StdOut.PrintLn("-------------------");
            for (int i = 0; i < x.Length; i++)
            {
                StdOut.PrintLn(x[i]);
            }
            StdOut.PrintLn();
        }

        /// <summary>
        /// Test client and sample execution
        ///
        /// % java FFT 4
        /// x
        /// -------------------
        /// -0.03480425839330703
        /// 0.07910192950176387
        /// 0.7233322451735928
        /// 0.1659819820667019
        ///
        /// y = FFT(x)
        /// -------------------
        /// 0.9336118983487516
        /// -0.7581365035668999 + 0.08688005256493803i
        /// 0.44344407521182005
        /// -0.7581365035668999 - 0.08688005256493803i
        ///
        /// z = ifft(y)
        /// -------------------
        /// -0.03480425839330703
        /// 0.07910192950176387 + 2.6599344570851287E-18i
        /// 0.7233322451735928
        /// 0.1659819820667019 - 2.6599344570851287E-18i
        ///
        /// c = cconvolve(x, x)
        /// -------------------
        /// 0.5506798633981853
        /// 0.23461407150576394 - 4.033186818023279E-18i
        /// -0.016542951108772352
        /// 0.10288019294318276 + 4.033186818023279E-18i
        ///
        /// d = convolve(x, x)
        /// -------------------
        /// 0.001211336402308083 - 3.122502256758253E-17i
        /// -0.005506167987577068 - 5.058885073636224E-17i
        /// -0.044092969479563274 + 2.1934338938072244E-18i
        /// 0.10288019294318276 - 3.6147323062478115E-17i
        /// 0.5494685269958772 + 3.122502256758253E-17i
        /// 0.240120239493341 + 4.655566391833896E-17i
        /// 0.02755001837079092 - 2.1934338938072244E-18i
        /// 4.01805098805014E-17i
        /// </summary>
        /// <param name="args"></param>

        public static void RunMain(string[] args)
        {
            int n = int.Parse(args[0]);
            Complex[] x = new Complex[n];

            Random random = new Random();

            // original data
            for (int i = 0; i < n; i++)
            {
                x[i] = new Complex(i, 0);

                x[i] = new Complex(-2 * random.NextDouble() + 1, 0);
            }
            Show(x, "x");

            // FFT of original data
            Complex[] y = FFT(x);
            Show(y, "y = FFT(x)");

            // take inverse FFT
            Complex[] z = IFFT(y);
            Show(z, "z = ifft(y)");

            // circular convolution of x with itself
            Complex[] c = CConvolve(x, x);
            Show(c, "c = cconvolve(x, x)");

            // linear convolution of x with itself
            Complex[] d = Convolve(x, x);
            Show(d, "d = convolve(x, x)");
        }
    }
}