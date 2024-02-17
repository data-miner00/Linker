namespace Linker.WebJob;

using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

internal class WebJobService : BackgroundService
{
    private readonly JobScheduler jobScheduler;

    public WebJobService(JobScheduler jobScheduler)
    {
        this.jobScheduler = jobScheduler;
    }

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
