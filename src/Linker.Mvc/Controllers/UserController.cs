namespace Linker.Mvc.Controllers;

using Linker.Core.Repositories;
using Linker.Core.V2;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public sealed class UserController : Controller
{
    private readonly IUserRepository repository;
    private readonly IAssetUploader assetUploader;
    private readonly IWorkspaceRepository workspaceRepository;

    public CancellationToken CancellationToken => this.HttpContext.RequestAborted;

    public string UserId =>
        this.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    public UserController(
        IUserRepository repository,
        IAssetUploader assetUploader,
        IWorkspaceRepository workspaceRepository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(assetUploader);
        ArgumentNullException.ThrowIfNull(workspaceRepository);
        this.repository = repository;
        this.assetUploader = assetUploader;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        try
        {
            var uploadResult = await this.assetUploader
                .UploadAsync(file)
                .ConfigureAwait(false);

            var user = await this.repository
                .GetByIdAsync(this.UserId, this.CancellationToken)
                .ConfigureAwait(false);

            user.PhotoUrl = uploadResult.FullPath;

            await this.repository
                .UpdateAsync(user, this.CancellationToken)
                .ConfigureAwait(false);

            this.TempData[Constants.Success] = "something wrong";
            return this.RedirectToAction(nameof(this.Index));
        }
        catch (Exception ex)
        {
            this.TempData[Constants.Error] = ex.Message;
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
