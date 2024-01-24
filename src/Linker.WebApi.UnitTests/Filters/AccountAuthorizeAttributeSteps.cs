namespace Linker.WebApi.UnitTests.Filters;

using Linker.TestCore;
using Linker.WebApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public sealed class AccountAuthorizeAttributeSteps
    : BaseSteps<AccountAuthorizeAttributeSteps>
{
    private readonly AccountAuthorizeAttribute attribute = new();
    private AuthorizationFilterContext filterContext;
    private ClaimsPrincipal user;
    private RouteData routeData;

    public AccountAuthorizeAttributeSteps GivenMyUserHasId(string guid)
    {
        var idClaim = new Claim(ClaimTypes.NameIdentifier, guid);
        var identity = new ClaimsIdentity([idClaim]);
        this.user = new ClaimsPrincipal(identity);

        return this;
    }

    public AccountAuthorizeAttributeSteps GivenTheRouteUserIdIs(string guid)
    {
        var kvp = KeyValuePair.Create("userId", guid);
        var routeValueDictionary = new RouteValueDictionary([kvp]);
        this.routeData = new RouteData(routeValueDictionary);

        return this;
    }

    public AccountAuthorizeAttributeSteps GivenNoRouteUserId()
    {
        var routeValueDictionary = new RouteValueDictionary();
        this.routeData = new RouteData(routeValueDictionary);
        return this;
    }

    public AccountAuthorizeAttributeSteps WhenIAuthorize()
    {
        var features = new FeatureCollection();
        features[typeof(IRoutingFeature)] = new RoutingFeature()
        {
            RouteData = this.routeData,
        };

        var httpContext = new DefaultHttpContext(features)
        {
            User = this.user,
        };

        var actionContext = new ActionContext(httpContext, this.routeData, new());
        this.filterContext = new(actionContext, []);

        this.RecordException(() => this.attribute.OnAuthorization(this.filterContext));
        this.Result = this.filterContext.Result;
        return this;
    }

    public override AccountAuthorizeAttributeSteps GetSteps() => this;
}
