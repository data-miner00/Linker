namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal class DeleteListCommandArguments
{
    [Required]
    public int Id { get; set; }

    public bool ConfirmDelete { get; set; }

    public bool ShowHelp { get; set; }
}
