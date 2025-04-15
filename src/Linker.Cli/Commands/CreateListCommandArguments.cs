namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal class CreateListCommandArguments
{
    [Required]
    public string Name { get; set; }

    public string? Description { get; set; }

    public bool ShowHelp { get; set; }
}
