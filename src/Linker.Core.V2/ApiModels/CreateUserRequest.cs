namespace Linker.Core.V2.ApiModels;

using System;

/// <summary>
/// Represents the create user request.
/// </summary>
/// <param name="Username">The username.</param>
/// <param name="Password">The password.</param>
/// <param name="PhotoUrl">The profile image Url.</param>
/// <param name="Email">The email address.</param>
/// <param name="DateOfBirth">The birthday of the user.</param>
public sealed record CreateUserRequest(
    string Username,
    string Password,
    string PhotoUrl,
    string Email,
    DateTime DateOfBirth);
