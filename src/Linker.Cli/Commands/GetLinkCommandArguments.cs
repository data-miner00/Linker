namespace Linker.Cli.Commands;

internal class GetLinkCommandArguments
{
    public int Id { get; set; }

    public bool Url { get; set; }

    public bool Name { get; set; }

    public bool Description { get; set; }

    public bool WatchLater { get; set; }

    public bool Tags { get; set; }

    public bool Language { get; set; }

    public bool CreatedAt { get; set; }

    public bool ModifiedAt { get; set; }

    public bool ShowHelp { get; set; }
}
