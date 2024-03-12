﻿namespace Linker.Mvc.Controllers;

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

    // GET: WorkspaceController/Explore
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
        var workspaces = await this.repository
            .GetAllAsync(this.CancellationToken)
            .ConfigureAwait(false);

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

            var viewModel = new WorkspaceDetailsViewModel
            {
                WorkspaceId = workspace.Id,
                WorkspaceHandle = workspace.Handle,
                WorkspaceDescription = workspace.Description,
                WorkspaceName = workspace.Name,
                WorkspaceCreatedAt = workspace.CreatedAt,
                WorkspaceOwnerUsername = users.FirstOrDefault(x => x.Id == workspace.OwnerId)!.Username,
                Members = users,
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