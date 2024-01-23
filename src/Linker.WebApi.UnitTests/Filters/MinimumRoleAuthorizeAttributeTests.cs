namespace Linker.WebApi.UnitTests.Filters;

using Xunit;
using Linker.Core.Models;

public sealed class MinimumRoleAuthorizeAttributeTests
{
    private readonly MinimumRoleAuthorizeAttributeSteps steps = new();

    [Theory]
    [InlineData(Role.Owner, Role.Owner)]
    [InlineData(Role.Administrator, Role.Owner)]
    [InlineData(Role.Administrator, Role.Administrator)]
    [InlineData(Role.Moderator, Role.Owner)]
    [InlineData(Role.Moderator, Role.Administrator)]
    [InlineData(Role.Moderator, Role.Moderator)]
    [InlineData(Role.User, Role.Owner)]
    [InlineData(Role.User, Role.Administrator)]
    [InlineData(Role.User, Role.Moderator)]
    [InlineData(Role.User, Role.User)]
    [InlineData(Role.ReadOnlyUser, Role.Owner)]
    [InlineData(Role.ReadOnlyUser, Role.Administrator)]
    [InlineData(Role.ReadOnlyUser, Role.Moderator)]
    [InlineData(Role.ReadOnlyUser, Role.User)]
    [InlineData(Role.ReadOnlyUser, Role.ReadOnlyUser)]
    [InlineData(Role.Guest, Role.Owner)]
    [InlineData(Role.Guest, Role.Administrator)]
    [InlineData(Role.Guest, Role.Moderator)]
    [InlineData(Role.Guest, Role.User)]
    [InlineData(Role.Guest, Role.ReadOnlyUser)]
    [InlineData(Role.Guest, Role.Guest)]
    public void OnAuthorize_MetRequiredRoles_ReturnsSuccess(Role required, Role current)
    {
        this.steps
            .GivenCurrentUserHasRole(current)
            .GivenMinimumRoleRequiredIs(required)
            .WhenIAuthorize()
            .ThenIExpectToBeAuthorized();
    }

    [Theory]
    [InlineData(Role.Owner, Role.Administrator)]
    [InlineData(Role.Owner, Role.Moderator)]
    [InlineData(Role.Owner, Role.User)]
    [InlineData(Role.Owner, Role.ReadOnlyUser)]
    [InlineData(Role.Owner, Role.Guest)]
    [InlineData(Role.Administrator, Role.Moderator)]
    [InlineData(Role.Administrator, Role.User)]
    [InlineData(Role.Administrator, Role.ReadOnlyUser)]
    [InlineData(Role.Administrator, Role.Guest)]
    [InlineData(Role.Moderator, Role.ReadOnlyUser)]
    [InlineData(Role.Moderator, Role.User)]
    [InlineData(Role.Moderator, Role.Guest)]
    [InlineData(Role.User, Role.ReadOnlyUser)]
    [InlineData(Role.User, Role.Guest)]
    [InlineData(Role.ReadOnlyUser, Role.Guest)]
    public void OnAuthorize_FailedRequiredRoles_ReturnsUnauthorized(Role required, Role current)
    {
        this.steps
            .GivenCurrentUserHasRole(current)
            .GivenMinimumRoleRequiredIs(required)
            .WhenIAuthorize()
            .ThenIExpectToBeUnauthorized();
    }

    [Fact]
    public void OnAuthorize_InvalidRoleProvided_ReturnsUnauthorized()
    {
        this.steps
            .GivenCurrentUserHasRole("invalid role")
            .GivenMinimumRoleRequiredIs(Role.Guest)
            .WhenIAuthorize()
            .ThenIExpectToBeUnauthorized();
    }

    [Fact]
    public void OnAuthorize_NoRoleProvided_ReturnsUnauthorized()
    {
        this.steps
            .GivenCurrentUserHasNoRole()
            .GivenMinimumRoleRequiredIs(Role.Guest)
            .WhenIAuthorize()
            .ThenIExpectToBeUnauthorized();
    }
}
