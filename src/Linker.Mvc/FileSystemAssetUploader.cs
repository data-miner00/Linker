namespace Linker.Mvc;

using Linker.Core.V2;
using Linker.Core.V2.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public sealed class FileSystemAssetUploader : IAssetUploader
{
    private readonly string basePath;
    private readonly string folder;
    private readonly IEnumerable<string> allowedExtensions;
    private readonly string qualifiedFolderPath;

    public FileSystemAssetUploader(string basePath, string folder, IEnumerable<string> allowedExtensions)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(basePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        ArgumentNullException.ThrowIfNull(allowedExtensions);
        this.basePath = basePath;
        this.folder = folder;
        this.allowedExtensions = allowedExtensions;
        this.qualifiedFolderPath = Path.Combine(basePath, folder);
    }

    public async Task<UploadResult> UploadAsync(IFormFile file)
    {
        var info = Directory.CreateDirectory(this.qualifiedFolderPath);

        var fileExtension = Path.GetExtension(file.FileName);

        if (!this.allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException($"The file format '{fileExtension}' is not accepted. Accepted formats are {string.Join(',', this.allowedExtensions)}");
        }

        var randomFileName = Path.GetRandomFileName();
        var fileSavePath = Path.Combine(this.qualifiedFolderPath, randomFileName);
        var filename = Path.ChangeExtension(fileSavePath, Path.GetExtension(file.FileName));
        using var fileStream = new FileStream(filename, FileMode.Create);

        await file.CopyToAsync(fileStream).ConfigureAwait(false);

        return new UploadResult
        {
            FileName = randomFileName + fileExtension,
            FileExtension = fileExtension,
            FolderPath = this.qualifiedFolderPath,
            FullPath = filename,
        };
    }
}
