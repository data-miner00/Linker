﻿namespace Linker.Cli.Commands;

/// <summary>
/// The arguments for show links command.
/// </summary>
internal class ShowLinksCommandArguments
{
    /// <summary>
    /// Gets or sets the top few links to retrieve.
    /// </summary>
    public int? Top { get; set; } = 5;

    /// <summary>
    /// Gets or sets a value indicating whether to retrieve just the watch later links.
    /// </summary>
    public bool WatchLater { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the help message and exit.
    /// </summary>
    public bool ShowHelp { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show all links.
    /// </summary>
    public bool All { get; set; }
}
