namespace Linker.Common
{
    using System;
    using System.Text.RegularExpressions;

    public static class UrlParser
    {
        public static string ExtractDomain(string url)
        {
            var uri = new Uri(url);
            return uri.Host;
        }

        public static string ExtractDomainLite(string url)
        {
            var regex = new Regex(@"^https?:\/\/(.*?)(\/.*)?$", RegexOptions.Compiled);
            var matches = regex.Match(url);
            return matches.Groups[1].Value;
        }
    }
}
