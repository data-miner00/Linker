namespace Linker.WebJob.Filters;

using System;
using System.ComponentModel.DataAnnotations;
using Quartz;

/// <summary>
/// Validates the validity of a cron expression.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
#pragma warning disable SA1206 // Declaration keywords should follow order
sealed public class CronExpressionAttribute : ValidationAttribute
{
    /// <inheritdoc/>
    override public bool IsValid(object? value)
#pragma warning restore SA1206 // Declaration keywords should follow order
    {
        if (value is string strVal)
        {
            return CronExpression.IsValidExpression(strVal);
        }

        return false;
    }
}
