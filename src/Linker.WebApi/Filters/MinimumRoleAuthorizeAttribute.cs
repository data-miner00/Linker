namespace Linker.WebApi.Filters;

using Linker.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

/// <summary>
/// Authorize the minimum allowed role.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class MinimumRoleAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role minimumRole;

    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumRoleAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="minimumRole">The minimum role required.</param>
    public MinimumRoleAuthorizeAttribute(Role minimumRole)
    {
        this.minimumRole = minimumRole;
    }

    /// <inheritdoc/>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var stringRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        if (stringRole is not null
            && Enum.TryParse(typeof(Role), stringRole, out var currentRole)
            && (Role)currentRole <= this.minimumRole)
        {
            return;
        }

        context.Result = new UnauthorizedResult();
    }
}
