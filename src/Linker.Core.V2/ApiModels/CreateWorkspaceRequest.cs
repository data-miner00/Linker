namespace Linker.Core.V2.ApiModels;

using Linker.Core.V2.Models;

/// <summary>
/// The request model to create a <see cref="Workspace"/>.
/// </summary>
public class CreateWorkspaceRequest
{
    /// <summary>
    /// Gets or sets the handle of the workspace.
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
