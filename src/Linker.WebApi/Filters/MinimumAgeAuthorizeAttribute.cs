namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Authorizes a user who met the minimum age requirement.
/// Implementation adapted from <see href="https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iauthorizationpolicyprovider?view=aspnetcore-8.0">Microsoft Docs</see>.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public sealed class MinimumAgeAuthorizeAttribute : AuthorizeAttribute
{
#pragma warning disable SA1310 // Field names should not contain underscore
    private const string POLICY_PREFIX = "MinimumAge";
#pragma warning restore SA1310 // Field names should not contain underscore

    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumAgeAuthorizeAttribute"/> class.
    /// </summary>
    /// <param name="age">The required minimum age.</param>
    public MinimumAgeAuthorizeAttribute(int age) => this.Age = age;

    /// <summary>
    /// Gets or sets the minimum required age.
    /// </summary>
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
