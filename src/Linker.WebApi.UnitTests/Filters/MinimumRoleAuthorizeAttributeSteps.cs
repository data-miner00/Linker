namespace Linker.WebApi.UnitTests.Filters;

using FluentAssertions;
using Linker.Core.Models;
using Linker.WebApi.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

internal sealed class MinimumRoleAuthorizeAttributeSteps
{
    private MinimumRoleAuthorizeAttribute attribute;
    private AuthorizationFilterContext filterContext;

    private object? result;

    public MinimumRoleAuthorizeAttributeSteps GivenCurrentUserHasRole(Role role)
    {
        var roleClaim = new Claim(ClaimTypes.Role, role.ToString());
        var identity = new ClaimsIdentity([roleClaim]);
        var user = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = user,
        };
        var actionContext = new ActionContext(httpContext, new(), new());
        this.filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps GivenCurrentUserHasRole(string role)
    {
        var roleClaim = new Claim(ClaimTypes.Role, role);
        var identity = new ClaimsIdentity([roleClaim]);
        var user = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = user,
        };
        var actionContext = new ActionContext(httpContext, new(), new());
        this.filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps GivenCurrentUserHasNoRole()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = user,
        };
        var actionContext = new ActionContext(httpContext, new(), new());
        this.filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());

        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps GivenMinimumRoleRequiredIs(Role role)
    {
        this.attribute = new MinimumRoleAuthorizeAttribute(role);
        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps WhenIAuthorize()
    {
        this.attribute.OnAuthorization(this.filterContext);
        this.result = this.filterContext.Result;
        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps ThenIExpectResultToBe(IActionResult result)
    {
        this.result.Should().BeAssignableTo<IActionResult>();
        this.result.Should().Be(result);
        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps ThenIExpectToBeAuthorized()
    {
        this.result.Should().BeNull();
        return this;
    }

    public MinimumRoleAuthorizeAttributeSteps ThenIExpectToBeUnauthorized()
    {
        this.result.Should().BeOfType<UnauthorizedResult>();
        return this;
    }
}
