namespace Linker.Core.V2;

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

/// <summary>
/// Specifies an asset uploader.
/// </summary>
public interface IAssetUploader
{
    /// <summary>
    /// Uploads a file to the server.
    /// </summary>
    /// <param name="id">The Id of the file.</param>
    /// <param name="file">The file object.</param>
    /// <returns>The task.</returns>
    Task UploadAsync(Guid id, IFormFile file);
}
