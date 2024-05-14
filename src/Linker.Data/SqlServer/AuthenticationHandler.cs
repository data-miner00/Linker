namespace Linker.Data.SqlServer;

using Linker.Common.Helpers;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The implementation for <see cref="IAuthenticationHandler"/>.
/// </summary>
public sealed class AuthenticationHandler : IAuthenticationHandler
{
    private readonly ICredentialRepository repository;
    private readonly int saltLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationHandler"/> class.
    /// </summary>
    /// <param name="repository">The credential repository.</param>
    /// <param name="saltLength">The length of salt generated.</param>
    public AuthenticationHandler(ICredentialRepository repository, int saltLength)
    {
        this.repository = Guard.ThrowIfNull(repository);
        this.saltLength = Guard.ThrowIfLessThan(saltLength, 0);
    }

    /// <inheritdoc/>
    public Task RegisterAsync(string userId, string password, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var salt = this.GenerateRandomString();
        var hash = this.ConvertPasswordAndSaltToHash(salt, password);

        var credentials = new Credential
        {
            UserId = userId,
            PasswordHash = hash,
            PasswordSalt = salt,
            HashAlgorithmType = HashAlgorithmType.Sha1,
            PreviousPasswordHash = null,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        };

        return this.repository.AddAsync(credentials, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> LoginAsync(string userId, string password, CancellationToken cancellationToken)
    {
        var credential = await this.repository.GetByUserIdAsync(userId, cancellationToken);

        var attemptHash = this.ConvertPasswordAndSaltToHash(credential.PasswordSalt, password);

        if (!attemptHash.Equals(credential.PasswordHash))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var credential = await this.repository.GetByUserIdAsync(userId, cancellationToken);

        var attemptHash = this.ConvertPasswordAndSaltToHash(credential.PasswordSalt, oldPassword);

        if (!attemptHash.Equals(credential.PasswordHash))
        {
            return false;
        }

        var newHash = this.ConvertPasswordAndSaltToHash(credential.PasswordSalt, newPassword);

        credential.PasswordSalt = newHash;
        credential.PreviousPasswordHash = attemptHash;

        await this.repository.UpdateAsync(credential, default);

        return true;
    }

    private string GenerateRandomString()
    {
        return RandomNumberGenerator.GetHexString(this.saltLength);
    }

    private string ConvertPasswordAndSaltToHash(string salt, string password)
    {
        var sb = new StringBuilder();
        var mixture = salt + password;
        var encodedMixture = Encoding.UTF8.GetBytes(mixture);
        var hashedBytes = SHA1.HashData(encodedMixture);

        foreach (var bytes in hashedBytes)
        {
            sb.Append(bytes.ToString("x2"));
        }

        return sb.ToString();
    }
}
