namespace Linker.Cli.Commands;

internal sealed class ExportLinksCommandArgument
{
    public string? Path { get; set; }

    public string? Format => throw new NotImplementedException();
}
