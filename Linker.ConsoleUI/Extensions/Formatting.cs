using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.ConsoleUI.Extensions
{
    public static class Formatting
    {
        public static string TruncateWithEllipsis(this string input, int length = 30) =>
            input.Length > length ? input.Substring(0, length) + "..." : input;

        public static string PadSides(this string str, int totalWidth, char paddingChar = ' ')
        {
            int padding = totalWidth - str.Length;
            int padLeft = padding / 2 + str.Length;
            return str.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
        }
    }
}
