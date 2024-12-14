namespace Linker.Mvc.Models;

using Linker.Core.V2.Models;

public sealed class PlaylistDetailsViewModel
{
    public Playlist Playlist { get; set; }

    public IEnumerable<Link> Links { get; set; }
}
