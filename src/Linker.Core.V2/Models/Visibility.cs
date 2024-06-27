namespace Linker.Core.V2.Models;

/// <summary>
/// The visibility of a resource.
/// </summary>
public enum Visibility
{
    /// <summary>
    /// The default value.
    /// </summary>
    None,

    /// <summary>
    /// The resource is publicly accessible.
    /// </summary>
    Public,

    /// <summary>
    /// The resource is only accessible to its owner.
    /// </summary>
    Private,

    /// <summary>
    /// The resource is available only through dedicated links.
    /// </summary>
    Unlisted,
}
