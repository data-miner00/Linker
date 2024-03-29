namespace Linker.Core.V2.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using Linker.Core.V2.Models;

/// <summary>
/// The contract for <see cref="Workspace"/> repository.
/// </summary>
public interface IWorkspaceRepository
{
    /// <summary>
    /// Creates a new workspace.
    /// </summary>
    /// <param name="workspace">The workspace to be created.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task CreateAsync(Workspace workspace, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all of the existing workspaces.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The full list of workspaces.</returns>
    Task<IEnumerable<Workspace>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the workspace by ID.
    /// </summary>
    /// <param name="id">The workspace ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The found workspace.</returns>
    Task<Workspace> GetByIdAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all workspaces by a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of workspaces of the user.</returns>
    Task<IEnumerable<Workspace>> GetAllByUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all the workspaces that is not joined by the user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of unjoined workspaces.</returns>
    Task<IEnumerable<Workspace>> GetAllUnjoinedByUserAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a workspace.
    /// </summary>
    /// <param name="id">The workspace ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Update a workspace.
    /// </summary>
    /// <param name="workspace">The workspace to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task UpdateAsync(Workspace workspace, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all links of a workspace.
    /// </summary>
    /// <param name="id">The ID of the workspace.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The links of the workspace.</returns>
    Task<IEnumerable<Link>> GetWorkspaceLinksAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a user into a workspace.
    /// </summary>
    /// <param name="membership">The workspace membership.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddWorkspaceMembershipAsync(WorkspaceMembership membership, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a link to the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="linkId">The link ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddWorkspaceLinkAsync(string workspaceId, string linkId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a link in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="linkId">The link ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveWorkspaceLinkAsync(string workspaceId, string linkId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all memberships of a workspace.
    /// </summary>
    /// <param name="id">The workspace ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of workspace memberships.</returns>
    Task<IEnumerable<WorkspaceMembership>> GetAllWorkspaceMembershipsAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a membership of a user in a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The membership.</returns>
    Task<WorkspaceMembership> GetWorkspaceMembershipAsync(string workspaceId, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates a workspace membership.
    /// </summary>
    /// <param name="workspaceMembership">The workspace membership.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task UpdateWorkspaceMembershipAsync(WorkspaceMembership workspaceMembership, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a user from a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task DeleteWorkspaceMembershipAsync(string workspaceId, string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves the members of a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The list of members.</returns>
    Task<IEnumerable<User>> GetWorkspaceMembersAsync(string workspaceId, CancellationToken cancellationToken);
}
