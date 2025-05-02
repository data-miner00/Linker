namespace Linker.Cli.Commands;

/// <summary>
/// Command arguments for the export links command.
/// </summary>
internal sealed class ExportLinksCommandArgument
{
    /// <summary>
    /// Gets or sets the path to the file where the links will be exported.
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Gets or sets the format of the export. Supported formats are "json" and "csv".
    /// </summary>
    public ExportFormat? Format { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show help information.
    /// </summary>
    public bool ShowHelp { get; set; }
}
