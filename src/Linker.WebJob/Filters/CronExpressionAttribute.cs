namespace Linker.WebJob.Filters;

using System;
using System.ComponentModel.DataAnnotations;
using Quartz;

/// <summary>
/// Validates the validity of a cron expression.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class CronExpressionAttribute : ValidationAttribute
{
    /// <inheritdoc/>
    override public bool IsValid(object? value)
    {
        if (value is string strVal)
        {
            return CronExpression.IsValidExpression(strVal);
        }

        return false;
    }
}
