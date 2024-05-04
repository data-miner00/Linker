namespace Linker.Common.UnitTests.Validators;

using Linker.Common.Validators;
using System;
using Xunit;

public sealed class DateTimeBeforeNowAttributeTests
{
    private readonly DateTimeBeforeNowAttribute attribute = new();

    [Fact]
    public void IsValid_DateInFuture_Invalid()
    {
        var futureDate = DateTime.Now.AddDays(1);

        var result = this.attribute.IsValid(futureDate);

        Assert.False(result);
    }

    [Fact]
    public void IsValid_DateInThePast_Valid()
    {
        var futureDate = DateTime.Now.AddDays(-1);

        var result = this.attribute.IsValid(futureDate);

        Assert.True(result);
    }
}
