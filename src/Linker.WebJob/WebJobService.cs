namespace Linker.WebJob;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

internal class WebJobService : ServiceBase
{
    private readonly JobScheduler jobScheduler;

    public WebJobService(JobScheduler jobScheduler)
    {
        this.jobScheduler = jobScheduler;
    }

    protected override void OnStart(string[] args)
    {
        try
        {
            this.jobScheduler
                .StartAsync()
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected override void OnShutdown()
    {
        try
        {
            this.jobScheduler
                .StopAsync()
                .GetAwaiter()
                .GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
