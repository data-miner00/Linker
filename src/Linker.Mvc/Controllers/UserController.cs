namespace Linker.Mvc.Controllers;

using Linker.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public sealed class UserController : Controller
{
    private readonly IUserRepository repository;
    private readonly IWorkspaceRepository workspaceRepository;

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    public UserController(IUserRepository repository, IWorkspaceRepository workspaceRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(workspaceRepository);
        this.repository = repository;
        this.workspaceRepository = workspaceRepository;
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

            return this.View((user, joinedWorkspaces));
        }
        catch (InvalidOperationException)
        {
            return this.NotFound();
        }
    }
}
