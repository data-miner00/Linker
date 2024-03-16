namespace Linker.Core.Repositories;

using Linker.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The contract for workspace repository.
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
    /// Retrieves all articles of a workspace.
    /// </summary>
    /// <param name="id">The ID of the workspace.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The articles of the workspace.</returns>
    Task<IEnumerable<Article>> GetWorkspaceArticlesAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all websites of a workspace.
    /// </summary>
    /// <param name="id">The ID of the workspace.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The websites of the workspace.</returns>
    Task<IEnumerable<Website>> GetWorkspaceWebsitesAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all youtubes of a workspace.
    /// </summary>
    /// <param name="id">The ID of the workspace.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The youtubes of the workspace.</returns>
    Task<IEnumerable<Youtube>> GetWorkspaceYoutubesAsync(string id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a user into a workspace.
    /// </summary>
    /// <param name="membership">The workspace membership.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddWorkspaceMembershipAsync(WorkspaceMembership membership, CancellationToken cancellationToken);

    /// <summary>
    /// Adds an article to the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="articleId">The article ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddWorkspaceArticleAsync(string workspaceId, string articleId, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a website to the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="websiteId">The website ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddWorkspaceWebsiteAsync(string workspaceId, string websiteId, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a youtube to the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="youtubeId">The youtube ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddWorkspaceYoutubeAsync(string workspaceId, string youtubeId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes an article in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="articleId">The article ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveWorkspaceArticleAsync(string workspaceId, string articleId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a website in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="websiteId">The website ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveWorkspaceWebsiteAsync(string workspaceId, string websiteId, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a youtube in the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="youtubeId">The youtube ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveWorkspaceYoutubeAsync(string workspaceId, string youtubeId, CancellationToken cancellationToken);

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
