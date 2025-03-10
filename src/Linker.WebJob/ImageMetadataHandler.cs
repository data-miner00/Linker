namespace Linker.WebJob;

using Linker.Core.V2.Repositories;
using System.Threading.Tasks;
using AngleSharp;
using Linker.Common.Helpers;

/// <summary>
/// Handler for thumbnails and favicons.
/// </summary>
internal sealed class ImageMetadataHandler : IImageMetadataHandler
{
    private const string OgImageCssSelector = "[property='og:image']";

    // https://stackoverflow.com/questions/5119041/how-can-i-get-a-web-sites-faviconElem
    private const string FaviconCssSelector = "[rel=\"shortcut icon\"], [rel=\"icon\"]";

    private readonly ILinkRepository linkRepository;
    private readonly IBrowsingContext browsingContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageMetadataHandler"/> class.
    /// </summary>
    /// <param name="linkRepository">The link repository.</param>
    public ImageMetadataHandler(ILinkRepository linkRepository)
    {
        this.linkRepository = Guard.ThrowIfNull(linkRepository);

        var config = Configuration.Default.WithDefaultLoader();
        this.browsingContext = BrowsingContext.New(config);
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string linkId, CancellationToken cancellationToken)
    {
        var link = await this.linkRepository.GetByIdAsync(linkId, cancellationToken);

        var document = await this.browsingContext.OpenAsync(link.Url, cancellationToken);
        var ogImageElem = document.QuerySelector(OgImageCssSelector);
        var faviconElem = document.QuerySelector(FaviconCssSelector);

        var ogImageUrl = ogImageElem?.GetAttribute("content");
        var faviconUrl = faviconElem?.GetAttribute("href");

        if (ogImageUrl is not null)
        {
            link.ThumbnailUrl = ogImageUrl;
        }

        if (!string.IsNullOrEmpty(faviconUrl))
        {
            link.FaviconUrl = faviconUrl;
        }

        if (!string.IsNullOrEmpty(ogImageUrl) || !string.IsNullOrEmpty(faviconUrl))
        {
            await this.linkRepository.UpdateAsync(link, cancellationToken);
        }
    }
}
