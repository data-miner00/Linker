﻿namespace Linker.Mvc.Controllers;

using AutoMapper;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public sealed class ArticleController : Controller
{
    private readonly IArticleRepository repository;
    private readonly IMapper mapper;

    public ArticleController(IArticleRepository repository, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(mapper);
        this.repository = repository;
        this.mapper = mapper;
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    // GET: ArticleController
    public async Task<IActionResult> Index()
    {
        var articles = await this.repository
            .GetAllAsync(this.CancellationToken)
            .ConfigureAwait(false);

        return View(articles);
    }

    // GET: ArticleController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var article = await this.repository
            .GetByIdAsync(id, this.CancellationToken)
            .ConfigureAwait(false);

        return View(article);
    }

    // GET: ArticleController/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ArticleController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateArticleRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var article = this.mapper.Map<Article>(request);
        article.CreatedBy = this.UserId;

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
                    .AddAsync(article, this.CancellationToken)
                    .ConfigureAwait(false);

                this.TempData[Constants.Success] = "Article created successfully!";

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

    // GET: ArticleController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var article = await this.repository
                .GetByIdAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            return this.View(article);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }

    // POST: ArticleController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateArticleRequest request)
    {
        try
        {
            var article = this.mapper.Map<Article>(request);
            article.Id = id.ToString();

            await this.repository
                .UpdateAsync(article, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Successfully updated article.";

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

    // POST: ArticleController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await this.repository
                .RemoveAsync(id.ToString(), this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "Article deleted successfully.";

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
