namespace Linker.WebJob.Jobs;

using Linker.Core.Repositories;
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
    private readonly IUrlHealthChecker checker;
    private readonly IArticleRepository articleRepository;
    private readonly IWebsiteRepository websiteRepository;
    private readonly IYoutubeRepository youtubeRepository;
    private readonly IHealthCheckRepository healthCheckRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UrlHealthCheckJob"/> class.
    /// </summary>
    /// <param name="checker">The URL health checker.</param>
    /// <param name="articleRepository">The article repository.</param>
    /// <param name="websiteRepository">The website repository.</param>
    /// <param name="youtubeRepository">The youtube repository.</param>
    /// <param name="healthCheckRepository">The health check result repository.</param>
    public UrlHealthCheckJob(
        IUrlHealthChecker checker,
        IArticleRepository articleRepository,
        IWebsiteRepository websiteRepository,
        IYoutubeRepository youtubeRepository,
        IHealthCheckRepository healthCheckRepository)
    {
        this.checker = checker;
        this.articleRepository = articleRepository;
        this.websiteRepository = websiteRepository;
        this.youtubeRepository = youtubeRepository;
        this.healthCheckRepository = healthCheckRepository;
    }

    /// <inheritdoc/>
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            Stopwatch stopwatch = Stopwatch.StartNew();
            await Console.Out.WriteLineAsync("Starting execution");

            var articleUrls = await this.articleRepository
                .GetAllAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var websiteUrls = await this.websiteRepository
                .GetAllAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var youtubeUrls = await this.youtubeRepository
                .GetAllAsync(context.CancellationToken)
                .ConfigureAwait(false);

            var allUrls = new List<string>();

            allUrls.AddRange(articleUrls.Select(x => x.Url));
            allUrls.AddRange(websiteUrls.Select(x => x.Url));
            allUrls.AddRange(youtubeUrls.Select(x => x.Url));

            var pingTasks = allUrls.Select(url => this.checker.PingAsync(url, context.CancellationToken));

            await Console.Out.WriteLineAsync("Pinging started");
            var healthCheckResults = await Task.WhenAll(pingTasks);
            await Console.Out.WriteLineAsync("Pinging done");

            var upsertTasks = healthCheckResults.Select(result => this.healthCheckRepository.UpsertAsync(result, default));
            await Console.Out.WriteLineAsync("Upsert started");
            await Task.WhenAll(upsertTasks);
            await Console.Out.WriteLineAsync("Upsert done");
            stopwatch.Stop();

            var ts = stopwatch.Elapsed;
            string elapsedTime = string.Format(
                "{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
