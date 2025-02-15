namespace Linker.Cli.Core;

using System;

public class UrlList
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public ICollection<Link> Links { get; set; } = [];
}
