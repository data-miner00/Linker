namespace Linker.Core.ApiModels;

using Linker.Core.Models;
using System;

public sealed class CreateWorkspaceMembershipRequest
{
    required public Guid WorkspaceId { get; set; }

    required public Guid UserId { get; set; }

    required public WorkspaceRole WorkspaceRole { get; set; }
}
