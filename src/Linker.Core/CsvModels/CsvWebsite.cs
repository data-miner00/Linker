﻿namespace Linker.Core.CsvModels;

using System;
using Linker.Core.Models;

/// <summary>
/// The CSV data transfer object for <see cref="Website"/>.
/// </summary>
public class CsvWebsite
{
    /// <summary>
    /// Gets or sets the unique identifier for the object.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the website.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the url of the website.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the category of the website.
    /// </summary>
    public int Category { get; set; }

    /// <summary>
    /// Gets or sets the domain of the website.
    /// </summary>
    public string Domain { get; set; }

    /// <summary>
    /// Gets or sets the description/summary of the website.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the tags of the website.
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// Gets or sets the main language of the website.
    /// </summary>
    public int MainLanguage { get; set; }

    /// <summary>
    /// Gets or sets the aesthetic value of the website.
    /// </summary>
    public int Aesthetics { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the website is a subdomain.
    /// </summary>
    public bool IsSubdomain { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the website is a multilingual.
    /// </summary>
    public bool IsMultilingual { get; set; }

    /// <summary>
    /// Gets or sets the last visit of the website.
    /// </summary>
    public DateTime LastVisitAt { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the website link entry.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the modified date of the website link entry.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
