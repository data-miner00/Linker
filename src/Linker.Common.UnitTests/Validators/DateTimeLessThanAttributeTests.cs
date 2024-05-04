namespace Linker.Common.UnitTests.Validators;

using Linker.Common.Validators;
using System;
using Xunit;

public sealed class DateTimeLessThanAttributeTests
{
    private readonly DateTime target = new(2021, 1, 1, 0, 0, 0, DateTimeKind.Local);
    private readonly DateTimeLessThanAttribute attribute;

    public DateTimeLessThanAttributeTests()
    {
        this.attribute = new(this.target);
    }

    [Theory]
    [InlineData(2021)]
    [InlineData(2024)]
    public void IsValid_AheadOrEqualOfTarget_Invalid(int year)
    {
        var date = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Local);

        var result = this.attribute.IsValid(date);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_BeforeTarget_Valid()
    {
        var date = new DateTime(1998, 1, 1, 0, 0, 0, DateTimeKind.Local);

        var result = this.attribute.IsValid(date);

        Assert.True(result);
    }
}
