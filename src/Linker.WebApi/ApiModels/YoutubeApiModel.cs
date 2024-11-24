namespace Linker.WebApi.ApiModels;

public sealed class YoutubeApiModel : LinkApiModel
{
    /// <summary>
    /// Gets or sets the name of the Youtube Channel.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the Youtuber.
    /// </summary>
    public string Youtuber { get; set; }

    /// <summary>
    /// Gets or sets the country of the Youtube Channel.
    /// </summary>
    public string Country { get; set; }
}
