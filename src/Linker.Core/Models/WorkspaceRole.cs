namespace Linker.Core.Models;

/// <summary>
/// The workspace role.
/// </summary>
public enum WorkspaceRole
{
    /// <summary>
    /// The owner of the workspace.
    /// </summary>
    Owner,

    /// <summary>
    /// The admin of the workspace.
    /// </summary>
    Admin,

    /// <summary>
    /// Regular members of the workspace.
    /// </summary>
    User,
}
