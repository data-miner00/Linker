namespace Linker.Core.V2;

using Linker.Core.V2.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

/// <summary>
/// Specifies an asset uploader.
/// </summary>
public interface IAssetUploader
{
    /// <summary>
    /// Uploads a file to the server.
    /// </summary>
    /// <param name="file">The file object.</param>
    /// <returns>The task.</returns>
    Task<UploadResult> UploadAsync(IFormFile file);
}
