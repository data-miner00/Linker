namespace Linker.WebApi.Filters;

using Linker.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly Role role;

    public RoleAuthorizeAttribute(Role role)
    {
        this.role = role;
    }

    /// <inheritdoc/>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var strRole = context.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        if (strRole is null ||
            !Enum.TryParse(typeof(Role), strRole, true, out var parsedRole) ||
             (Role)parsedRole != this.role)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
