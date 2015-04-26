using System;
using System.Globalization;

namespace algs4.stdlib
{
    public class UnicodeTest
    {
        /// <summary>
        /// Number of Unicode characters to display per line
        /// </summary>
        private const int CharsPerLine = 16;

        /// <summary>
        /// Number of Unicode characters to display (basic multilingual plane)
        /// </summary>
        private const int MaxChar = 65536;

        /// <summary>
        /// Returns a string representation of the given codePoint, or a single
        /// space if the codePoint should not be suppressed when printing.
        /// </summary>
        /// <param name="codePoint"></param>
        /// <returns></returns>
        public static string ToString(int codePoint)
        {
            char c;
            try
            {
                c = char.ConvertFromUtf32(codePoint)[0];
            }
            catch (ArgumentOutOfRangeException)
            {
                return " ";
            }
            switch (char.GetUnicodeCategory(c))
            {
                case UnicodeCategory.OtherNotAssigned:
                case UnicodeCategory.Control:
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.Surrogate:
                case UnicodeCategory.ModifierSymbol:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.Format:
                case UnicodeCategory.PrivateUse:
                    return " ";
                default:
                    return c.ToString(CultureInfo.InvariantCulture);
            }
        }

        public static void RunMain(string[] args)
        {
            for (int line = 0; line < 2 * MaxChar / CharsPerLine; line++)
            {
                string output = "";
                for (int i = 0; i < CharsPerLine; i++)
                {
                    int codePoint = CharsPerLine * line + i;
                    output += ToString(codePoint) + "  ";
                }
                if (!output.Trim().Equals(""))
                {
                    // U+202D is the Unicode override to force left-to-right direction
                    // but doesn't seem to work with Unix more
                    StdOut.PrintF("U+{0:X4}   {1}\n", 16 * line, output);
                }
            }
        }
    }

}
