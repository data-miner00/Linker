namespace Linker.Common.Validators;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Validates if a <see cref="DateTime"/> is less than the specified ones.
/// Invalid parameter ...
/// </summary>
public sealed class DateTimeLessThanAttribute : ValidationAttribute
{
    private readonly DateTime dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeLessThanAttribute"/> class.
    /// </summary>
    /// <param name="dateTime">The target value.</param>
    public DateTimeLessThanAttribute(DateTime dateTime)
    {
        this.dateTime = dateTime;
    }

    /// <inheritdoc/>
    public override string FormatErrorMessage(string name)
    {
        return "Date value should not be ahead of the specified date.";
    }

    /// <inheritdoc/>
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is DateTime dateValue && dateValue.Date >= this.dateTime)
        {
           return new ValidationResult(this.FormatErrorMessage(string.Empty));
        }

        return ValidationResult.Success;
    }
}
