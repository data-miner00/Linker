namespace Linker.Core.V2.Models;

using System.Security.Authentication;

public class Credential
{
    /// <summary>
    /// Gets or sets the user that this credential belongs to.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the password hash.
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the password salt.
    /// </summary>
    public string PasswordSalt { get; set; }

    /// <summary>
    /// Gets or sets the hashing algorithm.
    /// </summary>
    public HashAlgorithmType HashAlgorithmType { get; set; }

    /// <summary>
    /// Gets or sets the previous password hash.
    /// </summary>
    public string? PreviousPasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the updated time.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
