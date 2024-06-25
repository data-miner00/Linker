namespace Linker.Core.V2.Models;

using System;

/// <summary>
/// The playlist of links.
/// </summary>
public class Playlist
{
    /// <summary>
    /// Gets or sets the ID of the playlist.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the owner Id.
    /// </summary>
    public string OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the playlist.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the playlist.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the playlist.
    /// </summary>
    public Visibility Visibility { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the last modified date.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
