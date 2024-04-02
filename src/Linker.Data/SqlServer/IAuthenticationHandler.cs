namespace Linker.Data.SqlServer;

using System.Threading;
using System.Threading.Tasks;
using Linker.Core.V2.Models;

/// <summary>
/// The handler that contains the authentication logic.
/// </summary>
public interface IAuthenticationHandler
{
    /// <summary>
    /// Change the password for a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="oldPassword">The old password.</param>
    /// <param name="newPassword">The new password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A flag indicating the success of the operation.</returns>
    Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken);

    /// <summary>
    /// Validates a user login against the <see cref="Credential"/> record.
    /// </summary>
    /// <param name="userId">The user to be logged in.</param>
    /// <param name="password">The password for the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A flag indicating the success of the operation.</returns>
    Task<bool> LoginAsync(string userId, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Register a user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="password">The password of the user.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RegisterAsync(string userId, string password, CancellationToken cancellationToken);
}
