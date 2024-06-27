namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Common.Helpers;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Exceptions;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

[Authorize]
public sealed class PlaylistController : Controller
{
    private readonly IPlaylistRepository repository;
    private readonly ILogger logger;
    private readonly IMapper mapper;

    public PlaylistController(IPlaylistRepository repository, ILogger logger, IMapper mapper)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.logger = Guard.ThrowIfNull(logger);
        this.mapper = Guard.ThrowIfNull(mapper);
    }

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    public async Task<IActionResult> Index()
    {
        var playlists = await this.repository.GetAllByUserAsync(this.UserId, default);

        return this.View(playlists);
    }

    // GET: PlaylistController/Create
    public IActionResult Create()
    {
        return this.View();
    }

    // POST: PlaylistController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePlaylistRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var playlist = this.mapper.Map<Playlist>(request);
        playlist.OwnerId = this.UserId;

        try
        {
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .AddAsync(playlist, this.HttpContext.RequestAborted)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Playlist created successfully";

                return this.RedirectToAction(nameof(this.Index));
            }

            this.logger.Warning("The model is invalid. {@model}.", this.ModelState);

            return this.View(request);
        }
        catch (Exception ex)
        {
            this.TempData[Constants.Error] = "Something failed: " + ex.Message;
            this.logger.Error(ex, "Exception occurred.");

            return this.View(request);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        Guard.ThrowIfDefault(id);

        try
        {
            var playlist = await this.repository
                .GetByIdAsync(id.ToString(), this.HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return this.View(playlist);
        }
        catch (InvalidOperationException)
        {
            this.logger.ForContext("playlistId", id.ToString()).Warning("The playlist could not be found.");

            this.TempData[Constants.Error] = "The playlist could not be found.";
            return this.RedirectToAction(nameof(this.Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdatePlaylistRequest request)
    {
        Guard.ThrowIfDefault(id);
        Guard.ThrowIfNull(request);

        try
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(request);
            }

            var playlist = this.mapper.Map<Playlist>(request);
            playlist.Id = id.ToString();

            await this.repository
                .UpdateAsync(playlist, this.HttpContext.RequestAborted)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully updated playlist.";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (Exception ex)
        {
            this.TempData[Constants.Error] = ex.Message;
            return this.View(request);
        }
    }

    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var playlist = await this.repository
                .GetByIdAsync(id, this.HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return this.View(playlist);
        }
        catch (ApplicationExceptions.ItemNotFoundException ex)
        {
            this.logger.Warning(ex, "Playlist with Id \"{playlistId}\" could not be found.", id);

            this.TempData[Constants.Error] = "The link cannot be found.";
            this.TempData["StatusCode"] = 404;
            this.TempData["ReferenceId"] = this.HttpContext.TraceIdentifier;

            return this.RedirectToAction("Index", "Error");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await this.repository
                .RemoveAsync(id, this.HttpContext.RequestAborted)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Playlist successfully deleted.";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
        catch
        {
            return this.View();
        }
    }
}
