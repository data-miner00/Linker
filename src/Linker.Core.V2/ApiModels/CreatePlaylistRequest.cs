namespace Linker.Core.V2.ApiModels;

using Linker.Core.V2.Models;

public sealed class CreatePlaylistRequest
{
    public string Name { get; set; }

    public string Description { get; set; }

    public Visibility Visibility { get; set; }
}
