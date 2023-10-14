namespace Linker.WebApi.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Linker.Core.Models;
    using Linker.TestCore.DataBuilders;
    using Linker.WebApi.UnitTests.Steps;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public sealed class ArticleControllerTests
    {
        private readonly ArticleControllerSteps steps;

        public ArticleControllerTests()
        {
            this.steps = new ArticleControllerSteps();
        }

        [Fact]
        public void Constructor_InitWithInvalidParams_ThrowsException()
        {
            this.steps
                .WhenIInitWithNullRepo()
                .ThenIExpectExceptionIsThrown(typeof(ArgumentNullException));
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_CreatedSuccessfully()
        {
            var request = new CreateArticleRequestDataBuilder().Build();
            var expected = new ArticleDataBuilder()
                .WithTitle(request.Title)
                .WithTags(request.Tags)
                .WithUrl(request.Url)
                .WithCategory(request.Category)
                .WithDescription(request.Description)
                .WithAuthor(request.Author)
                .WithLanguage(request.Language)
                .WithGrammar(request.Grammar)
                .WithYear(request.Year)
                .WithWatchLater(request.WatchLater)
                .WithDomain("google.com")
                .Build();

            var expectedResult = new CreatedAtActionResult(
                actionName: "GetByIdAsync",
                controllerName: null,
                routeValues: new { expected.Id },
                value: request);

            this.steps
                .GivenRepoAddAsyncCompleted();

            await this.steps
                .WhenICreateAsync(request)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoAddAsyncToBeCalledWith(expected, 1)
                .ThenIExpectResultToBe(
                    expectedResult,
                    opt =>
                        opt.Excluding(result => result.RouteValues));
        }

        [Fact]
        public async Task DeleteAsync_ItemFound_DeletedSuccessfully()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            this.steps
                .GivenRepoRemoveAsyncCompleted();

            await this.steps
                .WhenIDeleteAsync(guid)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoRemoveAsyncToBeCalledWith(guidStr, 1)
                .ThenIExpectResultToBe(new NoContentResult());
        }

        [Fact]
        public async Task DeleteAsync_ItemNotFound_ReturnNotFound()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            this.steps
                .GivenRepoRemoveAsyncThrows(new InvalidOperationException());

            await this.steps
                .WhenIDeleteAsync(guid)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoRemoveAsyncToBeCalledWith(guidStr, 1)
                .ThenIExpectResultToBe(new NotFoundResult());
        }

        [Fact]
        public async Task GetAllAsync_HavingResults_ReturnResults()
        {
            var articles = new List<Article>
            {
                new ArticleDataBuilder().Build(),
                new ArticleDataBuilder().Build(),
            };

            this.steps
                .GivenRepoGetAllAsyncReturns(articles);

            await this.steps
                .WhenIGetAllAsync()
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoGetAllAsyncToBeCalled(1)
                .ThenIExpectResultToBe(new OkObjectResult(articles));
        }

        [Fact]
        public async Task GetByIdAsync_FoundRecord_ReturnResult()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);
            var article = new ArticleDataBuilder().WithId(guidStr).Build();

            this.steps
                .GivenRepoGetByIdAsyncReturns(article);

            await this.steps
                .WhenIGetByIdAsync(guid)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoGetByIdAsyncToBeCalledWith(guidStr, 1)
                .ThenIExpectResultToBe(new OkObjectResult(article));
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnNotFound()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            this.steps
                .GivenRepoGetByIdAsyncThrows(new InvalidOperationException());

            await this.steps
                .WhenIGetByIdAsync(guid)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoGetByIdAsyncToBeCalledWith(guidStr, 1)
                .ThenIExpectResultToBe(new NotFoundResult());
        }

        [Fact]
        public async Task UpdateAsync_RecordFound_UpdatedSuccessfully()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            var request = new UpdateArticleRequestDataBuilder().Build();
            var article = new ArticleDataBuilder()
                .WithId(guidStr)
                .WithTitle(request.Title)
                .WithUrl(request.Url)
                .WithCategory(request.Category)
                .WithDescription(request.Description)
                .WithAuthor(request.Author)
                .WithLanguage(request.Language)
                .WithGrammar(request.Grammar)
                .WithYear(request.Year)
                .WithWatchLater(request.WatchLater)
                .WithTags(null)
                .WithDomain("google.com")
                .WithLastVisitAt(new DateTime(1, 1, 1))
                .WithModifiedAt(new DateTime(1, 1, 1))
                .WithCreatedAt(new DateTime(1, 1, 1))
                .Build();

            this.steps
                .GivenRepoUpdateAsyncCompleted();

            await this.steps
                .WhenIUpdateAsync(guid, request)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoUpdateAsyncToBeCalledWith(article, 1)
                .ThenIExpectResultToBe(new NoContentResult());
        }

        [Fact]
        public async Task UpdateAsync_RecordNotFound_ReturnNotFound()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            var request = new UpdateArticleRequestDataBuilder().Build();
            var article = new ArticleDataBuilder()
                .WithId(guidStr)
                .WithTitle(request.Title)
                .WithUrl(request.Url)
                .WithCategory(request.Category)
                .WithDescription(request.Description)
                .WithAuthor(request.Author)
                .WithLanguage(request.Language)
                .WithGrammar(request.Grammar)
                .WithYear(request.Year)
                .WithWatchLater(request.WatchLater)
                .WithTags(null)
                .WithDomain("google.com")
                .WithLastVisitAt(new DateTime(1, 1, 1))
                .WithModifiedAt(new DateTime(1, 1, 1))
                .WithCreatedAt(new DateTime(1, 1, 1))
                .Build();

            this.steps
                .GivenRepoUpdateAsyncThrows(new InvalidOperationException());

            await this.steps
                .WhenIUpdateAsync(guid, request)
                .ConfigureAwait(false);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoUpdateAsyncToBeCalledWith(article, 1)
                .ThenIExpectResultToBe(new NotFoundResult());
        }
    }
}
