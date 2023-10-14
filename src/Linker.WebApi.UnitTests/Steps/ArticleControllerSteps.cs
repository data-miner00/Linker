namespace Linker.WebApi.UnitTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.TestCore;
    using Linker.WebApi.Controllers;

    public sealed class ArticleControllerSteps : BaseSteps<ArticleControllerSteps>
    {
        private readonly IArticleController controller;
        private readonly Mock<IArticleRepository> mockRepository;

        private Article article;

        public ArticleControllerSteps()
        {
            this.mockRepository = new Mock<IArticleRepository>();
            this.controller = new ArticleController(this.mockRepository.Object);
        }

        #region Given

        public ArticleControllerSteps GivenRepoAddAsyncCompleted()
        {
            this.mockRepository
                .Setup(x => x.AddAsync(It.IsAny<Article>()))
                .Callback<Article>(article => this.article = article)
                .Returns(Task.CompletedTask);
            return this;
        }

        public ArticleControllerSteps GivenRepoRemoveAsyncCompleted()
        {
            this.mockRepository
                .Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public ArticleControllerSteps GivenRepoRemoveAsyncThrows(Exception exception)
        {
            this.mockRepository
                .Setup(x => x.RemoveAsync(It.IsAny<string>()))
                .ThrowsAsync(exception);
            return this;
        }

        public ArticleControllerSteps GivenRepoUpdateAsyncCompleted()
        {
            this.mockRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Article>()))
                .Callback<Article>(article => this.article = article)
                .Returns(Task.CompletedTask);
            return this;
        }

        public ArticleControllerSteps GivenRepoUpdateAsyncThrows(Exception exception)
        {
            this.mockRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Article>()))
                .Callback<Article>(article => this.article = article)
                .ThrowsAsync(exception);
            return this;
        }

        public ArticleControllerSteps GivenRepoGetAllAsyncReturns(IEnumerable<Article> articles)
        {
            this.mockRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(articles);
            return this;
        }

        public ArticleControllerSteps GivenRepoGetByIdAsyncReturns(Article article)
        {
            this.mockRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(article);
            return this;
        }

        public ArticleControllerSteps GivenRepoGetByIdAsyncThrows(Exception exception)
        {
            this.mockRepository
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ThrowsAsync(exception);
            return this;
        }

        #endregion

        #region When

        public ArticleControllerSteps WhenIInitWithNullRepo()
        {
            this.RecordException(() => new ArticleController(null));
            return this;
        }

        public Task WhenICreateAsync(CreateArticleRequest request)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .CreateAsync(request)
                .ConfigureAwait(false));
        }

        public Task WhenIDeleteAsync(Guid id)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .DeleteAsync(id)
                .ConfigureAwait(false));
        }

        public Task WhenIGetAllAsync()
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .GetAllAsync()
                .ConfigureAwait(false));
        }

        public Task WhenIGetByIdAsync(Guid id)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .GetByIdAsync(id)
                .ConfigureAwait(false));
        }

        public Task WhenIUpdateAsync(Guid id, UpdateArticleRequest request)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .UpdateAsync(id, request)
                .ConfigureAwait(false));
        }

        #endregion

        #region Then

        public ArticleControllerSteps ThenIExpectRepoAddAsyncToBeCalledWith(Article article, int times)
        {
            this.mockRepository
                .Verify(x => x.AddAsync(It.IsAny<Article>()), Times.Exactly(times));
            this.article.Should().BeEquivalentTo(
                article,
                options =>
                    options
                        .Excluding(article => article.Id)
                        .Excluding(article => article.CreatedAt)
                        .Excluding(article => article.ModifiedAt)
                        .Excluding(article => article.LastVisitAt));
            return this;
        }

        public ArticleControllerSteps ThenIExpectRepoRemoveAsyncToBeCalledWith(string id, int times)
        {
            this.mockRepository
                .Verify(x => x.RemoveAsync(id), Times.Exactly(times));
            return this;
        }

        public ArticleControllerSteps ThenIExpectRepoGetAllAsyncToBeCalled(int times)
        {
            this.mockRepository
                .Verify(x => x.GetAllAsync(), Times.Exactly(times));
            return this;
        }

        public ArticleControllerSteps ThenIExpectRepoUpdateAsyncToBeCalledWith(Article article, int times)
        {
            this.mockRepository
                .Verify(x => x.UpdateAsync(It.IsAny<Article>()), Times.Exactly(times));
            this.article.Should().BeEquivalentTo(article);
            return this;
        }

        public ArticleControllerSteps ThenIExpectRepoGetByIdAsyncToBeCalledWith(string id, int times)
        {
            this.mockRepository
                .Verify(x => x.GetByIdAsync(id), Times.Exactly(times));
            return this;
        }

        #endregion

        public override ArticleControllerSteps GetSteps() => this;
    }
}
