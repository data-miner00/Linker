namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Common.Helpers;
using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Clients;
using Linker.Core.V2.Exceptions;
using Linker.Core.V2.Models;
using Linker.Core.V2.QueryParams;
using Linker.Core.V2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Security.Claims;

[Authorize]
public sealed class LinkController : Controller
{
    private readonly ILinkRepository repository;
    private readonly IMapper mapper;
    private readonly ILogger logger;
    private readonly ILinkUpdatedEventClient eventClient;

    public LinkController(
        ILinkRepository repository,
        IMapper mapper,
        ILogger logger,
        ILinkUpdatedEventClient eventClient)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.mapper = Guard.ThrowIfNull(mapper);
        this.logger = Guard.ThrowIfNull(logger);
        this.eventClient = Guard.ThrowIfNull(eventClient);
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    // GET: LinkController
    public async Task<IActionResult> Index(GetLinksQueryParams query)
    {
        var links = await this.repository
            .GetAllAsync(query, this.CancellationToken)
            .ConfigureAwait(false);

        var publicLinks = links.Where(link => link.Visibility == Visibility.Public);

        return this.View((publicLinks, LinkType.None));
    }

    public async Task<IActionResult> Search(string q)
    {
        var links = await this.repository
            .SearchAsync(q, this.CancellationToken)
            .ConfigureAwait(false);

        var publicLinks = links.Where(link => link.Visibility == Visibility.Public);

        return this.View(publicLinks);
    }

    // GET: LinkController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        try
        {
            var link = await this.repository
                .GetByIdAsync(id, this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(link);
        }
        catch (ApplicationExceptions.ItemNotFoundException ex)
        {
            this.logger.Warning(ex, "Link with Id \"{linkId}\" could not be found.", id);

            this.TempData[Constants.Error] = "The link cannot be found.";
            this.TempData["StatusCode"] = 404;
            this.TempData["ReferenceId"] = this.HttpContext.TraceIdentifier;

            return this.RedirectToAction("Index", "Error");
        }
    }

    // GET: LinkController/Create
    public IActionResult Create()
    {
        return this.View();
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
                var linkId = await this.repository
                    .AddAsync(link, this.CancellationToken)
                    .ConfigureAwait(false);

                await this.eventClient.PublishAsync(linkId, default);

                this.TempData[Constants.Success] = "Link created successfully!";

                return this.RedirectToAction(nameof(this.Index));
            }

            this.logger.Warning("The model is invalid. {@model}.", this.ModelState);

            return this.View(request);
        }
        catch (Exception ex)
        {
            this.TempData[Constants.Error] = "Something failed: " + ex.Message;

            return this.View(request);
        }
    }

    // GET: LinkController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        Guard.ThrowIfDefault(id);

        try
        {
            var link = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            await this.eventClient.PublishAsync(id.ToString(), default);

            return this.View(link);
        }
        catch (InvalidOperationException)
        {
            this.logger.Warning("The link with '{id}' could not be found.", id);

            return this.NotFound(); // Change this
        }
    }

    // POST: LinkController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateLinkRequest request)
    {
        Guard.ThrowIfDefault(id);
        Guard.ThrowIfNull(request);

        if (!this.ModelState.IsValid)
        {
            this.logger.Warning("The model is invalid. {@model}.", this.ModelState);

            return this.View(request);
        }

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
            return this.NotFound(); // update this
        }
    }

    // POST: LinkController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        Guard.ThrowIfDefault(id);

        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Link deleted successfully.";

            return this.RedirectToAction(nameof(this.Index));
        }
        catch (InvalidOperationException ex)
        {
            this.logger.Warning(ex, "Possibly item not found. Link Id: {LinkId}", id.ToString());

            return this.NotFound();
        }
        catch (Exception ex)
        {
            this.logger.Error(ex, "General exception caught.");

            return this.View();
        }
    }

    public async Task<IActionResult> Articles()
    {
        var type = LinkType.Article;

        try
        {
            var articles = await this.repository
                .GetAllByLinkTypeAsync(type, this.CancellationToken)
                .ConfigureAwait(false);

            var publicArticles = articles.Where(link => link.Visibility == Visibility.Public);

            return this.View("Index", (publicArticles, type));
        }
        catch
        {
            return this.NotFound(); // change this
        }
    }

    public async Task<IActionResult> Websites()
    {
        var type = LinkType.Website;

        try
        {
            var websites = await this.repository
                .GetAllByLinkTypeAsync(type, this.CancellationToken)
                .ConfigureAwait(false);

            var publicWebsites = websites.Where(link => link.Visibility == Visibility.Public);

            return this.View("Index", (publicWebsites, type));
        }
        catch
        {
            return this.NotFound(); // change this
        }
    }

    public async Task<IActionResult> Youtube()
    {
        var type = LinkType.Youtube;

        try
        {
            var websites = await this.repository
                .GetAllByLinkTypeAsync(type, this.CancellationToken)
                .ConfigureAwait(false);

            var publicWebsites = websites.Where(link => link.Visibility == Visibility.Public);

            return this.View("Index", (publicWebsites, type));
        }
        catch
        {
            return this.NotFound(); // change this
        }
    }
}
