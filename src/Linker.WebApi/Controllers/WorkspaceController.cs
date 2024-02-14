namespace Linker.WebApi.Controllers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Controllers;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using EnsureThat;
using Linker.WebApi.Filters;

/// <summary>
/// The workspace controller for Http requests.
/// </summary>
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class WorkspaceController : ControllerBase, IWorkspaceController
{
    private readonly IWorkspaceRepository repository;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor context;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceController"/> class.
    /// </summary>
    /// <param name="repository">The repository for <see cref="Workspace"/>.</param>
    /// <param name="mapper">The mapper.</param>
    /// <param name="context">The Http accessor context.</param>
    public WorkspaceController(
        IWorkspaceRepository repository,
        IMapper mapper,
        IHttpContextAccessor context)
    {
        this.repository = EnsureArg.IsNotNull(repository, nameof(repository));
        this.mapper = EnsureArg.IsNotNull(mapper, nameof(mapper));
        this.context = EnsureArg.IsNotNull(context, nameof(context));
    }

    /// <inheritdoc/>
    [HttpPost("", Name = "CreateWorkspace")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateWorkspaceRequest request)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        var userId = this.context.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        var workspace = this.mapper.Map<Workspace>(request);
        workspace.OwnerId = userId;

        await this.repository.CreateAsync(workspace, default).ConfigureAwait(false);

        return this.Created();
    }

    /// <inheritdoc/>
    [MinimumRoleAuthorize(Role.Administrator)]
    [HttpDelete("{id:guid}", Name = "DeleteWorkspace")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        EnsureArg.IsNotDefault(id, nameof(id));

        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), default)
                .ConfigureAwait(false);

            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [MinimumRoleAuthorize(Role.Administrator)]
    [HttpGet("", Name = "GetAllWorkspaces")]
    public async Task<IActionResult> GetAllAsync()
    {
        var workspaces = await this.repository
            .GetAllAsync(default)
            .ConfigureAwait(false);

        return this.Ok(workspaces);
    }

    /// <inheritdoc/>
    [MinimumRoleAuthorize(Role.Administrator)]
    [HttpGet("{id:guid}", Name = "GetWorkspaceById")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        EnsureArg.IsNotDefault(id, nameof(id));

        try
        {
            var workspace = await this.repository
                .GetByIdAsync(id.ToString(), default)
                .ConfigureAwait(false);

            return this.Ok(workspace);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound(id);
        }
    }

    /// <inheritdoc/>
    [HttpPut("{id:guid}", Name = "UpdateWorkspace")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWorkspaceRequest request)
    {
        EnsureArg.IsNotDefault(id, nameof(id));
        EnsureArg.IsNotNull(request, nameof(request));

        try
        {
            var existing = await this.repository
                .GetByIdAsync(id.ToString(), default)
                .ConfigureAwait(false);

            var workspace = this.mapper.Map<Workspace>(request);
            workspace.Id = id.ToString();

            await this.repository
                .UpdateAsync(workspace, default)
                .ConfigureAwait(false);

            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [AccountAuthorize]
    [HttpGet("byuser/{userId:guid}", Name = "GetWorkspacesByUser")]
    public async Task<IActionResult> GetWorkspaceByUserAsync(Guid userId)
    {
        EnsureArg.IsNotDefault(userId, nameof(userId));

        try
        {
            var workspaces = await this.repository
                .GetAllByUserAsync(userId.ToString(), default)
                .ConfigureAwait(false);

            return this.Ok(workspaces);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpPost("membership", Name = "AddWorkspaceMembership")]
    public async Task<IActionResult> AddWorkspaceMembershipAsync(CreateWorkspaceMembershipRequest request)
    {
        EnsureArg.IsNotNull(request, nameof(request));

        var membership = this.mapper.Map<WorkspaceMembership>(request);

        await this.repository
            .AddWorkspaceMembershipAsync(membership, default)
            .ConfigureAwait(false);

        return this.Created();
    }

    /// <inheritdoc/>
    [AccountAuthorize]
    [HttpDelete("membership/{workspaceId:guid}/{userId:guid}", Name = "DeleteWorkspaceMembership")]
    public async Task<IActionResult> DeleteWorkspaceMembershipAsync(Guid workspaceId, Guid userId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(userId, nameof(userId));

        try
        {
            await this.repository
                .DeleteWorkspaceMembershipAsync(workspaceId.ToString(), userId.ToString(), default)
                .ConfigureAwait(false);

            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpGet("article/{workspaceId:guid}", Name = "GetWorkspaceArticles")]
    public async Task<IActionResult> GetWorkspaceArticlesAsync(Guid workspaceId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));

        try
        {
            var articles = await this.repository
                .GetWorkspaceArticlesAsync(workspaceId.ToString(), default)
                .ConfigureAwait(false);

            return this.Ok(articles);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpPost("article/{workspaceId:guid}/{articleId:guid}", Name = "AddWorkspaceArticle")]
    public async Task<IActionResult> AddWorkspaceArticleAsync(Guid workspaceId, Guid articleId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(articleId, nameof(articleId));

        await this.repository
            .AddWorkspaceArticleAsync(workspaceId.ToString(), articleId.ToString(), default)
            .ConfigureAwait(false);

        return this.Created();
    }

    /// <inheritdoc/>
    [HttpDelete("article/{workspaceId:guid}/{articleId:guid}", Name = "DeleteWorkspaceArticle")]
    public async Task<IActionResult> DeleteWorkspaceArticleAsync(Guid workspaceId, Guid articleId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(articleId, nameof(articleId));

        try
        {
            await this.repository
                .RemoveWorkspaceArticleAsync(workspaceId.ToString(), articleId.ToString(), default)
                .ConfigureAwait(false);

            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpGet("website/{workspaceId:guid}", Name = "GetWorkspaceWebsites")]
    public async Task<IActionResult> GetWorkspaceWebsitesAsync(Guid workspaceId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));

        try
        {
            var websites = await this.repository
                .GetWorkspaceWebsitesAsync(workspaceId.ToString(), default)
                .ConfigureAwait(false);

            return this.Ok(websites);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpPost("website/{workspaceId:guid}/{websiteId:guid}", Name = "AddWorkspaceWebsite")]
    public async Task<IActionResult> AddWorkspaceWebsiteAsync(Guid workspaceId, Guid websiteId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(websiteId, nameof(websiteId));

        await this.repository
            .AddWorkspaceWebsiteAsync(workspaceId.ToString(), websiteId.ToString(), default)
            .ConfigureAwait(false);

        return this.Created();
    }

    /// <inheritdoc/>
    [HttpDelete("website/{workspaceId:guid}/{websiteId:guid}", Name = "DeleteWorkspaceWebsite")]
    public async Task<IActionResult> DeleteWorkspaceWebsiteAsync(Guid workspaceId, Guid websiteId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(websiteId, nameof(websiteId));

        try
        {
            await this.repository
                .RemoveWorkspaceWebsiteAsync(workspaceId.ToString(), websiteId.ToString(), default)
                .ConfigureAwait(false);

            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpGet("youtube/{workspaceId:guid}", Name = "GetWorkspaceYoutubes")]
    public async Task<IActionResult> GetWorkspaceYoutubesAsync(Guid workspaceId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));

        try
        {
            var youtubes = await this.repository
                .GetWorkspaceYoutubesAsync(workspaceId.ToString(), default)
                .ConfigureAwait(false);

            return this.Ok(youtubes);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    /// <inheritdoc/>
    [HttpPost("youtube/{workspaceId:guid}/{youtubeId:guid}", Name = "AddWorkspaceYoutube")]
    public async Task<IActionResult> AddWorkspaceYoutubeAsync(Guid workspaceId, Guid youtubeId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(youtubeId, nameof(youtubeId));

        await this.repository
            .AddWorkspaceYoutubeAsync(workspaceId.ToString(), youtubeId.ToString(), default)
            .ConfigureAwait(false);

        return this.Created();
    }

    /// <inheritdoc/>
    [HttpDelete("youtube/{workspaceId:guid}/{youtubeId:guid}", Name = "DeleteWorkspaceYoutube")]
    public async Task<IActionResult> DeleteWorkspaceYoutubeAsync(Guid workspaceId, Guid youtubeId)
    {
        EnsureArg.IsNotDefault(workspaceId, nameof(workspaceId));
        EnsureArg.IsNotDefault(youtubeId, nameof(youtubeId));

        try
        {
            await this.repository
                .RemoveWorkspaceYoutubeAsync(workspaceId.ToString(), youtubeId.ToString(), default)
                .ConfigureAwait(false);

            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }
}
