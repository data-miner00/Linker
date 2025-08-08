namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The command arguments for creating a URL list collection.
/// </summary>
internal struct CreateListCommandArguments
{
    /// <summary>
    /// Gets or sets the name of the URL list collection.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the list.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show help or not.
    /// </summary>
    public bool ShowHelp { get; set; }
}
