namespace Linker.Common
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The utility helpers for URLs.
    /// </summary>
    public static class UrlParser
    {
        /// <summary>
        /// Extract the domain using the built in <see cref="Uri"/> object.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>The extracted domain.</returns>
        public static string ExtractDomain(string url)
        {
            var uri = new Uri(url);
            return uri.Host;
        }

        /// <summary>
        /// A slightly light weighted alternative to <see cref="ExtractDomain(string)"/> using Regex.
        /// </summary>
        /// <param name="url">The url.</param>
        /// <returns>The extracted domain.</returns>
        public static string ExtractDomainLite(string url)
        {
            var regex = new Regex(@"^https?:\/\/(.*?)(\/.*)?$", RegexOptions.Compiled);
            var matches = regex.Match(url);
            return matches.Groups[1].Value;
        }
    }
}
