namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal class RemoveLinkFromListCommandArguments
{
    [Required]
    public int ListId { get; set; }

    [Required]
    public int LinkId { get; set; }
}
