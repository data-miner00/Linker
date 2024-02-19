namespace Linker.WebJob;

using Autofac;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class JobFactory : IJobFactory
{
    private readonly IComponentContext contextAccessor;

    public JobFactory(IComponentContext contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        return this.contextAccessor.Resolve(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
