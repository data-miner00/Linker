namespace Linker.WebJob;

using Linker.Common.Helpers;
using Linker.Core.V2.Models;
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
        this.httpClient = Guard.ThrowIfNull(httpClient);
    }

    /// <inheritdoc/>
    public async Task<HealthCheckResult> PingAsync(string url, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await Console.Out.WriteLineAsync($"Pinging {url}");
            await this.httpClient
                .SendAsync(new HttpRequestMessage(HttpMethod.Head, url), cancellationToken)
                .ConfigureAwait(false);
            await Console.Out.WriteLineAsync($"Done pinging {url}");

            return new HealthCheckResult
            {
                Url = url,
                Status = UrlStatus.Alive,
                LastCheckedAt = DateTime.Now,
            };
        }
        catch (Exception ex)
        {
            return new HealthCheckResult
            {
                Url = url,
                Status = UrlStatus.Dead,
                LastCheckedAt = DateTime.Now,
                ErrorMessage = ex.Message,
                DeadAt = DateTime.Now,
            };
        }
    }
}
