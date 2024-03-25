namespace Linker.Core.V2.Models;

/// <summary>
/// The visibility of a link.
/// </summary>
public enum Visibility
{
    /// <summary>
    /// The default value.
    /// </summary>
    None,

    /// <summary>
    /// The link is publicly accessible.
    /// </summary>
    Public,

    /// <summary>
    /// The link is only accessible to its owner.
    /// </summary>
    Private,
}
