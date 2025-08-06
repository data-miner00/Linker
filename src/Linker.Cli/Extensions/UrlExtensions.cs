namespace Linker.Cli.Extensions;

using System;
using System.Diagnostics;

/// <summary>
/// The static class that contains URL-related extensions.
/// </summary>
internal static class UrlExtensions
{
    /// <summary>
    /// Shortens the URL to a maximum set of characters. Defaults to 30.
    /// </summary>
    /// <param name="url">The url to shorten.</param>
    /// <returns>The shortened url.</returns>
    public static string ToShortenedUrl(this string url, int charsCount = 30)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        return url.Length > 30 ? string.Concat(url.AsSpan(0, charsCount), "...") : url;
    }

    /// <summary>
    /// Formats the URL to ensure it starts with http or https scheme.
    /// </summary>
    /// <param name="url">The unsanitised url.</param>
    /// <returns>The sanitised url.</returns>
    public static string ToFormattedUrl(this string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return "http://" + url;
        }

        return url;
    }

    /// <summary>
    /// Visits the specified URL using the default web browser.
    /// </summary>
    /// <param name="url">The url to visit.</param>
    public static void VisitUrl(this string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        var startInfo = new ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true,
        };

        Process.Start(startInfo);
    }
}
