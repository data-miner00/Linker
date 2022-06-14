namespace Linker.ConsoleUI.UnitTests.Controllers
{
    using System;
    using Linker.ConsoleUI.Controllers;
    using Xunit;

    public sealed class ArticleControllerTest
    {
        [Fact]
        public void ArticleController_InvalidArgs_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new ArticleController(null));
        }
    }
}
