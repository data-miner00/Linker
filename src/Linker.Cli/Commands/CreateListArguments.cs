namespace Linker.Cli.Commands;

using System;
using System.ComponentModel.DataAnnotations;

internal class CreateListArguments
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}
