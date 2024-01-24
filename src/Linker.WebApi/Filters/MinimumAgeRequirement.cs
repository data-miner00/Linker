namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;

/// <summary>
/// The minimum age authorization requirement.
/// </summary>
public sealed class MinimumAgeRequirement : IAuthorizationRequirement
{
    private readonly int age;

    /// <summary>
    /// Initializes a new instance of the <see cref="MinimumAgeRequirement"/> class.
    /// </summary>
    /// <param name="age">The minimum age required.</param>
    public MinimumAgeRequirement(int age)
    {
        this.age = age;
    }

    /// <summary>
    /// Gets the minimum age required.
    /// </summary>
    public int Age => this.age;
}
