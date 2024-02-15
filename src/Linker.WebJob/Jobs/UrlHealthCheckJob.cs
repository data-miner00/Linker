namespace Linker.WebJob.Jobs;

using Quartz;
using System;
using System.Threading.Tasks;

internal class UrlHealthCheckJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Hello");

        return Task.CompletedTask;
    }
}
