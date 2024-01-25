namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

/// <summary>
/// Authorize the resource that can only be accessed by the owner.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class AccountAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    /// <inheritdoc/>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var routeParameterDictionary = context.HttpContext.GetRouteData();
        var routeUserId = routeParameterDictionary.Values["userId"]
            ?? throw new InvalidOperationException("The route has no accountId.");

        if (userId is null)
        {
            goto ending;
        }

        if (routeUserId is string targetUserId && userId.Equals(targetUserId, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

    ending:
        context.Result = new UnauthorizedResult();
    }
}
