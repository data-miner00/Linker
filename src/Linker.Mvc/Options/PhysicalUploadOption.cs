namespace Linker.Mvc.Options;

public sealed class PhysicalUploadOption
{
    public string BasePath { get; set; }

    public string FolderName { get; set; }

    public IEnumerable<string> AllowedExtensions { get; set; }
}
