namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Authorize]
public sealed class WebsiteController : Controller
{
    private readonly IWebsiteRepository repository;
    private readonly IMapper mapper;

    public WebsiteController(IWebsiteRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    // GET: WebsiteController
    public async Task<IActionResult> Index()
    {
        var websites = await this.repository
            .GetAllAsync(this.CancellationToken)
            .ConfigureAwait(false);

        return this.View(websites);
    }

    // GET: WebsiteController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var website = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(website);
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

    // GET: WebsiteController/Create
    public IActionResult Create()
    {
        return this.View();
    }

    // POST: WebsiteController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateWebsiteRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var website = this.mapper.Map<Website>(request);

        try
        {
            if (this.ModelState.IsValid)
            {
                await this.repository
                    .AddAsync(website, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Website added successfully.";

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(request);
        }
        catch
        {
            this.TempData[Constants.Error] = "Create website failed.";

            return this.View(request);
        }
    }

    // GET: WebsiteController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var website = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(website);
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

    // POST: WebsiteController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateWebsiteRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var website = this.mapper.Map<Website>(request);
        website.Id = id.ToString();

        try
        {
            await this.repository
                .UpdateAsync(website, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Website updated successfully.";

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

    // POST: WebsiteController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Website deleted successfully.";

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
