namespace Linker.Cli.Core;

using System;

public class Visit
{
    public int Id { get; set; }

    public int LinkId { get; set; }

    public Link Link { get; set; }

    public DateTime CreatedAt { get; set; }
}
