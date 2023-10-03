namespace Linker.ConsoleUI.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Linker.ConsoleUI.Controllers;
    using Linker.Core.Models;
    using Linker.TestCore.DataBuilders;
    using Moq;
    using Xunit;

    public sealed class ArticleControllerTest
    {
        private readonly ArticleControllerSteps steps = new ArticleControllerSteps();

        [Fact]
        public void ArticleController_InvalidArgs_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new ArticleController(null));
        }

        [Fact]
        public void DisplayAllItems_RepositoryGetAllCalled()
        {
            var articles = new List<Article>
            {
                new ArticleDataBuilder().Build(),
            };

            this.steps
                .GivenIHaveReadLineInput("Input")
                .GivenIHaveArticles(articles)
                .WhenIDisplayAllItems()
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepositoryGetAllToBeCalled(Times.Once());
        }

        [Fact]
        public void DisplayItemDetails_RepositoryGetByIdCalled()
        {
            var id = "123";

            this.steps
                .GivenIHaveReadLineInput(id)
                .WhenIDisplayItemDetails()
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepositoryGetByIdToBeCalledWith(id, Times.Once());
        }
    }
}
