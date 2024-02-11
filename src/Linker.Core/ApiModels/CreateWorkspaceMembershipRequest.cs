namespace Linker.Core.ApiModels;

using Linker.Core.Models;
using System;

/// <summary>
/// The workspace membership request.
/// </summary>
public sealed class CreateWorkspaceMembershipRequest
{
    /// <summary>
    /// Gets or sets the workspace ID.
    /// </summary>
    required public Guid WorkspaceId { get; set; }

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    required public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the workspace role for the user.
    /// </summary>
    required public WorkspaceRole WorkspaceRole { get; set; }
}
