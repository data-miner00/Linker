namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public sealed class LinkController : Controller
{
    private readonly ILinkRepository repository;
    private readonly IMapper mapper;

    public LinkController(ILinkRepository repository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.repository = repository;
        this.mapper = mapper;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    // GET: LinkController
    public async Task<IActionResult> Index()
    {
        var links = await this.repository
            .GetAllAsync(this.CancellationToken)
            .ConfigureAwait(false);

        return View(links);
    }

    // GET: LinkController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var link = await this.repository
            .GetByIdAsync(id, this.CancellationToken)
            .ConfigureAwait(false);

        return View(link);
    }

    // GET: LinkController/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: LinkController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateLinkRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var link = this.mapper.Map<Link>(request);
        link.AddedBy = this.UserId;

        try
        {
            // TODO: For fun, remove soon.
            if (request.Url == "s")
            {
                this.ModelState.AddModelError("Error", "I hate you");
            }

            if (this.ModelState.IsValid)
            {
                await this.repository
                    .AddAsync(link, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Link created successfully!";

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

    // GET: LinkController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var link = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(link);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    // POST: LinkController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateLinkRequest request)
    {
        try
        {
            var link = this.mapper.Map<Link>(request);
            link.Id = id.ToString();

            await this.repository
                .UpdateAsync(link, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully updated link.";

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

    // POST: LinkController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Link deleted successfully.";

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
