namespace Linker.Mvc.UnitTests.Controllers;

using Linker.Core.Models;
using Linker.TestCore.DataBuilders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class ArticleControllerTests
{
    private readonly ArticleControllerSteps steps = new();

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public void Constructor_NullParameters_Throws(bool isRepoNull, bool isMapperNull)
    {
        this.steps
            .WhenIInitWith(isRepoNull, isMapperNull)
            .ThenIExpectExceptionIsThrown<ArgumentNullException>();
    }

    [Fact]
    public async Task Index_NoErrors_ReturnsArticles()
    {
        IEnumerable<Article> articles = [new ArticleDataBuilder().Build()];

        await this.steps
            .GivenGetAllAsyncReturns(articles)
            .WhenIVisitIndexAsync();

        this.steps
            .ThenIExpectRepositoryGetAllAsyncCalled(1)
            .ThenIExpectResultToBeOfType<ViewResult>()
            .ThenIExpectViewResultToContain(articles)
            .ThenIExpectNoExceptionIsThrown();
    }
}
