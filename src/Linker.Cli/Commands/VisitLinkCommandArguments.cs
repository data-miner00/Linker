namespace Linker.Cli.Commands;

/// <summary>
/// The arguments for visit link command.
/// </summary>
internal sealed class VisitLinkCommandArguments
{
    /// <summary>
    /// Gets or sets the ID for Link to be visited.
    /// </summary>
    public int? LinkId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visit a link randomly. This disrespects the <see cref="LinkId"/> argument.
    /// </summary>
    public bool Random { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to visit the last added link.
    /// </summary>
    public bool Last { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the help message and exit.
    /// </summary>
    public bool ShowHelp { get; set; }
}
