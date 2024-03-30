namespace Linker.Core.V2.Repositories;

using Linker.Core.V2.Models;
using System.Threading.Tasks;

/// <summary>
/// The abstraction for <see cref="Credential"/> repository.
/// </summary>
public interface ICredentialRepository
{
    /// <summary>
    /// Creates a new credential record.
    /// </summary>
    /// <param name="credential">The credential to be created.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task AddAsync(Credential credential, CancellationToken cancellationToken);

    /// <summary>
    /// Removes a credential record by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task RemoveAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the credential.
    /// </summary>
    /// <param name="credential">The credentials to be updated.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task UpdateAsync(Credential credential, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a credential record.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The credential found.</returns>
    Task<Credential> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
}
