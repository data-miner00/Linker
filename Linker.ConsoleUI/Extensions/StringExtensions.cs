namespace Linker.ConsoleUI.Extensions
{
    /// <summary>
    /// The static class for custom string extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks that return null value if the string is empty.
        /// </summary>
        /// <param name="input">The input to be checked</param>
        /// <returns>The input if not empty and null for empty string.</returns>
        public static string? ReturnNullIfEmpty(this string input) =>
            string.IsNullOrEmpty(input) ? null : input;
    }
}
