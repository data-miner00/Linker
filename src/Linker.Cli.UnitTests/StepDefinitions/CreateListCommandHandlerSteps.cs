namespace Linker.Cli.UnitTests.StepDefinitions;

using Linker.Cli.Commands;
using Linker.Cli.Core;
using Linker.Cli.Handlers;
using Linker.Cli.Integrations;
using Linker.TestCore;
using Moq;
using Spectre.Console.Rendering;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Assist;

using IAnsiConsole = Spectre.Console.IAnsiConsole;

[Binding]
[Scope(Feature = "CreateListCommandHandler")]
internal class CreateListCommandHandlerSteps : BaseSteps<CreateListCommandHandlerSteps>
{
    private readonly Mock<IRepository<UrlList>> mockRepository;
    private readonly Mock<IAnsiConsole> mockConsole;
    private readonly CreateListCommandHandler handler;

    private IRepository<UrlList>? repository;
    private IAnsiConsole? console;
    private object? commandArguments;

    public CreateListCommandHandlerSteps()
    {
        this.mockRepository = new();
        this.mockConsole = new();
        this.handler = new CreateListCommandHandler(this.mockRepository.Object, this.mockConsole.Object);
    }

    [Given("the repository is (.*?)null")]
    public void GivenTheRepositoryIs(string isNull)
    {
        this.repository = string.IsNullOrEmpty(isNull)
            ? null
            : this.mockRepository.Object;
    }

    [Given("the console is (.*?)null")]
    public void GivenTheConsoleIs(string isNull)
    {
        this.console = string.IsNullOrEmpty(isNull)
            ? null
            : this.mockConsole.Object;
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
        this.commandArguments = new CreateListCommandArguments
        {
            ShowHelp = true,
        };
    }

    [Given("the command argument has name \"(.*)\" and description \"(.*)\"")]
    public void GivenTheCommandArgumentHas(string name, string description)
    {
        this.commandArguments = new CreateListCommandArguments
        {
            Name = name,
            Description = description,
        };
    }

    [When("I instantiate the CreateListCommandHandler")]
    public void WhenIConstructTheHandler()
    {
        this.RecordException(() => new CreateListCommandHandler(this.repository, this.console));
    }

    [When("I handle the command arguments")]
    public Task WhenIHandleTheCommandArguments()
    {
        return this.RecordExceptionAsync(() => this.handler.HandleAsync(this.commandArguments));
    }

    [Then("I should expect (.*?) exception to be thrown")]
    public void ThenIExpectExceptionToThrow(string errorType)
    {
        var error = string.Equals(errorType, "argument null")
            ? typeof(ArgumentNullException)
            : string.Equals(errorType, "argument")
            ? typeof(ArgumentException)
            : typeof(InvalidOperationException);

        this.ThenIExpectExceptionIsThrown(error);
    }

    [Then("I should expect no exception is thrown")]
    public void ThenIShouldExpectNoExceptionIsThrown()
    {
        this.ThenIExpectNoExceptionIsThrown();
    }

    [Then("I should expect console write being called (.*) times")]
    public void ThenIShouldExpectConsoleWriteBeingCalled(int times)
    {
        this.mockConsole.Verify(
            x => x.Write(It.IsAny<IRenderable>()), Times.Exactly(times));
    }

    [Then("I should expect repository add async to be called with argument")]
    public void ThenIShouldExpectRepositoryAddAsyncToBeCalledWithArgs(Table values)
    {
        var args = values.CreateInstance<UrlList>();

        this.mockRepository.Verify(
            x => x.AddAsync(It.Is<UrlList>(
                y => y.Name == args.Name && y.Description == args.Description)),
            Times.Once);
    }

    public override CreateListCommandHandlerSteps GetSteps()
    {
        return this;
    }
}
