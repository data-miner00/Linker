namespace Linker.Core.V2.QueryParams;

using Linker.Core.V2.Models;

public sealed class GetLinksQueryParams
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Domain { get; set; }
    public Category? Category { get; set; }
    public Language? Language { get; set; }
    public Rating? Rating { get; set; }
    public Aesthetics? Aesthetics { get; set; }
    public Grammar? Grammar { get; set; }
    public string? Country { get; set; }
    public string? KeyPersonName { get; set; }
    public DateTime? CreatedAtStart { get; set; }
    public DateTime? CreatedAtEnd { get; set; }
}
