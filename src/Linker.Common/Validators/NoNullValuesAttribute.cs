namespace Linker.Common.Validators;

using System.Collections;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Validates if a collection have null inside it.
/// </summary>
public sealed class NoNullValuesAttribute : ValidationAttribute
{
    /// <inheritdoc/>
    public override string FormatErrorMessage(string name)
    {
        return "Collection items must not be null.";
    }

    /// <inheritdoc/>
    protected override ValidationResult IsValid(
        object value,
        ValidationContext validationContext)
    {
        if (value is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is null)
                {
                    return new ValidationResult(this.FormatErrorMessage(string.Empty));
                }
            }
        }

        return ValidationResult.Success;
    }
}
