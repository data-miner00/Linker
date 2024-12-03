﻿namespace Linker.Mvc.Controllers;

using Linker.Core.V2.Repositories;
using Linker.Core.V2;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Linker.Common.Helpers;
using Serilog;
using System.Text.Json;
using System.Text;

public sealed class UserController : Controller
{
    private readonly IUserRepository repository;
    private readonly IAssetUploader assetUploader;
    private readonly IWorkspaceRepository workspaceRepository;
    private readonly ILogger logger;
    private readonly ILinkRepository linkRepository;

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    public UserController(
        IUserRepository repository,
        IAssetUploader assetUploader,
        IWorkspaceRepository workspaceRepository,
        ILinkRepository linkRepository,
        ILogger logger)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.assetUploader = Guard.ThrowIfNull(assetUploader);
        this.workspaceRepository = Guard.ThrowIfNull(workspaceRepository);
        this.logger = Guard.ThrowIfNull(logger);
        this.linkRepository = Guard.ThrowIfNull(linkRepository);
    }

    // GET: UserController
    public async Task<IActionResult> Index()
    {
        try
        {
            var user = await this.repository
                .GetByIdAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            var joinedWorkspaces = await this.workspaceRepository
                .GetAllByUserAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            var posts = await this.linkRepository
                .GetAllByUserAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            return this.View((user, joinedWorkspaces, posts));
        }
        catch (InvalidOperationException ex)
        {
            this.logger.Error(ex, "Something went wrong: {message}", ex.Message);

            return this.NotFound();
        }
    }

    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var user = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            var joinedWorkspaces = await this.workspaceRepository
                .GetAllByUserAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            return this.View((user, joinedWorkspaces));
        }
        catch (InvalidOperationException ex)
        {
            this.logger.Error(ex, "Something went wrong: {message}", ex.Message);

            return this.NotFound();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        Guard.ThrowIfNull(file);

        try
        {
            var uploadResult = await this.assetUploader
                .UploadAsync(file)
                .ConfigureAwait(false);

            var user = await this.repository
                .GetByIdAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            user.PhotoUrl = uploadResult.FullPath.StartsWith(".\\wwwroot")
                ? uploadResult.FullPath.Replace(".\\wwwroot", "https://localhost:7201")
                : uploadResult.FullPath;

            await this.repository
                .UpdateAsync(user, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Profile photo uploaded successfully.";
            return this.RedirectToAction(nameof(this.Index));
        }
        catch (Exception ex)
        {
            this.TempData[Constants.Error] = ex.Message;
            this.logger.Error(ex, "Something went wrong: {message}", ex.Message);
            return this.RedirectToAction(nameof(this.Index));
        }
    }

    public async Task<IActionResult> Export()
    {
        var posts = await this.linkRepository.GetAllByUserAsync(this.UserId, this.CancellationToken);

        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
        };

        var jsonString = JsonSerializer.Serialize(posts, options);
        var byteString = Encoding.UTF8.GetBytes(jsonString);

        var memoryStream = new MemoryStream(byteString);

        return this.File(memoryStream, "application/octet-stream", "exports.json");
    }
}
