namespace Linker.Cli.UnitTests.StepDefinitions;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Handlers;
using Linker.Cli.Integrations;
using Linker.TestCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Binding]
[Scope(Feature = "AddLinkIntoListCommandHandler")]
internal class AddLinkIntoListCommandHandlerSteps : BaseSteps<AddLinkIntoListCommandHandlerSteps>
{
    private readonly Mock<IRepository<UrlList>> mockListRepository;
    private readonly Mock<IRepository<Link>> mockLinkRepository;
    private readonly Mock<AppDbContext> mockContext;
    private readonly AddLinkIntoListCommandHandler commandHandler;

    private IRepository<UrlList>? listRepository;
    private IRepository<Link>? linkRepository;
    private AppDbContext? context;
    private object? commandArguments;
    private UrlList? list;
    private Link? link;

    public AddLinkIntoListCommandHandlerSteps()
    {
        this.mockListRepository = new Mock<IRepository<UrlList>>();
        this.mockLinkRepository = new Mock<IRepository<Link>>();
        this.mockContext = new Mock<AppDbContext>();

        this.mockContext
            .SetupGet(x => x.Database)
            .Returns(new Mock<DatabaseFacade>(new Mock<DbContext>().Object).Object);

        this.commandHandler = new AddLinkIntoListCommandHandler(
            this.mockListRepository.Object,
            this.mockLinkRepository.Object,
            this.mockContext.Object);
    }

    [Given("the list repository is (.*?)null")]
    public void GivenTheListRepositoryIs(string isNull)
    {
        this.listRepository = string.IsNullOrEmpty(isNull)
            ? null
            : this.mockListRepository.Object;
    }

    [Given("the link repository is (.*?)null")]
    public void GivenTheLinkRepositoryIs(string isNull)
    {
        this.linkRepository = string.IsNullOrEmpty(isNull)
            ? null
            : this.mockLinkRepository.Object;
    }

    [Given("the app context is (.*?)null")]
    public void GivenTheAppContextwIs(string isNull)
    {
        this.context = string.IsNullOrEmpty(isNull)
            ? null
            : this.mockContext.Object;
    }

    [Given("the command argument is null")]
    public void GivenTheCommandArgumentIsNull()
    {
        this.commandArguments = null;
    }

    [Given("the command argument is incorrect type")]
    public void GivenTheCommandArgumentIsIncorrectType()
    {
        this.commandArguments = new object();
    }

    [Given("the command argument is valid")]
    public void GivenTheCommandArgumentIsValid()
    {
        this.commandArguments = new AddLinkIntoListCommandArguments
        {
            ListId = 1,
            LinkId = 1,
        };
    }

    [Given("the list does not exist")]
    public void GivenTheListDoesNotExist()
    {
        this.mockListRepository.Setup(
            x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((UrlList?)null);
    }

    [Given("the link exists")]
    public void GivenTheLinkWithUrlExists()
    {
        this.link = new Link { };
        this.mockLinkRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(this.link);
    }

    [Given("the list exists")]
    public void GivenTheListExists()
    {
        this.list = new UrlList
        {
            Id = 1,
            Name = "Test List",
            Description = "A test list for adding links.",
            Links = new List<Link>(),
        };

        this.mockListRepository.Setup(
            x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(this.list);
    }

    [Given("the link does not exist")]
    public void GivenTheLinkDoesNotExist()
    {
        this.mockLinkRepository.Setup(
            x => x.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Link?)null);
    }

    [When("I instantiate the AddLinkIntoListCommandHandler")]
    public void WhenIInstantiateTheAddLinkIntoListCommandHandler()
    {
        this.RecordException(() => new AddLinkIntoListCommandHandler(
            this.listRepository!,
            this.linkRepository!,
            this.context!));
    }

    [When("I handle the command")]
    public Task WhenIHandleTheCommand()
    {
        return this.RecordExceptionAsync(() => this.commandHandler.HandleAsync(this.commandArguments!));
    }

    [Then("I should expect an exception to be thrown")]
    public void ThenIShouldExpectAnExceptionToBeThrown()
    {
        this.ThenIExpectExceptionIsThrown<ArgumentNullException>();
    }

    [Then("I should expect ArgumentException to be thrown")]
    public void ThenIShouldExpectArgumentExceptionToBeThrown()
    {
        this.ThenIExpectExceptionIsThrown<ArgumentException>();
    }

    [Then("I should expect InvalidOperationException to be thrown")]
    public void ThenIShouldExpectInvalidOperationExceptionToBeThrown()
    {
        this.ThenIExpectExceptionIsThrown<InvalidOperationException>();
    }

    [Then("I should expect no exception is thrown")]
    public void ThenIShouldExpectNoExceptionIsThrown()
    {
        this.ThenIExpectNoExceptionIsThrown();
    }

    [Then("I should expect the link added to the list")]
    public void ThenIShouldExpectTheLinkAddedToTheList()
    {
        this.list?.Links.Should().Contain(this.link);
        this.mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    public override AddLinkIntoListCommandHandlerSteps GetSteps()
    {
        return this;
    }
}
