namespace Linker.Cli.Commands;

using Linker.Cli.Core;
using System;
using System.ComponentModel.DataAnnotations;

internal class AddLinkCommandArguments
{
    [Required]
    public string Url { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool WatchLater { get; set; }

    public string? Tags { get; set; }

    public string? Language { get; set; }

    public Link ToLink()
    {
        return new Link
        {
            Url = Url,
            Name = Name,
            Description = Description,
            WatchLater = WatchLater,
            Tags = Tags,
            Language = Language,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
        };
    }
}
