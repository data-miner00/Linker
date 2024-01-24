namespace Linker.Core.ApiModels
{
    using System;
    using Linker.Core.Models;

    /// <summary>
    /// The response when a user is successfully created.
    /// </summary>
    /// <param name="Id">The user Id.</param>
    /// <param name="Username">The username.</param>
    /// <param name="Role">The role of the user.</param>
    /// <param name="Status">The status of the user.</param>
    /// <param name="DateOfBirth">The birthday of the user.</param>
    /// <param name="CreatedAt">Timestamp when user was created.</param>
    /// <param name="ModifiedAt">Timestamp when user was modified.</param>
    public record CreateUserResponse(
        string Id,
        string Username,
        Role Role,
        Status Status,
        DateTime DateOfBirth,
        DateTime CreatedAt,
        DateTime ModifiedAt);
}
