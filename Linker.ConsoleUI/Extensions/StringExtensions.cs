using System;
using System.Collections.Generic;
using System.Text;

namespace Linker.ConsoleUI.Extensions
{
    public static class StringExtensions
    {
        public static string? ReturnNullIfEmpty(this string input) =>
            string.IsNullOrEmpty(input) ? null : input;
    }
}
