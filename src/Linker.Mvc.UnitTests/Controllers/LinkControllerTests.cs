﻿namespace Linker.Mvc.UnitTests.Controllers;

using Linker.Core.V2.Models;
using Linker.TestCore.DataBuilders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public sealed class LinkControllerTests
{
    private readonly LinkControllerSteps steps = new();

    [Theory]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, true)]
    public void Constructor_NullParameters_Throws(
        bool isRepoNull,
        bool isMapperNull,
        bool isLoggerNull)
    {
        this.steps
            .WhenIInitWith(isRepoNull, isMapperNull, isLoggerNull)
            .ThenIExpectExceptionIsThrown<ArgumentNullException>();
    }

    [Fact]
    public async Task Index_NoErrors_ReturnsLinks()
    {
        IEnumerable<Link> links = [new LinkDataBuilder().Build()];

        await this.steps
            .GivenGetAllAsyncReturns(links)
            .WhenIVisitIndexAsync();

        this.steps
            .ThenIExpectRepositoryGetAllAsyncCalled(1)
            .ThenIExpectResultToBeOfType<ViewResult>()
            .ThenIExpectViewResultToContain((links, LinkType.None))
            .ThenIExpectNoExceptionIsThrown();
    }
}
