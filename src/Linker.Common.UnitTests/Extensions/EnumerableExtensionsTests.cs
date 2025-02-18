namespace Linker.Common.UnitTests.Extensions;

using Linker.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public sealed class EnumerableExtensionsTests
{
    private readonly IEnumerable<int> items = [1, 2, 3, 4, 5];

    [Fact]
    public void WithIndex_CallWithArray_ExpectTupleWithIndex()
    {
        var result = this.items.WithIndex().ToArray();
        (int Item, int Index)[] expected =
        [
            (1, 0),
            (2, 1),
            (3, 2),
            (4, 3),
            (5, 4),
        ];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SkipOrAll_Total5Skip2_ExpectLeft3()
    {
        var result = this.items.SkipOrAll(2).ToArray();
        int[] expected = [3, 4, 5];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SkipOrAll_NoSkip_ExpectGetAll()
    {
        int? skip = null;
        var result = this.items.SkipOrAll(skip).ToArray();
        int[] expected = [1, 2, 3, 4, 5];

        Assert.Equal(expected, result);
    }

    [Fact]
    public void TakeOrAll_Total5Take2_ExpectGet3()
    {
        var results = this.items.TakeOrAll(2).ToArray();
        int[] expected = [1, 2];

        Assert.Equal(expected, results);
    }

    [Fact]
    public void TakeOrAll_NoSpecifyTake_ExpectGetAll()
    {
        int? take = null;
        var result = this.items.TakeOrAll(take).ToArray();
        int[] expected = [1, 2, 3, 4, 5];

        Assert.Equal(expected, result);
    }
}
