namespace Linker.WebJob.Models;

using Quartz;

/// <summary>
/// The internal model that describes a job with its metadata.
/// </summary>
internal sealed class JobDescriptor
{
    /// <summary>
    /// Gets or sets the concrete type of an <see cref="IJob"/>.
    /// </summary>
    required public Type JobType { get; set; }

    /// <summary>
    /// Gets or sets the text description of the job.
    /// </summary>
    required public string Description { get; set; }

    /// <summary>
    /// Gets or sets the cron expression of the job.
    /// </summary>
    required public string CronExpression { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to enable the job or not.
    /// </summary>
    public bool Enabled { get; set; }
}
