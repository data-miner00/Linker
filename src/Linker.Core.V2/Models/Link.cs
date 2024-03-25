namespace Linker.Core.V2.Models;

using System;
using System.Collections.Generic;

/// <summary>
/// The link or web URL that refers to some resource on the internet.
/// </summary>
public sealed class Link
{
    /// <summary>
    /// Gets or sets the unique identifier for the link.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the url of the link.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the category of the link.
    /// </summary>
    public LinkType Type { get; set; }

    /// <summary>
    /// Gets or sets the category of the link.
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Gets or sets the description/summary of the link.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the tags of the link.
    /// </summary>
    public IEnumerable<string> Tags { get; set; }

    /// <summary>
    /// Gets or sets the language of the link.
    /// </summary>
    public Language Language { get; set; }

    /// <summary>
    /// Gets or sets the rating of the link.
    /// </summary>
    public Rating Rating { get; set; }

    /// <summary>
    /// Gets or sets the creator of the link.
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the domain of the link.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Gets or sets the aesthetics of the link.
    /// </summary>
    public Aesthetics Aesthetics { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the link belongs to a subdomain.
    /// </summary>
    public bool IsSubdomain { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the link is multilingual.
    /// </summary>
    public bool IsMultilingual { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the link is a resource.
    /// A resource is something hosted on the domain, but not the main page itself.
    /// For example, the link to a Google Docs is considered as a resource.
    /// A dropbox link, social media posts are also defined as a resource.
    /// </summary>
    public bool IsResource { get; set; }

    /// <summary>
    /// Gets or sets the perceived country of the link.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets the key person or owner of the designated link.
    /// For article, the key person is the author.
    /// For websites, there are no key person, hence left it <c>null</c>.
    /// For Youtube channels, it is the name of the Youtuber, if identifiable.
    /// For social medias, it refers to the original poster.
    /// </summary>
    public string? KeyPersonName { get; set; }

    /// <summary>
    /// Gets or sets the grammar of the website.
    /// This is more appropriate on article-based links.
    /// </summary>
    public Grammar Grammar { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the link.
    /// </summary>
    public Visibility Visibility { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the link entry.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the modified date of the link entry.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
