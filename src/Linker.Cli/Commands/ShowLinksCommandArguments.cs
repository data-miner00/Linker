namespace Linker.Cli.Commands;

internal class ShowLinksCommandArguments
{
    public int? Top { get; set; }

    public int? Skip { get; set; }

    public bool WatchLater { get; set; }

    public int? Last { get; set; }
}
