namespace Linker.Core.V2.Models;

public sealed record UploadResult
{
    public string FileName { get; init; }

    public string FolderPath { get; init; }

    public string FullPath { get; init; }

    public string FileExtension { get; init; }
}
