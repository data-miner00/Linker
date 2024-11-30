namespace Linker.Mvc.Models;

using Linker.Core.V2.Models;

public sealed class HomeViewModel
{
    public IEnumerable<string> TrendingTags { get; set; }

    public IEnumerable<Link> TrendingLinks { get; set; }

    public IEnumerable<Link> LatestLinks { get; set; }

    public IEnumerable<User> Users { get; set; }

    public IEnumerable<Workspace> Workspaces { get; set; }
}
