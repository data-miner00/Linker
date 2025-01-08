namespace Linker.WebJob.Jobs;

using Linker.Core.V2.Clients;
using Linker.WebJob.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[DisallowConcurrentExecution]
internal class ImageMetadataRetrieverJob : IJob
{
    private readonly ILinkUpdatedEventClient client;
    private readonly ImageMetadataRetrieverOption option;

    public ImageMetadataRetrieverJob(
        ILinkUpdatedEventClient client,
        ImageMetadataRetrieverOption option)
    {
        this.client = client;
        this.option = option;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var timeout = TimeSpan.FromSeconds(this.option.TimeoutInSeconds);

        try
        {
            var @event = await this.client.PeekAsync(default);

            Console.WriteLine(@event.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
