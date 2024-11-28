namespace Linker.Core.V2.Models;

using System;

/// <summary>
/// Allow users to create meaningful collection and making sense of the links.
/// </summary>
public class Workspace
{
    /// <summary>
    /// Gets or sets the unique Id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the "username" of the workspace for easy reference.
    /// </summary>
    public string Handle { get; set; }

    /// <summary>
    /// Gets or sets the display name of the workspace.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the workspace.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the owner Id.
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the workspace.
    /// </summary>
    public Visibility Visibility { get; set; }

    /// <summary>
    /// Gets or sets the maximum accomodated member count.
    /// </summary>
    public short? MaxMemberCount { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last modified date.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
