namespace Linker.ConsoleUI.Extensions
{
    public static class StringExtensions
    {
        public static string? ReturnNullIfEmpty(this string input) =>
            string.IsNullOrEmpty(input) ? null : input;
    }
}
