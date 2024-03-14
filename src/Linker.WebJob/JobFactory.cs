namespace Linker.WebJob;

using Autofac;
using Quartz;
using Quartz.Spi;
using System;

/// <summary>
/// Custom DI aware job provider for Quartz.
/// </summary>
internal sealed class JobFactory : IJobFactory
{
    private readonly IComponentContext contextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobFactory"/> class.
    /// </summary>
    /// <param name="contextAccessor">The DI context accessor.</param>
    public JobFactory(IComponentContext contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    /// <inheritdoc/>
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var resolvedJob = this.contextAccessor.Resolve(bundle.JobDetail.JobType) as IJob;
        ArgumentNullException.ThrowIfNull(resolvedJob);

        return resolvedJob;
    }

    /// <inheritdoc/>
    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
