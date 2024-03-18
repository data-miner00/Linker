namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Linker.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public sealed class WorkspaceController : Controller
{
    private readonly IWorkspaceRepository repository;
    private readonly IMapper mapper;

    public WorkspaceController(IWorkspaceRepository respository, IMapper mapper)
    {
        this.repository = respository;
        this.mapper = mapper;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string? UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    // GET: WorkspaceController/Index
    public async Task<IActionResult> Index()
    {
        var userId = this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return this.RedirectToAction(nameof(this.Explore));
        }

        var workspaces = await this.repository
            .GetAllByUserAsync(userId, this.CancellationToken)
            .ConfigureAwait(false);

        var viewModels = await this.ConvertWorkspacesToViewModelsAsync(workspaces);

        return this.View(viewModels);
    }

    // GET: WorkspaceController/Explore
    public async Task<IActionResult> Explore()
    {
        IEnumerable<Workspace> workspaces;

        if (string.IsNullOrEmpty(this.UserId))
        {
            workspaces = await this.repository
                .GetAllAsync(this.CancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            workspaces = await this.repository
                .GetAllUnjoinedByUserAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);
        }

        var viewModels = await this.ConvertWorkspacesToViewModelsAsync(workspaces);

        return this.View(viewModels);
    }

    // GET: WorkspaceController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var workspace = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            var users = await this.repository
                .GetWorkspaceMembersAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            var articles = await this.repository
                .GetWorkspaceArticlesAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            var websites = await this.repository
                .GetWorkspaceWebsitesAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            var youtubes = await this.repository
                .GetWorkspaceYoutubesAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            var viewModel = new WorkspaceDetailsViewModel
            {
                WorkspaceId = workspace.Id,
                WorkspaceHandle = workspace.Handle,
                WorkspaceDescription = workspace.Description,
                WorkspaceName = workspace.Name,
                WorkspaceCreatedAt = workspace.CreatedAt,
                WorkspaceOwnerUsername = users.FirstOrDefault(x => x.Id == workspace.OwnerId)!.Username,
                WorkspaceOwnerId = workspace.OwnerId,
                Members = users,
                Articles = articles,
                Websites = websites,
                Youtubes = youtubes,
            };

            return this.View(viewModel);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    // GET: WorkspaceController/Create
    public IActionResult Create()
    {
        return this.View();
    }

    // POST: WorkspaceController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateWorkspaceRequest request)
    {
        var newWorkspace = this.mapper.Map<Workspace>(request);
        newWorkspace.OwnerId = this.HttpContext.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        try
        {
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .CreateAsync(newWorkspace, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData["success"] = "Workspace created successfully.";

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(request);
        }
        catch
        {
            return this.View(request);
        }
    }

    // GET: WorkspaceController/Join
    public async Task<IActionResult> Join(Guid workspaceId)
    {
        if (string.IsNullOrEmpty(this.UserId))
        {
            return this.RedirectToAction(nameof(this.Index));
        }

        try
        {
            var membership = new WorkspaceMembership
            {
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                UserId = this.UserId,
                WorkspaceId = workspaceId.ToString(),
                WorkspaceRole = WorkspaceRole.User,
            };

            await this.repository
                .AddWorkspaceMembershipAsync(membership, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully joined";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "The workspace does not exist.";

            return this.RedirectToAction(nameof(this.Index));
        }
    }

    public async Task<IActionResult> AddArticle()
    {
        return this.PartialView("_AddArticlePartial");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddArticle(Guid workspaceId, Guid articleId)
    {
        try
        {
            await this.repository
                .AddWorkspaceArticleAsync(workspaceId.ToString(), articleId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully added article to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Failed to add article to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    // TODO: Change to Post
    public async Task<IActionResult> RemoveArticle(Guid workspaceId, Guid articleId)
    {
        try
        {
            await this.repository
                .RemoveWorkspaceArticleAsync(workspaceId.ToString(), articleId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully deleted article from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Failed to remove article from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddWebsite(Guid workspaceId, Guid websiteId)
    {
        try
        {
            await this.repository
                .AddWorkspaceWebsiteAsync(workspaceId.ToString(), websiteId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully added website to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Failed to add website to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    // TODO: Change to Post
    public async Task<IActionResult> RemoveWebsite(Guid workspaceId, Guid websiteId)
    {
        try
        {
            await this.repository
                .RemoveWorkspaceWebsiteAsync(workspaceId.ToString(), websiteId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully deleted website from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Failed to remove website from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddYoutube(Guid workspaceId, Guid youtubeId)
    {
        try
        {
            await this.repository
                .AddWorkspaceYoutubeAsync(workspaceId.ToString(), youtubeId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully added youtube to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Failed to add youtube to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    // TODO: Change to Post
    public async Task<IActionResult> RemoveYoutube(Guid workspaceId, Guid youtubeId)
    {
        try
        {
            await this.repository
                .RemoveWorkspaceYoutubeAsync(workspaceId.ToString(), youtubeId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully deleted youtube from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "Failed to remove youtube from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    // POST: WorkspaceController/Leave/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Leave(Guid workspaceId)
    {
        if (string.IsNullOrEmpty(this.UserId))
        {
            return this.RedirectToAction(nameof(this.Index));
        }

        try
        {
            await this.repository
                .DeleteWorkspaceMembershipAsync(workspaceId.ToString(), this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Successfully left the workspace";
            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            this.TempData["error"] = "You are not a member of the workspace.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }

    // GET: WorkspaceController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var workspace = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(workspace);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    // POST: WorkspaceController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateWorkspaceRequest request)
    {
        var workspace = this.mapper.Map<Workspace>(request);
        workspace.Id = id.ToString();

        try
        {
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .UpdateAsync(workspace, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData["success"] = "Successfully updated workspace.";

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(request);
        }
        catch
        {
            return this.View(request);
        }
    }

    // GET: WorkspaceController/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData["success"] = "Workspace successfully deleted.";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
        catch
        {
            this.TempData["error"] = "Something wrong.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }

    private async Task<IEnumerable<WorkspaceIndexViewModel>> ConvertWorkspacesToViewModelsAsync(IEnumerable<Workspace> workspaces)
    {
        var viewModel = new List<WorkspaceIndexViewModel>();

        foreach (var workspace in workspaces)
        {
            var members = await this.repository
                .GetAllWorkspaceMembershipsAsync(workspace.Id, this.CancellationToken)
                .ConfigureAwait(false);

            var model = new WorkspaceIndexViewModel
            {
                Id = workspace.Id,
                Handle = workspace.Handle,
                Name = workspace.Name,
                Description = workspace.Description,
                OwnerId = workspace.OwnerId,
                MemberCounts = members.Count(),
            };

            viewModel.Add(model);
        }

        return viewModel;
    }
}
