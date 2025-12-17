namespace Linker.WebJob.Options;

using Linker.WebJob.Filters;
using Linker.WebJob.Jobs;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The settings for the <see cref="UrlHealthCheckJob"/>.
/// </summary>
internal sealed class UrlHealthCheckOption
{
    /// <summary>
    /// Gets or sets the cron expression for the job.
    /// </summary>
    [Required]
    [CronExpression]
    required public string CronExpression { get; set; }

    /// <summary>
    /// Gets or sets the maximum timeout in seconds.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    required public int TimeoutInSeconds { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this job should be enabled or not.
    /// </summary>
    public bool Enabled { get; set; }
}
