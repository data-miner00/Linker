namespace Linker.Core.V2.Models;

/// <summary>
/// The enumeration for Url's status.
/// </summary>
public enum UrlStatus
{
    /// <summary>
    /// The default value.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the Url is still accessible on last check.
    /// </summary>
    Alive,

    /// <summary>
    /// Indicates that the Url is not accessible on last check.
    /// </summary>
    Dead,
}
