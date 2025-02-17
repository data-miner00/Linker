namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal class AddLinkIntoListCommandArguments
{
    [Required]
    public int ListId { get; set; }

    [Required]
    public int LinkId { get; set; }
}
