namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal class DeleteLinkCommandArguments
{
    [Required]
    public int Id { get; set; }

    public bool ConfirmDelete { get; set; }
}
