namespace Linker.Core.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Linker.Core.ApiModels;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The abstraction for authentication controller.
    /// </summary>
    public interface IAuthController
    {
        /// <summary>
        /// Login as a user.
        /// </summary>
        /// <param name="request">The request containing login credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The action result.</returns>
        Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Register a user.
        /// </summary>
        /// <param name="request">The request containing info for user creation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The action result.</returns>
        Task<IActionResult> RegisterAsync(CreateUserRequest request, CancellationToken cancellationToken);
    }
}
