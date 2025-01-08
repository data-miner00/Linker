namespace Linker.WebJob;

using Linker.Core.V2.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;

internal class ImageMetadataHandler : IImageMetadataHandler
{
    private const string OgImageSelector = "[property='og:image']";

    private readonly ILinkRepository linkRepository;
    private readonly HttpClient httpClient;

    public ImageMetadataHandler(
        ILinkRepository linkRepository,
        HttpClient httpClient)
    {
        this.linkRepository = linkRepository;
        this.httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task HandleAsync(string linkId, CancellationToken cancellationToken)
    {
        var link = await this.linkRepository.GetByIdAsync(linkId, cancellationToken);

        var ogImageUrl = await GetOgImage(link.Url);

        if (ogImageUrl is not null)
        {
            link.ThumbnailUrl = ogImageUrl;
            await this.linkRepository.UpdateAsync(link, cancellationToken);
        }
    }

    private static async Task<string?> GetOgImage(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(url);
        var meta = document.QuerySelector(OgImageSelector);

        return meta?.GetAttribute("content");
    }
}
