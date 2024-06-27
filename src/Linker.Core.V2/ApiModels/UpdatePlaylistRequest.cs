namespace Linker.Core.V2.ApiModels;

using Linker.Core.V2.Models;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The update playlist request object.
/// </summary>
public sealed class UpdatePlaylistRequest
{
    /// <summary>
    /// Gets or sets the new name of the playlist.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the playlist.
    /// </summary>
    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the playlist.
    /// </summary>
    public Visibility Visibility { get; set; }
}
