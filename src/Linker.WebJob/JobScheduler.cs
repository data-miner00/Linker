namespace Linker.WebJob;

using Linker.WebJob.Models;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class JobScheduler
{
    private readonly IEnumerable<JobDescriptor> jobDescriptors;
    private readonly IScheduler scheduler;

    public JobScheduler(IScheduler scheduler, IEnumerable<JobDescriptor> jobDescriptors)
    {
        this.scheduler = scheduler;
        this.jobDescriptors = jobDescriptors;
    }

    public Task StartAsync()
    {
        return Task.WhenAll(this.scheduler.Start(), this.ScheduleJobsAsync());
    }

    public Task StopAsync()
    {
        return this.scheduler.Shutdown();
    }

    private async Task ScheduleJobsAsync()
    {
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

            await this.scheduler.ScheduleJob(job, trigger).ConfigureAwait(false);
        }
    }
}
