namespace Linker.WebJob;

using Linker.Core.V2.Models;

/// <summary>
/// The interface for Url health checker.
/// </summary>
public interface IUrlHealthChecker
{
    /// <summary>
    /// Pings a website to check for its status.
    /// </summary>
    /// <param name="url">The url of the website.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The health check result.</returns>
    public Task<HealthCheckResult> PingAsync(string url, CancellationToken cancellationToken);
}
