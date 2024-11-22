namespace Linker.Core.V2.ApiModels;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The login request class.
/// </summary>
public sealed class LoginRequest
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [StringLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [StringLength(100, MinimumLength = 3)]
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = null!;
}
