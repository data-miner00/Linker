namespace Linker.Mvc.Controllers;

using Linker.Core.ApiModels;
using Linker.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

public class ArticleController : Controller
{
    private readonly IArticleRepository repository;

    public ArticleController(IArticleRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        this.repository = repository;
    }

    // GET: ArticleController
    public async Task<IActionResult> Index()
    {
        var articles = await this.repository
            .GetAllAsync(default)
            .ConfigureAwait(false);

        return View(articles);
    }

    // GET: ArticleController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var article = await this.repository
            .GetByIdAsync(id, default)
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
    public IActionResult Create(CreateArticleRequest request)
    {
        try
        {
            if (request.Url == "s")
            {
                this.ModelState.AddModelError("Error", "I hate you");
            }

            if (this.ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return this.View(request);
        }
        catch
        {
            return View(request);
        }
    }

    // GET: ArticleController/Edit/5
    public IActionResult Edit(int id)
    {
        return View();
    }

    // POST: ArticleController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: ArticleController/Delete/5
    public IActionResult Delete(int id)
    {
        return View();
    }

    // POST: ArticleController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
