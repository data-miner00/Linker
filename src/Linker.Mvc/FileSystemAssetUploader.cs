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
    private readonly string qualifiedFolderPath;

    public FileSystemAssetUploader(string basePath, string folder)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(basePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        this.basePath = basePath;
        this.folder = folder;
        this.qualifiedFolderPath = Path.Combine(basePath, folder);
    }

    public async Task<UploadResult> UploadAsync(IFormFile file)
    {
        var info = Directory.CreateDirectory(this.qualifiedFolderPath);

        var fileExtension = Path.GetExtension(file.FileName);
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
