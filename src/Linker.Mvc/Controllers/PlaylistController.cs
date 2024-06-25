namespace Linker.Mvc.Controllers;

using Linker.Common.Helpers;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

public sealed class PlaylistController : Controller
{
    private readonly IPlaylistRepository repository;
    private readonly ILogger logger;

    public PlaylistController(IPlaylistRepository repository, ILogger logger)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.logger = Guard.ThrowIfNull(logger);
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

        var playlist = new Playlist
        {
            Id = Guid.NewGuid().ToString(),
            OwnerId = this.UserId,
            Name = request.Name,
            Description = request.Description,
            Visibility = request.Visibility,
        };

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
}
