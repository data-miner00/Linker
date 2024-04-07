namespace Linker.WebJob.Jobs;

using Linker.Core.V2.Repositories;
using Linker.WebJob.Options;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Checks for the health of the URLs in the database.
/// </summary>
[DisallowConcurrentExecution]
internal class UrlHealthCheckJob : IJob
{
    private const string LogTemplate = "{0:00}:{1:00}:{2:00}.{3:00}";

    private readonly IUrlHealthChecker checker;
    private readonly ILinkRepository linkRepository;
    private readonly IHealthCheckRepository healthCheckRepository;
    private readonly UrlHealthCheckOption option;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlHealthCheckJob"/> class.
    /// </summary>
    /// <param name="checker">The URL health checker.</param>
    /// <param name="linkRepository">The link repository.</param>
    /// <param name="healthCheckRepository">The health check result repository.</param>
    /// <param name="option">The settings for the job.</param>
    public UrlHealthCheckJob(
        IUrlHealthChecker checker,
        ILinkRepository linkRepository,
        IHealthCheckRepository healthCheckRepository,
        UrlHealthCheckOption option)
    {
        this.checker = checker;
        this.linkRepository = linkRepository;
        this.healthCheckRepository = healthCheckRepository;
        this.option = option;
    }

    /// <inheritdoc/>
    public async Task Execute(IJobExecutionContext context)
    {
        var timeout = TimeSpan.FromSeconds(this.option.TimeoutInSeconds);

        try
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            using var timeoutCancellationToken = new CancellationTokenSource(timeout);
            using var linkedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
                timeoutCancellationToken.Token,
                context.CancellationToken);

            Stopwatch stopwatch = Stopwatch.StartNew();
            await Console.Out.WriteLineAsync("Starting execution");

            var allUrls = await this
                .GetAllUrlsAsync(linkedCancellationToken.Token)
                .ConfigureAwait(false);

            var pingTasks = allUrls.Select(url => this.checker.PingAsync(url, linkedCancellationToken.Token));

            await Console.Out.WriteLineAsync("Pinging started");
            var healthCheckResults = await Task.WhenAll(pingTasks);
            await Console.Out.WriteLineAsync("Pinging done");

            var upsertTasks = healthCheckResults.Select(result => this.healthCheckRepository.UpsertAsync(result, default));
            await Console.Out.WriteLineAsync("Upsert started");
            await Task.WhenAll(upsertTasks);
            await Console.Out.WriteLineAsync("Upsert done");
            stopwatch.Stop();

            var ts = stopwatch.Elapsed;
            var elapsedTime = string.Format(LogTemplate, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task<IEnumerable<string>> GetAllUrlsAsync(CancellationToken cancellationToken)
    {
        var articleUrls = await this.linkRepository
            .GetAllAsync(cancellationToken)
            .ConfigureAwait(false);

        return articleUrls.Select(x => x.Url).ToList();
    }
}
