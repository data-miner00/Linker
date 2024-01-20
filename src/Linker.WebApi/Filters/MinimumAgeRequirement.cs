namespace Linker.WebApi.Filters;

using Microsoft.AspNetCore.Authorization;

public sealed class MinimumAgeRequirement : IAuthorizationRequirement
{
    private int age;

    public MinimumAgeRequirement(int age)
    {
        this.age = age;
    }

    public int Age => this.age;
}
