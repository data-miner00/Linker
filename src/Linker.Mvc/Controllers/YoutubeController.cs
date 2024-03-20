namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public sealed class YoutubeController : Controller
{
    private readonly IYoutubeRepository repository;
    private readonly IMapper mapper;

    public YoutubeController(IYoutubeRepository repository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.repository = repository;
        this.mapper = mapper;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    // GET: YoutubeController
    public async Task<IActionResult> Index()
    {
        var youtubes = await this.repository
            .GetAllAsync(this.CancellationToken)
            .ConfigureAwait(false);

        return this.View(youtubes);
    }

    // GET: YoutubeController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var youtube = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(youtube);
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

    // GET: YoutubeController/Create
    public IActionResult Create()
    {
        return this.View();
    }

    // POST: YoutubeController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateYoutubeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var youtube = this.mapper.Map<Youtube>(request);
        youtube.CreatedBy = this.UserId;

        try
        {
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .AddAsync(youtube, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Youtube created successfully!";

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(request);
        }
        catch
        {
            this.TempData[Constants.Error] = "Something failed.";

            return this.View(request);
        }
    }

    // GET: YoutubeController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var youtube = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(youtube);
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

    // POST: YoutubeController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateYoutubeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var youtube = this.mapper.Map<Youtube>(request);
            youtube.Id = id.ToString();

            await this.repository
                .UpdateAsync(youtube, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully updated youtube.";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException)
        {
            return this.NotFound(request);
        }
        catch
        {
            return this.View();
        }
    }

    // POST: YoutubeController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Youtube deleted successfully.";

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
