namespace Linker.WebApi.ApiModels;

using System.ComponentModel.DataAnnotations;

public class LinkApiModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the object.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the url of the link.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the category of the link.
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the description/summary of the link.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the tags of the link.
    /// </summary>
    public IEnumerable<string> Tags { get; set; }

    /// <summary>
    /// Gets or sets the language of the link.
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// Gets or sets the rating of the link.
    /// </summary>
    public string Rating { get; set; }

    /// <summary>
    /// Gets or sets the last visit of the link.
    /// </summary>
    public DateTime LastVisitAt { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the link entry.
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:d MMMM, yyyy}")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the modified date of the link entry.
    /// </summary>
    public DateTime ModifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the creator of the link.
    /// </summary>
    public string CreatedBy { get; set; }
}
