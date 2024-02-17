namespace Linker.WebJob.Jobs;

using Quartz;
using System;
using System.Threading.Tasks;

internal class UrlHealthCheckJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        Console.WriteLine("Hello");

        return Task.CompletedTask;
    }
}
