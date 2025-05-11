namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The command arguments for searching link lists.
/// </summary>
internal sealed class SearchListCommandArguments
{
    /// <summary>
    /// Gets or sets the keyword to search.
    /// </summary>
    [Required]
    public string Keyword { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether to show the help message and exit.
    /// </summary>
    public bool ShowHelp { get; set; }
}
