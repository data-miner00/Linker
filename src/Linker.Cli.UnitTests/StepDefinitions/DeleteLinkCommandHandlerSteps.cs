namespace Linker.Cli.UnitTests.StepDefinitions;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Handlers;
using Linker.Cli.Integrations;
using Linker.TestCore;
using Moq;
using System;
using System.Threading.Tasks;

[Binding]
[Scope(Feature = "DeleteLinkCommandHandler")]
internal class DeleteLinkCommandHandlerSteps : BaseSteps<DeleteLinkCommandHandlerSteps>
{
    private readonly Mock<IRepository<Link>> mockRepository;
    private readonly DeleteLinkCommandHandler commandHandler;
    private IRepository<Link>? repository;
    private object? commandArguments;

    public DeleteLinkCommandHandlerSteps()
    {
        this.mockRepository = new Mock<IRepository<Link>>();
        this.commandHandler = new DeleteLinkCommandHandler(this.mockRepository.Object);
    }

    [Given("the link repository is null")]
    public void GivenTheRepositoryIsNull()
    {
        this.repository = null;
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

    [Given("the command argument is help")]
    public void GivenTheCommandArgumentIsHelp()
    {
        this.commandArguments = new DeleteLinkCommandArguments
        {
            ShowHelp = true,
        };
    }

    [Given("the link with id (.*) does not exist")]
    public void GivenTheLinkWithIdDoesNotExist(int id)
    {
        this.commandArguments = new DeleteLinkCommandArguments
        {
            Id = id,
        };
        this.mockRepository
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((Link?)null);
    }

    [Given("the command argument with id (.*) and confirm delete is true")]
    public void GivenTheCommandArgumentWithIdAndConfirmDeleteIsTrue(int id)
    {
        this.commandArguments = new DeleteLinkCommandArguments
        {
            Id = id,
            ConfirmDelete = true,
        };
    }

    [Given("the link with id (.*) exists")]
    public void GivenTheLinkWithIdExists(int id)
    {
        this.commandArguments = new DeleteLinkCommandArguments
        {
            Id = id,
            ConfirmDelete = true,
        };
        this.mockRepository
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(new Link { Id = id });
    }

    [When("I instantiate the DeleteLinkCommandHandler")]
    public void WhenIInstantiateTheDeleteLinkCommandHandler()
    {
        this.RecordException(() => new DeleteLinkCommandHandler(this.repository));
    }

    [When("I handle the command arguments")]
    public Task WhenIHandleTheCommandArguments()
    {
        return this.RecordExceptionAsync(() => this.commandHandler.HandleAsync(this.commandArguments));
    }

    [Then("I should expect ArgumentNullException to be thrown")]
    public void ThenIShouldExpectAnExceptionToBeThrown()
    {
        this.ThenIExpectExceptionIsThrown<ArgumentNullException>();
    }

    [Then("I should expect ArgumentException to be thrown")]
    public void ThenIShouldExpectArgumentExceptionToBeThrown()
    {
        this.ThenIExpectExceptionIsThrown<ArgumentException>();
    }

    [Then("I should expect no exception is thrown")]
    public void ThenIShouldExpectNoExceptionIsThrown()
    {
        this.ThenIExpectNoExceptionIsThrown();
    }

    [Then("I should expect GetByIdAsync to not be called")]
    public void ThenIShouldExpectGetByIdAsyncToNotBeCalled()
    {
        this.mockRepository
            .Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never());
    }

    [Then("I should expect RemoveAsync to not be called")]
    public void ThenIShouldExpectRemoveAsyncToNotBeCalled()
    {
        this.mockRepository
            .Verify(x => x.RemoveAsync(It.IsAny<int>()), Times.Never());
    }

    [Then("I should expect GetByIdAsync to be called with id (.*)")]
    public void ThenIShouldExpectGetByIdAsyncToBeCalled(int id)
    {
        this.mockRepository
            .Verify(x => x.GetByIdAsync(id), Times.Once());
    }

    [Then("I should expect RemoveAsync to be called with id (.*)")]
    public void ThenIShouldExpectRemoveAsyncToBeCalled(int id)
    {
        this.mockRepository
            .Verify(x => x.RemoveAsync(id), Times.Once());
    }

    public override DeleteLinkCommandHandlerSteps GetSteps()
    {
        return this;
    }
}
