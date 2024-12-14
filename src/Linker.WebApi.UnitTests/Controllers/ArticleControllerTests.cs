namespace Linker.WebApi.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Linker.Core.Models;
    using Linker.TestCore.DataBuilders;
    using Linker.WebApi.ApiModels;
    using Linker.WebApi.UnitTests.Steps;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;

    public sealed class ArticleControllerTests
    {
        private readonly ArticleControllerSteps steps = new();

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void Constructor_InitWithInvalidParams_ThrowsException(bool isRepoNull, bool isMapperNull, bool isAccessorNull)
        {
            this.steps
                .WhenIInitWith(isRepoNull, isMapperNull, isAccessorNull)
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

            var result = new CreatedResult();

            this.steps
                .GivenUserLoggedIn(expected.CreatedBy)
                .GivenRepoAddAsyncCompleted()
                .GivenPostRequestMapToArticle(expected);

            await this.steps.WhenICreateAsync(request);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectMapperToBeCalledWith(request, 1)
                .ThenIExpectRepoAddAsyncToBeCalledWith(expected, 1)
                .ThenIExpectResultToBe(result);
        }

        [Fact]
        public async Task DeleteAsync_ItemFound_DeletedSuccessfully()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            this.steps
                .GivenRepoRemoveAsyncCompleted();

            await this.steps.WhenIDeleteAsync(guid);

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

            await this.steps.WhenIDeleteAsync(guid);

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

            var articleApiModels = new List<ArticleApiModel>
            {
                new ArticleApiModelDataBuilder().Build(),
                new ArticleApiModelDataBuilder().Build(),
            };

            this.steps
                .GivenRepoGetAllAsyncReturns(articles)
                .GivenMapperMapToArticleApiModelReturns(articleApiModels);

            await this.steps.WhenIGetAllAsync();

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoGetAllAsyncToBeCalled(1)
                .ThenIExpectMapperToBeCalledWithArticle(2)
                .ThenIExpectResultToBe(new OkObjectResult(articleApiModels));
        }

        [Fact]
        public async Task GetByIdAsync_FoundRecord_ReturnResult()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);
            var article = new ArticleDataBuilder().WithId(guidStr).Build();
            var articleApiModel = new ArticleApiModelDataBuilder().WithId(guidStr).Build();

            this.steps
                .GivenRepoGetByIdAsyncReturns(article)
                .GivenMapperMapToArticleApiModelReturns([articleApiModel]);

            await this.steps.WhenIGetByIdAsync(guid);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepoGetByIdAsyncToBeCalledWith(guidStr, 1)
                .ThenIExpectMapperToBeCalledWithArticle(1)
                .ThenIExpectResultToBe(new OkObjectResult(articleApiModel));
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnNotFound()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            this.steps
                .GivenRepoGetByIdAsyncThrows(new InvalidOperationException());

            await this.steps.WhenIGetByIdAsync(guid);

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

            var userId = Guid.NewGuid().ToString();

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
                .WithCreatedBy(userId)
                .Build();

            this.steps
                .GivenUserLoggedIn(userId)
                .GivenRepoUpdateAsyncCompleted()
                .GivenRepoGetByIdAsyncReturns(article)
                .GivenPutRequestMapToArticle(article);

            await this.steps.WhenIUpdateAsync(guid, request);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectMapperToBeCalledWith(request, 1)
                .ThenIExpectRepoUpdateAsyncToBeCalledWith(article, 1)
                .ThenIExpectResultToBe(new NoContentResult());
        }

        [Fact]
        public async Task UpdateAsync_RecordNotFound_ReturnNotFound()
        {
            var guidStr = "ba3e784b-5edd-432d-a6fb-5215c27d83d2";
            var guid = Guid.Parse(guidStr);

            var userId = Guid.NewGuid().ToString();

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
                .WithCreatedBy(userId)
                .Build();

            this.steps
                .GivenUserLoggedIn(userId)
                .GivenRepoUpdateAsyncThrows(new InvalidOperationException())
                .GivenPutRequestMapToArticle(article)
                .GivenRepoGetByIdAsyncReturns(article);

            await this.steps.WhenIUpdateAsync(guid, request);

            this.steps
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectMapperToBeCalledWith(request, 1)
                .ThenIExpectRepoUpdateAsyncToBeCalledWith(article, 1)
                .ThenIExpectResultToBe(new NotFoundResult());
        }
    }
}
