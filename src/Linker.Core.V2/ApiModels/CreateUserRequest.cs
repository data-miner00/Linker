namespace Linker.Core.V2.ApiModels;

using System;

/// <summary>
/// Represents the create user request.
/// </summary>
/// <param name="Username">The username.</param>
/// <param name="Password">The password.</param>
/// <param name="PhotoUrl">The profile image Url.</param>
/// <param name="DateOfBirth">The birthday of the user.</param>
public record CreateUserRequest(string Username, string Password, string PhotoUrl, DateTime DateOfBirth);
