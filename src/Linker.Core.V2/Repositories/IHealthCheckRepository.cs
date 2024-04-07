namespace Linker.Core.V2.Repositories;

using Linker.Core.V2.Models;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The abstraction for URL health check repository.
/// </summary>
public interface IHealthCheckRepository
{
    /// <summary>
    /// Gets a health check result by URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The health check result.</returns>
    Task<HealthCheckResult> GetByUrlAsync(string url, CancellationToken cancellationToken);

    /// <summary>
    /// Inserts a health check result if not exist. Update if exist.
    /// </summary>
    /// <param name="result">The health check result.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task.</returns>
    Task UpsertAsync(HealthCheckResult result, CancellationToken cancellationToken);
}
