namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Authorizes a user who met the minimum age requirement.
/// Implementation adapted from <see href="https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iauthorizationpolicyprovider?view=aspnetcore-8.0">Microsoft Docs</see>.
/// </summary>
public sealed class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "MinimumAge";

    public MinimumAgeAuthorizeAttribute(int age) => this.Age = age;

    public int Age
    {
        get
        {
            if (int.TryParse(this.Policy.AsSpan(POLICY_PREFIX.Length), out var age))
            {
                return age;
            }

            return default;
        }

        set
        {
            this.Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
