namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// The minimum age authorization handler.
/// </summary>
public sealed class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    /// <inheritdoc/>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var dateOfBirthClaim = context.User.FindFirst(
            c => c.Type == ClaimTypes.DateOfBirth);

        if (dateOfBirthClaim is null)
        {
            goto ending;
        }

        var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
        int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
        {
            calculatedAge--;
        }

        if (calculatedAge >= requirement.Age)
        {
            context.Succeed(requirement);
        }

    ending:
        return Task.CompletedTask;
    }
}
