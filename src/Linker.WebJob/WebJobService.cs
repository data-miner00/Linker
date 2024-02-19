namespace Linker.WebJob;

using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The long running web job service.
/// </summary>
internal class WebJobService : BackgroundService
{
    private readonly JobScheduler jobScheduler;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebJobService"/> class.
    /// </summary>
    /// <param name="jobScheduler">The custom job scheduler.</param>
    public WebJobService(JobScheduler jobScheduler)
    {
        this.jobScheduler = jobScheduler;
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        try
        {
            await this.jobScheduler
                .StartAsync(stoppingToken)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
