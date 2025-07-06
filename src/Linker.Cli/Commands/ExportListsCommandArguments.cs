namespace Linker.Cli.Commands;

internal class ExportListsCommandArguments
{
    public string? Path { get; set; }

    public ExportFormat? Format { get; set; }

    public int? ListId { get; set; }

    public bool ShowHelp { get; set; }
}
