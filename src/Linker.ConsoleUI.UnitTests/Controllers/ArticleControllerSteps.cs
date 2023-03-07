namespace Linker.ConsoleUI.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Linker.ConsoleUI.Controllers;
    using Linker.Core.Controllers;
    using Linker.Core.Models;
    using Linker.Core.Repositories;
    using Linker.TestCore;
    using Moq;

    internal class ArticleControllerSteps : BaseSteps<ArticleControllerSteps>
    {
        private readonly Mock<ICsvArticleRepository> mockArticleRepository;
        private readonly IArticleController articleController;

        public ArticleControllerSteps()
        {
            this.mockArticleRepository = new Mock<ICsvArticleRepository>();
            this.articleController = new ArticleController(this.mockArticleRepository.Object);
        }

        public ArticleControllerSteps GivenIHaveReadLineInput(string input)
        {
            Console.SetIn(new StringReader(input));
            return this;
        }

        public ArticleControllerSteps GivenIMockConsoleOutput()
        {
            Console.SetOut(new StringWriter());
            return this;
        }

        public ArticleControllerSteps GivenIHaveArticles(IEnumerable<Article> articles)
        {
            this.mockArticleRepository.Setup(x => x.GetAll()).Returns(articles);
            return this;
        }

        public ArticleControllerSteps ThenIExpectRepositoryGetAllToBeCalled(Times times)
        {
            this.mockArticleRepository.Verify(x => x.GetAll(), times);

            return this;
        }

        public ArticleControllerSteps GivenIHaveArticleDetails(Article article)
        {
            this.mockArticleRepository.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns(article);
            return this;
        }

        public ArticleControllerSteps ThenIExpectRepositoryGetByIdToBeCalled(Times times)
        {
            this.mockArticleRepository.Verify(x => x.GetById(It.IsAny<string>()), times);

            return this;
        }

        public ArticleControllerSteps WhenIDisplayAllItems()
        {
            return this.RecordException(() => this.articleController.DisplayAllItems());
        }

        public ArticleControllerSteps WhenIDisplayItemDetails()
        {
            return this.RecordException(() => this.articleController.DisplayItemDetails());
        }

        public override ArticleControllerSteps GetSteps() => this;
    }
}
