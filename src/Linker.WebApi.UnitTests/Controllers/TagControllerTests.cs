namespace Linker.WebApi.UnitTests.Controllers;

using Linker.Core.Models;
using Linker.WebApi.UnitTests.Steps;
using Microsoft.AspNetCore.Mvc;

public sealed class TagControllerTests
{
    private readonly TagControllerSteps steps;

    public TagControllerTests()
    {
        this.steps = new TagControllerSteps();
    }

    [Fact]
    public async Task GetAllAsync_RepoReturnTags_Success()
    {
        this.steps
            .GivenIHaveAllTags(Enumerable.Empty<Tag>());

        await this.steps
            .WhenIGetAllTags()
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectRepoGetAllAsyncCalled(1)
            .ThenIExpectResultToBe(new OkObjectResult(Enumerable.Empty<Tag>()));
    }
}
