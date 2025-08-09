namespace Linker.Cli.Commands;

/// <summary>
/// The arguments for visit links inside a list command.
/// </summary>
internal struct VisitListLinkCommandArguments
{
    /// <summary>
    /// Gets or sets the identifier of the list to visit links for.
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visit all links. This disrespects the <see cref="LinkIndex"/> argument.
    /// </summary>
    public bool All { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visit a link randomly. This disrespects the <see cref="LinkIndex"/> argument.
    /// </summary>
    public bool Random { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visit the last link in the list.
    /// </summary>
    public bool Last { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the help message and exit.
    /// </summary>
    public bool ShowHelp { get; set; }
}
