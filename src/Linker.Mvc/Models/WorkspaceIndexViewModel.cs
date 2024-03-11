namespace Linker.Mvc.Models;

public sealed class WorkspaceIndexViewModel
{
    required public string Id { get; set; }

    required public string Handle { get; set; }

    required public string Name { get; set; }

    required public string Description { get; set; }

    required public string OwnerId { get; set; }

    required public int MemberCounts { get; set; }
}
