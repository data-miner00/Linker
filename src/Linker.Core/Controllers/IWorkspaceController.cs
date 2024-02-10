namespace Linker.Core.Controllers;

using Linker.Core.ApiModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

/// <summary>
/// The interface for the workspace controller.
/// </summary>
public interface IWorkspaceController
{
    /// <summary>
    /// Get the workspace by ID.
    /// </summary>
    /// <param name="id">The workspace Id.</param>
    /// <returns>The found workspace.</returns>
    Task<IActionResult> GetByIdAsync(Guid id);

    /// <summary>
    /// Get all the existing workspaces.
    /// </summary>
    /// <returns>A list of workspaces.</returns>
    Task<IActionResult> GetAllAsync();

    /// <summary>
    /// Create a new workspace.
    /// </summary>
    /// <param name="request">The workspace creation request.</param>
    /// <returns>The Http response. </returns>
    Task<IActionResult> CreateAsync(CreateWorkspaceRequest request);

    /// <summary>
    /// Delete an existing workspace.
    /// </summary>
    /// <param name="id">The workspace ID.</param>
    /// <returns>The Http response.</returns>
    Task<IActionResult> DeleteAsync(Guid id);

    /// <summary>
    /// Update an existing workspace.
    /// </summary>
    /// <param name="id">The workspace ID.</param>
    /// <param name="request">The update workspace request.</param>
    /// <returns>The Http response.</returns>
    Task<IActionResult> UpdateAsync(Guid id, UpdateWorkspaceRequest request);

    /// <summary>
    /// Get all the workspace that the user has access to.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of user accessible workspaces.</returns>
    Task<IActionResult> GetWorkspaceByUserAsync(Guid userId);

    /// <summary>
    /// Adds a user into a workspace.
    /// </summary>
    /// <param name="request">The workspace membership creation request.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> AddWorkspaceMembershipAsync(CreateWorkspaceMembershipRequest request);

    /// <summary>
    /// Deletes a user from a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> DeleteWorkspaceMembershipAsync(Guid workspaceId, Guid userId);

    /// <summary>
    /// Retrieves all the articles that the workspace has.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <returns>The list of articles that the workspace has.</returns>
    Task<IActionResult> GetWorkspaceArticlesAsync(Guid workspaceId);

    /// <summary>
    /// Adds an article into the workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="articleId">The article ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> AddWorkspaceArticleAsync(Guid workspaceId, Guid articleId);

    /// <summary>
    /// Removes a article from a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="articleId">The article ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> DeleteWorkspaceArticleAsync(Guid workspaceId, Guid articleId);

    /// <summary>
    /// Retrieves the list of websites within a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> GetWorkspaceWebsitesAsync(Guid workspaceId);

    /// <summary>
    /// Adds a website into a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="websiteId">The website ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> AddWorkspaceWebsiteAsync(Guid workspaceId, Guid websiteId);

    /// <summary>
    /// Removes a website from a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="websiteId">The website ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> DeleteWorkspaceWebsiteAsync(Guid workspaceId, Guid websiteId);

    /// <summary>
    /// Retrieves a list of youtube links that a workspace has.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <returns>The list of youtube links.</returns>
    Task<IActionResult> GetWorkspaceYoutubesAsync(Guid workspaceId);

    /// <summary>
    /// Adds a youtube link into a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="youtubeId">The youtube ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> AddWorkspaceYoutubeAsync(Guid workspaceId, Guid youtubeId);

    /// <summary>
    /// Removes a youtube link from a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace ID.</param>
    /// <param name="youtubeId">The youtube ID.</param>
    /// <returns>Http response.</returns>
    Task<IActionResult> DeleteWorkspaceYoutubeAsync(Guid workspaceId, Guid youtubeId);
}
