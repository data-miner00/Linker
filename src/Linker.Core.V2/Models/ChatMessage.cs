namespace Linker.Core.V2.Models;

using System;

/// <summary>
/// The chat object.
/// </summary>
public sealed record ChatMessage
{
    /// <summary>
    /// Gets or sets the Id of the chat record.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the author's ID.
    /// </summary>
    public string AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the workspace's ID.
    /// </summary>
    public string WorkspaceId { get; set; }

    /// <summary>
    /// Gets or sets the actual chat message.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat content has been edited.
    /// </summary>
    public bool IsEdited { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the chat content has been deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or set the updated date.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
