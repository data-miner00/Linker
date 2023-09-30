namespace Linker.WebApi.UnitTests.Controllers;

using System.Data.SQLite;
using Linker.Core.ApiModels;
using Linker.Core.Models;
using Linker.TestCore.DataBuilders;
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
    public void Constructor_InitWithInvalidParams_ThrowsException()
    {
        this.steps
            .WhenIInitWithNull()
            .ThenIExpectExceptionIsThrown(typeof(ArgumentNullException));
    }

    [Fact]
    public async Task GetAllAsync_RepoReturnTags_Success()
    {
        this.steps
            .GivenRepoGetAllAsyncReturns(Enumerable.Empty<Tag>());

        await this.steps
            .WhenIGetAllTags()
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectRepoGetAllAsyncCalled(1)
            .ThenIExpectResultToBe(new OkObjectResult(Enumerable.Empty<Tag>()));
    }

    [Fact]
    public async Task GetByAsync_RepoThrowsInvalidOperationException_ExpectNotFound()
    {
        this.steps
            .GivenRepoGetByAsyncThrows(new InvalidOperationException());

        await this.steps
            .WhenIGetByAsync("my_id", null)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectRepoGetByAsyncCalledWith("Id", "my_id", 1)
            .ThenIExpectResultToBe(new NotFoundResult());
    }

    [Theory]
    [InlineData("ba3e784b-5edd-432d-a6fb-5215c27d83d2", null, "Id")]
    [InlineData(null, "mytag", "Name")]
    public async Task GetByAsync_FoundRecord_ExpectOk(string? id, string? name, string expectedType)
    {
        var tag = new TagDataBuilder().Build();

        this.steps
            .GivenRepoGetByAsyncReturns(tag);

        await this.steps
            .WhenIGetByAsync(id, name)
            .ConfigureAwait(false);

#pragma warning disable CS8604 // Possible null reference argument.
        this.steps
            .ThenIExpectRepoGetByAsyncCalledWith(expectedType, id ?? name, 1)
            .ThenIExpectResultToBe(new OkObjectResult(tag));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public async Task GetByAsync_EmptyQueryParams_ExpectBadRequest()
    {
        await this.steps
            .WhenIGetByAsync(null, null)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectRepoGetByAsyncNotCalled()
            .ThenIExpectResultToBe(new BadRequestResult());
    }

    [Fact]
    public async Task CreateAsync_AddNewTag_Success()
    {
        var request = new CreateTagRequest { TagName = "mytag" };

        this.steps
            .GivenRepoAddAsyncSuccess();

        await this.steps
            .WhenICreateAsync(request)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectRepoAddAsyncCalledWith("mytag", 1)
            .ThenIExpectResultToBe(new NoContentResult());
    }

    [Fact]
    public async Task CreateAsync_TagNameAlreadyExist_ExpectBadRequest()
    {
        var request = new CreateTagRequest { TagName = "mytag" };

        this.steps
            .GivenRepoAddAsyncThrows(new SQLiteException(SQLiteErrorCode.Constraint, "UNIQUE constraint failed: Tags.Name"));

        await this.steps
            .WhenICreateAsync(request)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectRepoAddAsyncCalledWith("mytag", 1)
            .ThenIExpectResultToBe(new BadRequestObjectResult("The tag with the same name already exists."));
    }

    [Fact]
    public async Task CreateLinkTagAsync_SuccessfulCreation_ExpectNoContent()
    {
        var linkId = Guid.Parse("c322f3b1-7a39-4d05-b0c9-f4c8e6e3ce9a");
        var tagId = Guid.Parse("ba3e784b-5edd-432d-a6fb-5215c27d83d2");

        this.steps
            .GivenRepoAddLinkTagAsyncSuccess();

        await this.steps
            .WhenICreateLinkTagAsync(linkId, tagId)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectRepoAddLinkTagAsyncCalledWith(linkId.ToString(), tagId.ToString(), 1)
            .ThenIExpectResultToBe(new NoContentResult());
    }

    [Fact]
    public async Task UpdateAsync_SucessfulUpdate_ExpectOk()
    {
        var id = Guid.Parse("ba3e784b-5edd-432d-a6fb-5215c27d83d2");
        var request = new UpdateTagRequest { NewName = "newtag" };

        this.steps
            .GivenRepoEditNameAsyncSuccess();

        await this.steps
            .WhenIUpdateAsync(id, request)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectRepoEditNameAsyncCalledWith(id.ToString(), request.NewName, 1)
            .ThenIExpectResultToBe(new OkResult());
    }

    [Fact]
    public async Task DeleteAsync_SuccessfulDelete_ExpectNoContent()
    {
        var id = Guid.Parse("ba3e784b-5edd-432d-a6fb-5215c27d83d2");

        this.steps
            .GivenRepoDeleteAsyncSuccess();

        await this.steps
            .WhenIDeleteAsync(id)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectRepoDeleteAsyncCalledWith(id.ToString(), 1)
            .ThenIExpectResultToBe(new NoContentResult());
    }

    [Fact]
    public async Task DeleteLinkTagAsync_SuccessfulDelete_ExpectNoContent()
    {
        var linkId = Guid.Parse("c322f3b1-7a39-4d05-b0c9-f4c8e6e3ce9a");
        var tagId = Guid.Parse("ba3e784b-5edd-432d-a6fb-5215c27d83d2");

        this.steps
            .GivenRepoDeleteLinkTagAsyncSuccess();

        await this.steps
            .WhenIDeleteLinkTagAsync(linkId, tagId)
            .ConfigureAwait(false);

        this.steps
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectRepoDeleteLinkTagAsyncCalledWith(linkId.ToString(), tagId.ToString(), 1)
            .ThenIExpectResultToBe(new NoContentResult());
    }
}
