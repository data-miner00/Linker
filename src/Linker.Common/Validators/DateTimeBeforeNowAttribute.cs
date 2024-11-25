namespace Linker.Common.Validators;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Validates if a <see cref="DateTime"/> is less than now.
/// </summary>
public sealed class DateTimeBeforeNowAttribute : ValidationAttribute
{
    /// <inheritdoc/>
    public override string FormatErrorMessage(string name)
    {
        return "Date value should not be a date in the future.";
    }

    /// <inheritdoc/>
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is DateTime dateValue && dateValue.Date >= DateTime.Now)
        {
           return new ValidationResult(this.FormatErrorMessage(string.Empty));
        }

        return ValidationResult.Success;
    }
}
