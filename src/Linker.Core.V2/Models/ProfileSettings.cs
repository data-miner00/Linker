namespace Linker.Core.V2.Models;

using System;

/// <summary>
/// The settings for a user.
/// </summary>
public class ProfileSettings
{
    /// <summary>
    /// Gets or sets the Id of the setting.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the theme preferred by the user.
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has accepted the telemetry.
    /// </summary>
    public bool AcceptedTelemetry { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has accepted the terms and conditions.
    /// </summary>
    public bool AcceptedTerms { get; set; }

    /// <summary>
    /// Gets or sets the browsing level preferred by the user.
    /// </summary>
    public Rating BrowsingLevel { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last updated date.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
