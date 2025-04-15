namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal class GetListCommandArguments
{
    [Required]
    public int Id { get; set; }

    public bool Name { get; set; }

    public bool Description { get; set; }

    public bool Links { get; set; }

    public bool ShowHelp { get; set; }
}
