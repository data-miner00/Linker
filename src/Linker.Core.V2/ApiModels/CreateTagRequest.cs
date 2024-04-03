namespace Linker.Core.V2.ApiModels;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The create tag request object.
/// </summary>
public sealed class CreateTagRequest
{
    /// <summary>
    /// Gets or sets the name of the tag to be created.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string TagName { get; set; }
}
