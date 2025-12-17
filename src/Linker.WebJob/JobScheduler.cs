namespace Linker.WebJob;

using Linker.WebJob.Models;
using Quartz;
using Quartz.Spi;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Schedules all jobs and starts the scheduler.
/// </summary>
internal sealed class JobScheduler
{
    private readonly IEnumerable<JobDescriptor> jobDescriptors;
    private readonly IJobFactory jobFactory;
    private readonly IScheduler scheduler;

    /// <summary>
    /// Initializes a new instance of the <see cref="JobScheduler"/> class.
    /// </summary>
    /// <param name="scheduler">The actual scheduler from Quartz.</param>
    /// <param name="jobDescriptors">The list of job descriptors.</param>
    /// <param name="jobFactory">The custom IoC-aware job factory.</param>
    public JobScheduler(IScheduler scheduler, IEnumerable<JobDescriptor> jobDescriptors, IJobFactory jobFactory)
    {
        this.scheduler = scheduler;
        this.jobDescriptors = jobDescriptors;
        this.jobFactory = jobFactory;
    }

    /// <summary>
    /// Starts the scheduler and schedule registered jobs.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The task itself.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        this.scheduler.JobFactory = this.jobFactory;
        return Task.WhenAll(this.scheduler.Start(cancellationToken), this.ScheduleJobsAsync(cancellationToken));
    }

    /// <summary>
    /// Stops the running scheduler.
    /// </summary>
    /// <returns>The task.</returns>
    public Task StopAsync()
    {
        return this.scheduler.Shutdown();
    }

    private Task<DateTimeOffset[]> ScheduleJobsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var jobSchedulingTasks = this.jobDescriptors
            .Where(jobDescriptor => jobDescriptor.Enabled)
            .Select(jobDescriptor =>
            {
                Console.WriteLine($"Registering Job: {jobDescriptor.JobType.Name}");

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

                return this.scheduler.ScheduleJob(job, trigger, cancellationToken);
            });

        return Task.WhenAll(jobSchedulingTasks);
    }
}
