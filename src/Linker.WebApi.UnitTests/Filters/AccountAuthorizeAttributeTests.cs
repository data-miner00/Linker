namespace Linker.WebApi.UnitTests.Filters;

using Microsoft.AspNetCore.Mvc;
using System;

public sealed class AccountAuthorizeAttributeTests
{
    private readonly AccountAuthorizeAttributeSteps steps = new();

    [Fact]
    public void OnAuthorize_UserAccessOwnResource_Authorized()
    {
        var userId = Guid.NewGuid().ToString();

        this.steps
            .GivenMyUserHasId(userId)
            .GivenTheRouteUserIdIs(userId)
            .WhenIAuthorize()
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectResultToBeNull();
    }

    [Fact]
    public void OnAuthorize_UserAccessOtherResource_NotAuthorized()
    {
        var userId = Guid.NewGuid().ToString();
        var otherId = Guid.NewGuid().ToString();

        this.steps
            .GivenMyUserHasId(userId)
            .GivenTheRouteUserIdIs(otherId)
            .WhenIAuthorize()
            .ThenIExpectNoExceptionIsThrown()
            .ThenIExpectResultToBeOfType<UnauthorizedResult>();
    }

    [Fact]
    public void OnAuthorize_NoUserIdRouteParam_ThrowsException()
    {
        var userId = Guid.NewGuid().ToString();

        this.steps
            .GivenMyUserHasId(userId)
            .GivenNoRouteUserId()
            .WhenIAuthorize()
            .ThenIExpectResultToBeNull()
            .ThenIExpectExceptionIsThrown<InvalidOperationException>();
    }
}
