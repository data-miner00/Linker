namespace Linker.Core.ApiModels
{
    using System;

    /// <summary>
    /// Represents the create user request.
    /// </summary>
    /// <param name="Username">The username.</param>
    /// <param name="Password">The password.</param>
    /// <param name="DateOfBirth">The birthday of the user.</param>
    public record CreateUserRequest(string Username, string Password, DateTime DateOfBirth);
}
