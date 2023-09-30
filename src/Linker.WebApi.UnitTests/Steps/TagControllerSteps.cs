namespace Linker.WebApi.UnitTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Linker.Core.ApiModels;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.TestCore;
    using Linker.WebApi.Controllers;

    public sealed class TagControllerSteps : BaseSteps<TagControllerSteps>
    {
        private readonly ITagController controller;
        private readonly Mock<ITagRepository> mockRepository;

        public TagControllerSteps()
        {
            this.mockRepository = new Mock<ITagRepository>();
            this.controller = new TagController(this.mockRepository.Object);
        }

        #region Given

        public TagControllerSteps GivenRepoGetByAsyncReturns(Tag tag)
        {
            this.mockRepository
                .Setup(x =>
                    x.GetByAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(tag);
            return this;
        }

        public TagControllerSteps GivenRepoGetByAsyncThrows<TException>(TException exception)
            where TException : Exception
        {
            this.mockRepository
                .Setup(x =>
                    x.GetByAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);
            return this;
        }

        public TagControllerSteps GivenRepoAddAsyncSuccess()
        {
            this.mockRepository
                .Setup(x => x.AddAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public TagControllerSteps GivenRepoAddAsyncThrows(Exception exception)
        {
            this.mockRepository
                .Setup(x => x.AddAsync(It.IsAny<string>()))
                .ThrowsAsync(exception);
            return this;
        }

        public TagControllerSteps GivenRepoAddLinkTagAsyncSuccess()
        {
            this.mockRepository
                .Setup(x => x.AddLinkTagAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public TagControllerSteps GivenRepoEditNameAsyncSuccess()
        {
            this.mockRepository
                .Setup(x => x.EditNameAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public TagControllerSteps GivenRepoDeleteAsyncSuccess()
        {
            this.mockRepository
                .Setup(x => x.DeleteAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public TagControllerSteps GivenRepoDeleteLinkTagAsyncSuccess()
        {
            this.mockRepository
                .Setup(x => x.DeleteLinkTagAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            return this;
        }

        public TagControllerSteps GivenRepoGetAllAsyncReturns(IEnumerable<Tag> tags)
        {
            this.mockRepository.Setup(x =>
                x.GetAllAsync()).ReturnsAsync(tags);
            return this;
        }

        #endregion

        #region When

        public TagControllerSteps WhenIInitWithNull()
        {
            this.RecordException(() => new TagController(null));
            return this;
        }

        public Task WhenIGetAllTags()
        {
            return this.RecordExceptionAsync(this.controller.GetAllAsync);
        }

        public Task WhenIGetByAsync(string? id, string? name)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .GetByAsync(id, name)
                .ConfigureAwait(false));
        }

        public Task WhenICreateAsync(CreateTagRequest request)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .CreateAsync(request)
                .ConfigureAwait(false));
        }

        public Task WhenICreateLinkTagAsync(Guid linkId, Guid tagId)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .CreateLinkTagAsync(linkId, tagId)
                .ConfigureAwait(false));
        }

        public Task WhenIUpdateAsync(Guid id, UpdateTagRequest request)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .UpdateAsync(id, request)
                .ConfigureAwait(false));
        }

        public Task WhenIDeleteAsync(Guid id)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .DeleteAsync(id)
                .ConfigureAwait(false));
        }

        public Task WhenIDeleteLinkTagAsync(Guid linkId, Guid tagId)
        {
            return this.RecordExceptionAsync(
                async () => await this.controller
                .DeleteLinkTagAsync(linkId, tagId)
                .ConfigureAwait(false));
        }

        #endregion

        #region Then

        public TagControllerSteps ThenIExpectRepoGetAllAsyncCalled(int times)
        {
            this.mockRepository.Verify(
                x => x.GetAllAsync(), Times.Exactly(times));
            return this;
        }

        public TagControllerSteps ThenIExpectRepoGetByAsyncCalledWith(string type, string value, int times)
        {
            this.mockRepository.Verify(
                x => x.GetByAsync(type, value), Times.Exactly(times));
            return this;
        }

        public TagControllerSteps ThenIExpectRepoGetByAsyncNotCalled()
        {
            this.mockRepository.Verify(
                x => x.GetByAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
            return this;
        }

        public TagControllerSteps ThenIExpectRepoAddAsyncCalledWith(string tagName, int times)
        {
            this.mockRepository.Verify(
                x => x.AddAsync(tagName), Times.Exactly(times));
            return this;
        }

        public TagControllerSteps ThenIExpectRepoAddLinkTagAsyncCalledWith(string linkId, string tagId, int times)
        {
            this.mockRepository.Verify(
                x => x.AddLinkTagAsync(linkId, tagId),
                Times.Exactly(times));
            return this;
        }

        public TagControllerSteps ThenIExpectRepoEditNameAsyncCalledWith(string id, string newName, int times)
        {
            this.mockRepository.Verify(
                x => x.EditNameAsync(id, newName),
                Times.Exactly(times));
            return this;
        }

        public TagControllerSteps ThenIExpectRepoDeleteAsyncCalledWith(string id, int times)
        {
            this.mockRepository.Verify(
                x => x.DeleteAsync(id),
                Times.Exactly(times));
            return this;
        }

        public TagControllerSteps ThenIExpectRepoDeleteLinkTagAsyncCalledWith(string linkId, string tagId, int times)
        {
            this.mockRepository.Verify(
                x => x.DeleteLinkTagAsync(linkId, tagId),
                Times.Exactly(times));
            return this;
        }

        #endregion

        public override TagControllerSteps GetSteps() => this;
    }
}
