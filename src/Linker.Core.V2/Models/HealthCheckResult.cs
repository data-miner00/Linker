namespace Linker.Core.V2.Models;

using System;

/// <summary>
/// The result of an Url health check.
/// </summary>
public class HealthCheckResult
{
    /// <summary>
    /// Gets or sets the url of the webpage.
    /// </summary>
    required public string Url { get; set; }

    /// <summary>
    /// Gets or sets the status of the webpage.
    /// </summary>
    required public UrlStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the last checked date.
    /// </summary>
    required public DateTime LastCheckedAt { get; set; }

    /// <summary>
    /// Gets or sets the error message if any.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the first date that the webpage is inaccessible.
    /// </summary>
    public DateTime? DeadAt { get; set; }
}
