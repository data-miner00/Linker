namespace Linker.Cli.UnitTests.StepDefinitions;

using Linker.Cli.Handlers;
using Linker.Cli.Integrations;
using Linker.Cli.Core;
using System;
using System.Threading.Tasks;
using Linker.TestCore;
using Moq;
using Linker.Cli.Commands;
using Microsoft.Data.Sqlite;
using TechTalk.SpecFlow.Assist;
using Linker.Cli.UnitTests.Support;
using Spectre.Console.Rendering;

using IAnsiConsole = Spectre.Console.IAnsiConsole;

[Binding]
[Scope(Feature = "AddLinkCommandHandler")]
internal class AddLinkCommandHandlerStepDefinitions : BaseSteps<AddLinkCommandHandlerStepDefinitions>
{
    private readonly Mock<IRepository<Link>> mockRepository;
    private readonly Mock<IAnsiConsole> mockConsole;
    private readonly AddLinkCommandHandler commandHandler;

    private IRepository<Link>? repository;
    private IAnsiConsole? console;
    private object? commandArguments;

    public AddLinkCommandHandlerStepDefinitions()
    {
        this.mockRepository = new Mock<IRepository<Link>>();
        this.mockConsole = new Mock<IAnsiConsole>();
        this.commandHandler = new AddLinkCommandHandler(this.mockRepository.Object, this.mockConsole.Object);
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        Service.Instance.ValueRetrievers.Unregister<TechTalk.SpecFlow.Assist.ValueRetrievers.StringValueRetriever>();
        Service.Instance.ValueRetrievers.Register(new StringValueRetriver());
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
        this.commandArguments = new AddLinkCommandArguments
        {
            ShowHelp = true,
        };
    }

    [Given(@"the command with url ""(.*?)""")]
    public void GivenTheCommandWithUrl(string url)
    {
        this.commandArguments = new AddLinkCommandArguments
        {
            Url = url,
        };
    }

    [Given(@"the url already exists")]
    public void GivenUrlAlreadyExists()
    {
        var exception = new Exception(
            "Outer error",
            new Exception("SQLite Error 19: 'UNIQUE constraint failed: Links.Url'"));

        this.mockRepository
            .Setup(x => x.AddAsync(It.IsAny<Link>()))
            .ThrowsAsync(exception);
    }

    [Given("SqliteException occurs")]
    public void GivenSqlExceptionOccurs()
    {
        var exception = new SqliteException("Error!", 888);

        this.mockRepository
            .Setup(x => x.AddAsync(It.IsAny<Link>()))
            .ThrowsAsync(exception);
    }

    [Given("I have the command argument")]
    public void GivenIHaveTheCommandArgument(Table values)
    {
        var arguments = values.CreateInstance<AddLinkCommandArguments>();
        this.commandArguments = arguments;
    }

    [When("I instantiate the AddLinkCommandHandler")]
    public void WhenIInstantiateTheAddLinkCommandHandler()
    {
        this.RecordException(() => new AddLinkCommandHandler(this.repository!, this.console!));
    }

    [When("I handle the command arguments")]
    public Task WhenIHandleTheCommandArguments()
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

    [Then("I should expect SqliteException to be thrown")]
    public void ThenIShouldExpectSqliteExceptionToBeThrown()
    {
        this.ThenIExpectExceptionIsThrown<SqliteException>();
    }

    [Then("I should expect no exception is thrown")]
    public void ThenIShouldExpectNoExceptionIsThrown()
    {
        this.ThenIExpectNoExceptionIsThrown();
    }

    [Then("I should expect AddAsync to not be called")]
    public void ThenIShouldExpectAddAsyncToNotBeCalled()
    {
        this.mockRepository
            .Verify(x => x.AddAsync(It.IsAny<Link>()), Times.Never());
    }

    [Then(@"I should expect AddAsync to be called with command with url ""(.*?)""")]
    public void ThenIShouldExpectAddAsyncToBeCalledWith(string url)
    {
        this.mockRepository
            .Verify(x => x.AddAsync(It.Is<Link>(l => l.Url == url)), Times.Once());
    }

    [Then("I should expect environment exit code to be (.*)")]
    public void ThenIShouldExpectEnvironmentExitCodeToBe(int exitCode)
    {
        Assert.Equal(exitCode, Environment.ExitCode);
    }

    [Then("I should expect console write to be called")]
    public void ThenIShouldExpectConsoleMarkupLineToBeCalled()
    {
        this.mockConsole
            .Verify(x => x.Write(It.IsAny<IRenderable>()), Times.Once());
    }

    public override AddLinkCommandHandlerStepDefinitions GetSteps()
    {
        return this;
    }
}
