namespace Linker.WebJob;

using EnsureThat;
using Linker.Core.Models;
using System;
using System.Threading.Tasks;

/// <summary>
/// The url health checker implementation.
/// </summary>
public sealed class UrlHealthChecker : IUrlHealthChecker
{
    private readonly HttpClient httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlHealthChecker"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public UrlHealthChecker(HttpClient httpClient)
    {
        this.httpClient = EnsureArg.IsNotNull(httpClient, nameof(httpClient));
    }

    /// <inheritdoc/>
    public async Task<HealthCheckResult> PingAsync(string url, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await this.httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken)
                .ConfigureAwait(false);

            return new HealthCheckResult
            {
                Url = url,
                Status = UrlStatus.Alive,
                LastChecked = DateTime.Now,
            };
        }
        catch (HttpRequestException ex)
        {
            return new HealthCheckResult
            {
                Url = url,
                Status = UrlStatus.Dead,
                LastChecked = DateTime.Now,
                ErrorMessage = ex.Message,
                DeadAt = DateTime.Now,
            };
        }
    }
}
