namespace Linker.Mvc.UnitTests.StepDefinitions;

using Linker.TestCore;
using TechTalk.SpecFlow;
using FluentAssertions;
using Linker.Mvc.Controllers;
using Moq;
using AutoMapper;
using Serilog;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Linker.TestCore.DataBuilders;
using Linker.Core.V2.QueryParams;

[Binding]
public sealed class LinkControllerSteps : BaseSteps<LinkControllerSteps>
{
    private readonly Mock<ILinkRepository> mockRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILogger> mockLogger;
    private readonly LinkController controller;

    public LinkControllerSteps()
    {
        this.mockRepo = new Mock<ILinkRepository>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILogger>();

        this.controller = new LinkController(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object); ;
    }

    // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

    [BeforeScenario("@nulls")]
    public void BeforeScenarioWithTag()
    {
        // Example of filtering hooks using tags. (in this case, this 'before scenario' hook will execute if the feature/scenario contains the tag '@tag1')
        // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=hooks#tag-scoping

        //TODO: implement logic that has to run before executing each scenario
    }

    [BeforeScenario(Order = 1)]
    public void FirstBeforeScenario()
    {
        // Example of ordering the execution of hooks
        // See https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html?highlight=order#hook-execution-order

        //TODO: implement logic that has to run before executing each scenario
    }

    [AfterScenario]
    public void AfterScenario()
    {
        //TODO: implement logic that has to run after executing each scenario
    }

    [Given(@"controller context is properly setup")]
    public void GivenIHaveDefaultHttpContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionDescriptor = new ControllerActionDescriptor();
        var actionContext = new ActionContext(httpContext, new(), actionDescriptor);

        this.controller.ControllerContext = new ControllerContext(actionContext);
    }

    [Given(@"the repository is able to fetch all links that contains name ""(.*?)""")]
    public void GivenRepositoryReturnLinksWithAnExampleName(string urlName)
    {
        IEnumerable<Link> links = [
            new LinkDataBuilder()
                .WithId("random_id")
                .WithAddedBy("owner_id")
                .WithName(urlName)
                .Build()
        ];

        this.mockRepo
            .Setup(x => x.GetAllAsync(It.IsAny<GetLinksQueryParams>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(links);
    }

    [When(@"I initialize with ""Null"" for repo ""(.*?)"" and ""Null"" for mapper ""(.*?)"" and ""Null"" for logger ""(.*?)""")]
    public void WhenIInitializeWith(string isRepoNull, string isMapperNull, string isLoggerNull)
    {
        var repo = isRepoNull == "true"
            ? null
            : this.mockRepo.Object;

        var mapper = isMapperNull == "true"
            ? null
            : this.mockMapper.Object;

        var logger = isLoggerNull == "true"
            ? null
            : this.mockLogger.Object;

        this.RecordException(() => new LinkController(repo, mapper, logger));
    }

    [When(@"I invoke ""Index"" method")]
    public async Task WhenIInvokeIndex()
    {
        await this.RecordExceptionAsync(
            async () => this.Result = await this.controller.Index(new()));
    }

    [Then(@"I expect ""ArgumentNullException"" should be thrown.")]
    public void ThenIExpectErrorThrown()
    {
        this.ThenIExpectExceptionIsThrown<ArgumentNullException>();
    }

    [Then(@"the repository's ""GetAllAsync"" method should be called (\d+) times")]
    public void ThenIExpectRepositoryGetAllAsyncCalled(int times)
    {
        this.mockRepo.Verify(x => x.GetAllAsync(It.IsAny<GetLinksQueryParams>(), It.IsAny<CancellationToken>()), Times.Exactly(times));
    }

    [Then(@"I expect the response to contain a list of links that contains ""(.*?)""")]
    public void ThenIExpectLinksToBe(string urlName)
    {
        IEnumerable<Link> links = [
            new LinkDataBuilder()
                .WithId("random_id")
                .WithAddedBy("owner_id")
                .WithName(urlName)
                .Build()
        ];

        (this.Result as ViewResult)?
            .Model.Should().BeEquivalentTo(
                (object)(links, LinkType.None),
                options => options.Excluding(su => su.Type == typeof(DateTime)));
    }

    [Then(@"no exception was thrown")]
    public void ThenNoExceptionThrown()
    {
        this.ThenIExpectNoExceptionIsThrown();
    }

    public override LinkControllerSteps GetSteps() => this;
}
