namespace Linker.Core.V2.ApiModels;

using Linker.Common.Validators;
using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents the create user request.
/// </summary>
/// <param name="Username">The username.</param>
/// <param name="Password">The password.</param>
/// <param name="PhotoUrl">The profile image Url.</param>
/// <param name="Email">The email address.</param>
/// <param name="DateOfBirth">The birthday of the user.</param>
public sealed record CreateUserRequest(
    [StringLength(50)]
    [Required]
    string Username,

    [StringLength(100, MinimumLength = 10)]
    [Required]
    string Password,

    [Required]
    [Url]
    string PhotoUrl,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [DateTimeBeforeNow]
    DateTime DateOfBirth);
