namespace Linker.Cli.Core;

using System.Text.Json.Serialization;

public class Link
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Url { get; set; }

    public string? Description { get; set; }

    public string? Language { get; set; }

    public bool WatchLater { get; set; }

    public string? Tags { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    [JsonIgnore]
    public ICollection<UrlList> Lists { get; set; } = [];

    [JsonIgnore]
    public ICollection<Visit> Visits { get; set; } = [];
}
