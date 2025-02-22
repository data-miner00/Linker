namespace Linker.Cli.Commands;

using System.ComponentModel.DataAnnotations;

internal sealed class SearchLinkCommandArguments
{
    [Required]
    public string Keyword { get; set; }

    public int? Top { get; set; }

    public int? Skip { get; set; }
}
