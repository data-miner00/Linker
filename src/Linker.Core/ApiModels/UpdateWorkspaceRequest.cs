namespace Linker.Core.ApiModels;

/// <summary>
/// The request to update workspace info.
/// </summary>
public sealed class UpdateWorkspaceRequest
{
    /// <summary>
    /// The unique handle of the workspace.
    /// </summary>
    required public string Handle { get; set; }

    /// <summary>
    /// Gets or sets the name of the workspace.
    /// </summary>
    required public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the workspace.
    /// </summary>
    required public string Description { get; set; }
}
