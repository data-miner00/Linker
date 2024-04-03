namespace Linker.Core.V2.ApiModels;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The update tag request object.
/// </summary>
public sealed class UpdateTagRequest
{
    /// <summary>
    /// Gets or sets the new name of the targeted tag to change to.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string NewName { get; set; }
}
