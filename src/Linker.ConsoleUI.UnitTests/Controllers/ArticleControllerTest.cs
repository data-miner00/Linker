namespace Linker.ConsoleUI.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Linker.ConsoleUI.Controllers;
    using Linker.Core.Models;
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
                new Article
                {
                    Id = "123",
                    Url = "https://www.google.com/blog",
                    Category = Category.Education,
                    Description = "A long description",
                    Tags = new List<string> { "Tag" },
                    Language = Language.English,
                    LastVisitAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    Title = "mock title",
                    Author = "mock author",
                    Year = 1999,
                    WatchLater = false,
                    Domain = "www.google.com",
                    Grammar = Grammar.Unknown,
                },
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
            this.steps
                .GivenIHaveReadLineInput("Input")
                .WhenIDisplayItemDetails()
                .ThenIExpectNoExceptionIsThrown()
                .ThenIExpectRepositoryGetByIdToBeCalled(Times.Once());
        }
    }
}
