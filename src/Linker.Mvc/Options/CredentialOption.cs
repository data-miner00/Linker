namespace Linker.Mvc.Options;

using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;

/// <summary>
/// The settings for credential related options.
/// </summary>
public sealed class CredentialOption
{
    /// <summary>
    /// Gets or sets the length of the generated salt.
    /// </summary>
    [Range(16, 50)]
    public int SaltLength { get; set; }

    /// <summary>
    /// Gets or sets the hashing algorithm used to hash passwords.
    /// </summary>
    public HashAlgorithmType HashAlgorithmType { get; set; }
}
