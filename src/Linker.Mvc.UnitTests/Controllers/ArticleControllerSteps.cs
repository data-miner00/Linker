namespace Linker.Mvc.UnitTests.Controllers;

using AutoMapper;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Linker.Mvc.Controllers;
using Linker.TestCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;

internal sealed class ArticleControllerSteps : BaseSteps<ArticleControllerSteps>
{
    private readonly Mock<IArticleRepository> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly ArticleController controller;

    public ArticleControllerSteps()
    {
        this.mockRepository = new();
        this.mockMapper = new();

        this.controller = new(this.mockRepository.Object, this.mockMapper.Object);
        this.GivenIHaveDefaultHttpContext();
    }

    public override ArticleControllerSteps GetSteps() => this;

    public ArticleControllerSteps WhenIInitWith(bool isRepoNull, bool isMapperNull)
    {
        var repo = isRepoNull
            ? null
            : this.mockRepository.Object;

        var mapper = isMapperNull
            ? null
            : this.mockMapper.Object;

        return this.RecordException(() => new ArticleController(repo, mapper));
    }

    public ArticleControllerSteps GivenIHaveDefaultHttpContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionDescriptor = new ControllerActionDescriptor();
        var actionContext = new ActionContext(httpContext, new(), actionDescriptor);

        this.controller.ControllerContext = new ControllerContext(actionContext);

        return this;
    }

    public ArticleControllerSteps GivenGetAllAsyncReturns(IEnumerable<Article> articles)
    {
        this.mockRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(articles);

        return this;
    }

    public ArticleControllerSteps GivenGetAllAsyncThrows(Exception exception)
    {
        this.mockRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        return this;
    }

    public Task WhenIVisitIndexAsync()
    {
        return this.RecordExceptionAsync(
            async () => this.Result = await this.controller.Index());
    }

    public ArticleControllerSteps ThenIExpectRepositoryGetAllAsyncCalled(int times)
    {
        this.mockRepository
            .Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Exactly(times));

        return this;
    }

    public ArticleControllerSteps ThenIExpectViewResultToContain(object model)
    {
        (this.Result as ViewResult)?.Model.Should().BeEquivalentTo(model);

        return this;
    }
}
