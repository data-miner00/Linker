namespace Linker.Mvc.Controllers;

using Linker.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Serilog;
using AutoMapper;
using Linker.Common.Helpers;
using Linker.Core.V2.Repositories;

/// <summary>
/// The controller for the home page.
/// </summary>
[Authorize]
public sealed class HomeController : Controller
{
    private readonly IMapper mapper;
    private readonly ILogger logger;
    private readonly ILinkRepository linkRepository;
    private readonly IUserRepository userRepository;
    private readonly ITagRepository tagRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="mapper">The mapper.</param>
    public HomeController(
        ILogger logger,
        IMapper mapper,
        ILinkRepository linkRepository,
        IUserRepository userRepository,
        ITagRepository tagRepository)
    {
        this.logger = Guard.ThrowIfNull(logger);
        this.mapper = Guard.ThrowIfNull(mapper);
        this.linkRepository = Guard.ThrowIfNull(linkRepository);
        this.userRepository = Guard.ThrowIfNull(userRepository);
        this.tagRepository = Guard.ThrowIfNull(tagRepository);
    }

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    /// <summary>
    /// The home page.
    /// </summary>
    /// <returns>The page.</returns>
    public async Task<IActionResult> Index()
    {
        var links = await this.linkRepository.GetAllAsync(this.CancellationToken);
        var tags = await this.tagRepository.GetAllAsync();
        var users = await this.userRepository.GetAllAsync(this.CancellationToken);

        var viewModel = new HomeViewModel
        {
            TrendingTags = tags.Select(tag => tag.Name),
            LatestLinks = links.Take(20),
            TrendingLinks = links.Skip(20),
            Users = users,
        };

        return this.View(viewModel);
    }

    /// <summary>
    /// The privacy page.
    /// </summary>
    /// <returns>The page.</returns>
    public IActionResult Privacy()
    {
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var viewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
        };

        this.logger.Warning("Error page called. Request Id: {RequestId}", viewModel.RequestId);

        return this.View(viewModel);
    }

    public IActionResult NotFound()
    {
        this.logger.Information("Not found page called.");

        return this.View();
    }
}
