namespace Linker.Core.V2.ApiModels;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Linker.Core.V2.Models;

/// <summary>
/// The link creation request body.
/// </summary>
public sealed class CreateLinkRequest
{
    /// <summary>
    /// Gets or sets the name of the website.
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the url of the website.
    /// </summary>
    [Required]
    public string Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets the category of the website.
    /// </summary>
    [Required]
    public Category Category { get; set; }

    /// <summary>
    /// Gets or sets the description of the website.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the tags of the website.
    /// </summary>
    [Required]
    public IEnumerable<string> Tags { get; set; } = null!;

    /// <summary>
    /// Gets or sets the language of the website.
    /// </summary>
    [Required]
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets the rating of the article.
    /// </summary>
    [Required]
    public Rating Rating { get; set; }

    /// <summary>
    /// Gets or sets the aesthetics of the website.
    /// </summary>
    [Required]
    public Aesthetics Aesthetics { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the website is under a subdomain.
    /// </summary>
    [Required]
    public bool IsSubdomain { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the website supports multilingual.
    /// </summary>
    [Required]
    public bool IsMultilingual { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the link or website points to a resource.
    /// </summary>
    [Required]
    public bool IsResource { get; set; }

    /// <summary>
    /// Gets or sets the type of the link.
    /// </summary>
    [Required]
    public LinkType Type { get; set; }

    /// <summary>
    /// Gets or sets the country of the link if any.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets the key person name of the link if any.
    /// </summary>
    public string? KeyPersonName { get; set; }

    /// <summary>
    /// Gets or sets the grammar level of the link.
    /// </summary>
    [Required]
    public Grammar Grammar { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the link created.
    /// </summary>
    [Required]
    public Visibility Visibility { get; set; }
}
