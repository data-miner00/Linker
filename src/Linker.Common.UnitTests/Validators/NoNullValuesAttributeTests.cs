namespace Linker.Common.UnitTests.Validators;

using Linker.Common.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

public sealed class NoNullValuesAttributeTests
{
    private readonly NoNullValuesAttribute attribute = new();

    [Fact]
    public void IsValid_HaveNullValue_Invalid()
    {
        List<string> strings = ["not_null", null];

        var result = this.attribute.IsValid(strings);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_NoNullValue_Valid()
    {
        List<string> strings = ["not_null", "not_null_too"];

        var result = this.attribute.IsValid(strings);

        Assert.True(result);
    }

    [Fact]
    public void StringsTryValidate_HaveNullValue_Invalid()
    {
        var model = new ModelContainsStrings
        {
            Strings = new List<string> { "not_null", null },
        };
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        var success = Validator.TryValidateObject(model, context, results, true);

        Assert.False(success);
        Assert.Single(results);
        Assert.Equal("Collection items must not be null.", results[0].ErrorMessage);
    }

    [Fact]
    public void StringsTryValidate_NoNullValue_Valid()
    {
        var model = new ModelContainsStrings
        {
            Strings = new List<string> { "not_null", "hello" },
        };
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        var success = Validator.TryValidateObject(model, context, results, true);

        Assert.True(success);
        Assert.Empty(results);
    }
}

file sealed class ModelContainsStrings
{
    [NoNullValues]
    public IEnumerable<string> Strings { get; set; } = null!;
}
