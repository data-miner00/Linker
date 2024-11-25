namespace Linker.Common.UnitTests.Validators;

using Linker.Common.Validators;
using System.Collections.Generic;
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
}
