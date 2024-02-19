namespace Linker.WebJob;

using Linker.WebJob.Models;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class JobScheduler
{
    private readonly IEnumerable<JobDescriptor> jobDescriptors;
    private readonly IJobFactory jobFactory;
    private readonly IScheduler scheduler;

    public JobScheduler(IScheduler scheduler, IEnumerable<JobDescriptor> jobDescriptors, IJobFactory jobFactory)
    {
        this.scheduler = scheduler;
        this.jobDescriptors = jobDescriptors;
        this.jobFactory = jobFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.scheduler.JobFactory = this.jobFactory;
        return Task.WhenAll(this.scheduler.Start(cancellationToken), this.ScheduleJobsAsync(cancellationToken));
    }

    public Task StopAsync()
    {
        return this.scheduler.Shutdown();
    }

    private async Task ScheduleJobsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var jobDescriptor in this.jobDescriptors)
        {
            var job = JobBuilder.Create()
                .OfType(jobDescriptor.JobType)
                .WithIdentity(jobDescriptor.JobType.Name)
                .WithDescription(jobDescriptor.Description)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(jobDescriptor.JobType.Name + "trigger")
                .WithDescription($"The trigger for {jobDescriptor.JobType.Name} job")
                .WithCronSchedule(jobDescriptor.CronExpression)
                .Build();

            await this.scheduler.ScheduleJob(job, trigger, cancellationToken).ConfigureAwait(false);
        }
    }
}
