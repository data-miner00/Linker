namespace Linker.ConsoleUI.Extensions
{
    /// <summary>
    /// The static class for custom string extension methods for UI formatting.
    /// </summary>
    internal static class Formatting
    {
        /// <summary>
        /// Truncates a string that is longer than the threshold and replace with ellipsis.
        /// </summary>
        /// <param name="input">The actual string to be truncated.</param>
        /// <param name="length">The threshold of the allowable length.</param>
        /// <returns>The formatted string.</returns>
        public static string TruncateWithEllipsis(this string input, int length = 30) =>
            input.Length > length ? input.Substring(0, length) + "..." : input;

        /// <summary>
        /// Pads the string with character of choice from left and right.
        /// </summary>
        /// <param name="str">The string to pad.</param>
        /// <param name="totalWidth">The maximum width for the string.</param>
        /// <param name="paddingChar">The character of choice to pad. Defaulted to space.</param>
        /// <returns>String with both sides padded.</returns>
        public static string PadSides(this string str, int totalWidth, char paddingChar = ' ')
        {
            int padding = totalWidth - str.Length;
            int padLeft = (padding / 2) + str.Length;
            return str.PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar);
        }
    }
}
