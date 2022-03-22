using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.ConsoleUI.Extensions
{
    public static class Formatting
    {
        public static string TruncateWithEllipsis(this string input, int length = 30) =>
            input.Length > length ? input.Substring(0, length) + "..." : input;
    }
}
