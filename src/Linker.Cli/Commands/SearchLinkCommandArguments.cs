namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The search link command arguments.
/// </summary>
internal sealed class SearchLinkCommandArguments
{
    /// <summary>
    /// Gets or sets the keyword to search.
    /// </summary>
    [Required]
    public string Keyword { get; set; } = null!;

    /// <summary>
    /// Gets or sets the number of first few links to retrieve.
    /// </summary>
    public int? Top { get; set; }

    /// <summary>
    /// Gets or sets the number of links to be skipped.
    /// </summary>
    public int? Skip { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to scope the search to the tags.
    /// </summary>
    public bool Tags { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the help message and exit.
    /// </summary>
    public bool ShowHelp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visit the link.
    /// </summary>
    public bool Visit { get; set; }
}
