namespace Linker.Cli.Commands;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

internal class UpdateLinkCommandArguments
{
    [Required]
    public int Id { get; set; }

    public string? Url { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool WatchLater { get; set; }

    public bool NoWatchLater { get; set; }

    public string? Tags { get; set; }

    public bool ClearTags { get; set; }

    public IList<string> AddTags { get; set; } = [];

    public IList<string> RemoveTags { get; set; } = [];

    public string? Language { get; set; }
}
