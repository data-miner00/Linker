namespace Linker.Core.Models;

using System;

public class WorkspaceMembership
{
    required public string WorkspaceId { get; set; }

    required public string UserId { get; set; }

    required public WorkspaceRole WorkspaceRole { get; set; }

    required public DateTime CreatedAt { get; set; }

    required public DateTime ModifiedAt { get; set; }
}
