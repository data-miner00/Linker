namespace Linker.WebJob;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The image metadata handler abstraction.
/// </summary>
internal interface IImageMetadataHandler
{
    /// <summary>
    /// Handles a single link to update the open graph image.
    /// </summary>
    /// <param name="linkId">The link Id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task HandleAsync(string linkId, CancellationToken cancellationToken);
}
