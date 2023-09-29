namespace Linker.WebApi.UnitTests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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

        public TagControllerSteps WhenIInitWithNull()
        {
            this.RecordException(() => new TagController(null));
            return this;
        }

        public TagControllerSteps GivenIHaveAllTags(IEnumerable<Tag> tags)
        {
            this.mockRepository.Setup(x =>
                x.GetAllAsync()).ReturnsAsync(tags);
            return this;
        }

        public Task WhenIGetAllTags()
        {
            return this.RecordExceptionAsync(this.controller.GetAllAsync);
        }

        public TagControllerSteps ThenIExpectRepoGetAllAsyncCalled(int times)
        {
            this.mockRepository.Verify(
                x => x.GetAllAsync(), Times.Exactly(times));
            return this;
        }

        public override TagControllerSteps GetSteps() => this;
    }
}
