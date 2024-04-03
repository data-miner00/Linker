namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Linker.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Security.Claims;

[Authorize]
public sealed class WorkspaceController : Controller
{
    private readonly IWorkspaceRepository repository;
    private readonly IUserRepository userRepository;
    private readonly ILinkRepository linkRepository;
    private readonly IMapper mapper;

    public WorkspaceController(
        IWorkspaceRepository repository,
        IUserRepository userRepository,
        ILinkRepository linkRepository,
        IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(linkRepository);
        ArgumentNullException.ThrowIfNull(mapper);

        this.repository = repository;
        this.userRepository = userRepository;
        this.linkRepository = linkRepository;
        this.mapper = mapper;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

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

            var links = await this.repository
                .GetWorkspaceLinksAsync(id.ToString(), this.CancellationToken)
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
                Links = links,
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
        newWorkspace.OwnerId = this.UserId;

        try
        {
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .CreateAsync(newWorkspace, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Workspace created successfully.";

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

            this.TempData[Constants.Success] = "Successfully joined";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "The workspace does not exist.";

            return this.RedirectToAction(nameof(this.Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Kick(Guid workspaceId, Guid userId)
    {
        try
        {
            await this.repository
                .DeleteWorkspaceMembershipAsync(workspaceId.ToString(), userId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Removed user from workspace";

            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "The user is not in the workspace.";

            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    public async Task<IActionResult> AddLink(Guid workspaceId)
    {
        try
        {
            var allLinks = await this.linkRepository
                .GetAllAsync(this.CancellationToken)
                .ConfigureAwait(false);

            return this.PartialView("_AddLinkPartial", (allLinks, workspaceId.ToString()));
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "A task was canceled";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddLink(Guid workspaceId, Guid linkId)
    {
        try
        {
            await this.repository
                .AddWorkspaceLinkAsync(workspaceId.ToString(), linkId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully added article to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "Failed to add article to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
    }

    // TODO: Change to Post
    public async Task<IActionResult> RemoveLink(Guid workspaceId, Guid linkId)
    {
        try
        {
            await this.repository
                .RemoveWorkspaceLinkAsync(workspaceId.ToString(), linkId.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully deleted article from workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "Failed to remove article from workspace.";
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

            this.TempData[Constants.Success] = "Successfully left the workspace";
            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            this.TempData[Constants.Error] = "You are not a member of the workspace.";
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

                this.TempData[Constants.Success] = "Successfully updated workspace.";

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

            this.TempData[Constants.Success] = "Workspace successfully deleted.";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
        catch
        {
            this.TempData[Constants.Error] = "Something wrong.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }

    public async Task<IActionResult> InviteUser(Guid workspaceId)
    {
        try
        {
            var users = await this.userRepository
                .GetAllAsync(this.CancellationToken)
                .ConfigureAwait(false);

            return this.PartialView("_InviteUser", (users, workspaceId.ToString()));
        }
        catch
        {
            return this.NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InviteUser(Guid workspaceId, Guid userId)
    {
        try
        {
            var membership = new WorkspaceMembership
            {
                UserId = userId.ToString(),
                WorkspaceId = workspaceId.ToString(),
                WorkspaceRole = WorkspaceRole.User,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            await this.repository
                .AddWorkspaceMembershipAsync(membership, default)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully invited user to workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
        }
        catch (SqlException)
        {
            this.TempData[Constants.Error] = "User already in workspace.";
            return this.RedirectToAction(nameof(this.Details), new { id = workspaceId.ToString() });
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

            var links = await this.repository
                .GetWorkspaceLinksAsync(workspace.Id, this.CancellationToken)
                .ConfigureAwait(false);

            var model = new WorkspaceIndexViewModel
            {
                Id = workspace.Id,
                Handle = workspace.Handle,
                Name = workspace.Name,
                Description = workspace.Description,
                OwnerId = workspace.OwnerId,
                MemberCounts = members.Count(),
                LinkCounts = links.Count(),
            };

            viewModel.Add(model);
        }

        return viewModel;
    }
}
