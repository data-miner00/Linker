namespace Linker.Core.CsvModels;

using System;
using Linker.Core.Models;

/// <summary>
/// The CSV data transfer object for <see cref="Article"/>.
/// </summary>
public class CsvArticle
{
    /// <summary>
    /// Gets or sets the unique identifier for the object.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the article.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the author of the article.
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the year of the published article.
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Gets or sets the url of the article.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the category of the article.
    /// </summary>
    public int Category { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the article is listed as watch later.
    /// </summary>
    public bool WatchLater { get; set; }

    /// <summary>
    /// Gets or sets the domain of the article.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Gets or sets the description/summary of the article.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the tags of the article.
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// Gets or sets the language of the article.
    /// </summary>
    public int Language { get; set; }

    /// <summary>
    /// Gets or sets the grammatical level of the level.
    /// </summary>
    public int Grammar { get; set; }

    /// <summary>
    /// Gets or sets the last visit of the article.
    /// </summary>
    public DateTime LastVisitAt { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the article link entry.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the modified date of the article link entry.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
