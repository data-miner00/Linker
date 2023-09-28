namespace Linker.WebApi.UnitTests;

using FluentAssertions;
using Linker.Core.Models;
using Linker.Core.Repositories;
using Linker.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

public sealed class TagControllerTests
{
    public readonly TagController controller;
    public readonly Mock<ITagRepository> tagRepositoryMock;

    public TagControllerTests()
    {
        this.tagRepositoryMock = new Mock<ITagRepository>();
        this.controller = new TagController(this.tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Test1()
    {
        this.tagRepositoryMock.Setup(
            x => x.GetAllAsync()).ReturnsAsync(Enumerable.Empty<Tag>());

        var result = await this.controller.GetAllAsync().ConfigureAwait(false);

        this.tagRepositoryMock.Verify(
            x => x.GetAllAsync(), Times.Once);

        result.Should().BeEquivalentTo(new OkObjectResult(Enumerable.Empty<Tag>()));
    }
}
