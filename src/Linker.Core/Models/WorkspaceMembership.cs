namespace Linker.Core.Models;

using System;

/// <summary>
/// The membership record for a workspace.
/// </summary>
public class WorkspaceMembership
{
    /// <summary>
    /// Gets or sets the workspace ID.
    /// </summary>
    required public string WorkspaceId { get; set; }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    required public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the workspace role of the user.
    /// </summary>
    required public WorkspaceRole WorkspaceRole { get; set; }

    /// <summary>
    /// Gets or sets the date of joining.
    /// </summary>
    required public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date of modification.
    /// </summary>
    required public DateTime ModifiedAt { get; set; }
}
