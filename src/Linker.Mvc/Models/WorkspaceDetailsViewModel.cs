﻿namespace Linker.Mvc.Models;

using Linker.Core.Models;

public sealed class WorkspaceDetailsViewModel
{
    required public string WorkspaceId { get; set; }

    required public string WorkspaceName { get; set; }

    required public string WorkspaceHandle { get; set; }

    required public string WorkspaceDescription { get; set; }

    required public DateTime WorkspaceCreatedAt { get; set; }

    required public string WorkspaceOwnerUsername { get; set; }

    required public IEnumerable<User> Members { get; set; }
}