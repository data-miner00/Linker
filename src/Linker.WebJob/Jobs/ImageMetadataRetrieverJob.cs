namespace Linker.WebJob.Jobs;

using Linker.Common.Helpers;
using Linker.Core.V2.Clients;
using Linker.WebJob.Options;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Try to retrieve the OpenGraph image thumbnail of a web url and update accordingly.
/// </summary>
[DisallowConcurrentExecution]
internal class ImageMetadataRetrieverJob : IJob
{
    private const string LogTemplate = "{0:00}:{1:00}:{2:00}.{3:00}";

    private readonly ILinkUpdatedEventClient client;
    private readonly ImageMetadataRetrieverOption option;
    private readonly IImageMetadataHandler handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageMetadataRetrieverJob"/> class.
    /// </summary>
    /// <param name="client">The link updated event client.</param>
    /// <param name="option">The job option.</param>
    /// <param name="handler">The job handler.</param>
    public ImageMetadataRetrieverJob(
        ILinkUpdatedEventClient client,
        ImageMetadataRetrieverOption option,
        IImageMetadataHandler handler)
    {
        this.client = Guard.ThrowIfNull(client);
        this.option = Guard.ThrowIfNull(option);
        this.handler = Guard.ThrowIfNull(handler);
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
            await Console.Out.WriteLineAsync($"Starting {nameof(ImageMetadataRetrieverJob)}");

            var @event = await this.client.PeekAsync(linkedCancellationToken.Token);

            await this.handler.HandleAsync(@event.LinkId.ToString(), default);

            await this.client.CompleteAsync(@event.Id, default);

            stopwatch.Stop();

            await Console.Out.WriteLineAsync($"Done {nameof(ImageMetadataRetrieverJob)}");
            var ts = stopwatch.Elapsed;
            var elapsedTime = string.Format(LogTemplate, ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
