namespace Linker.Mvc.UnitTests.Controllers;

using AutoMapper;
using Linker.Core.V2.Models;
using Linker.Core.V2.QueryParams;
using Linker.Core.V2.Repositories;
using Linker.Mvc.Controllers;
using Linker.TestCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using Serilog;
using Linker.Core.V2.Clients;

internal sealed class LinkControllerSteps : BaseSteps<LinkControllerSteps>
{
    private readonly Mock<ILinkRepository> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILogger> mockLogger;
    private readonly Mock<ILinkUpdatedEventClient> mockClient;
    private readonly LinkController controller;

    public LinkControllerSteps()
    {
        this.mockRepository = new();
        this.mockMapper = new();
        this.mockLogger = new();
        this.mockClient = new();

        this.controller = new(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockClient.Object);
        this.GivenIHaveDefaultHttpContext();
    }

    public override LinkControllerSteps GetSteps() => this;

    public LinkControllerSteps WhenIInitWith(
        bool isRepoNull,
        bool isMapperNull,
        bool isLoggerNull,
        bool isClientNull)
    {
        var repo = isRepoNull
            ? null
            : this.mockRepository.Object;

        var mapper = isMapperNull
            ? null
            : this.mockMapper.Object;

        var logger = isLoggerNull
            ? null
            : this.mockLogger.Object;

        var client = isClientNull
            ? null
            : this.mockClient.Object;

        return this.RecordException(() => new LinkController(repo, mapper, logger, client));
    }

    public LinkControllerSteps GivenIHaveDefaultHttpContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionDescriptor = new ControllerActionDescriptor();
        var actionContext = new ActionContext(httpContext, new(), actionDescriptor);

        this.controller.ControllerContext = new ControllerContext(actionContext);

        return this;
    }

    public LinkControllerSteps GivenGetAllAsyncReturns(IEnumerable<Link> articles)
    {
        this.mockRepository
            .Setup(x => x.GetAllAsync(It.IsAny<GetLinksQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        return this;
    }

    public LinkControllerSteps GivenGetAllAsyncThrows(Exception exception)
    {
        this.mockRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        return this;
    }

    public Task WhenIVisitIndexAsync()
    {
        return this.RecordExceptionAsync(
            async () => this.Result = await this.controller.Index(new()));
    }

    public LinkControllerSteps ThenIExpectRepositoryGetAllAsyncCalled(int times)
    {
        this.mockRepository
            .Verify(x => x.GetAllAsync(It.IsAny<GetLinksQueryParams>(), It.IsAny<CancellationToken>()), Times.Exactly(times));

        return this;
    }

    public LinkControllerSteps ThenIExpectViewResultToContain(object model)
    {
        (this.Result as ViewResult)?.Model.Should().BeEquivalentTo(model);

        return this;
    }
}
