namespace Linker.WebJob.Options;

using Linker.WebJob.Filters;
using System;
using System.ComponentModel.DataAnnotations;

internal sealed class ImageMetadataRetrieverOption
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
