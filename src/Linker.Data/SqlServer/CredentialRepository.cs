namespace Linker.Data.SqlServer;

using Dapper;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The implementation of <see cref="Credential"/> repository.
/// </summary>
public sealed class CredentialRepository : ICredentialRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="CredentialRepository"/> class.
    /// </summary>
    /// <param name="connection">The db connection.</param>
    public CredentialRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    /// <inheritdoc/>
    public Task AddAsync(Credential credential, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var statement = @"
            INSERT INTO Credentials (
                UserId,
                PasswordHash,
                PasswordSalt,
                HashAlgorithmType,
                PreviousPasswordHash,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @UserId,
                @PasswordHash,
                @PasswordSalt,
                @HashAlgorithmType,
                @PreviousPasswordHash,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        return this.connection.ExecuteAsync(statement, new
        {
            credential.UserId,
            credential.PasswordHash,
            credential.PasswordSalt,
            HashAlgorithmType = credential.HashAlgorithmType.ToString(),
            credential.PreviousPasswordHash,
            credential.CreatedAt,
            credential.ModifiedAt,
        });
    }

    /// <inheritdoc/>
    public Task<Credential> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = @"SELECT * FROM Credentials WHERE UserId = @UserId;";

        return this.connection.QueryFirstAsync<Credential>(query, new { UserId = userId });
    }

    /// <inheritdoc/>
    public Task RemoveAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var statement = @"DELETE FROM Credentials WHERE UserId = @UserId;";

        return this.connection.ExecuteAsync(statement, new { UserId = userId });
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Credential credential, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var statement = @"
            UPDATE Credentials
            SET
                PasswordHash = @PasswordHash,
                PasswordSalt = @PasswordSalt,
                HashAlgorithmType = @HashAlgorithmType,
                PreviousPasswordHash = @PreviousPasswordHash,
                ModifiedAt = @ModifiedAt
            WHERE
                UserId = @UserId;            
        ";

        return this.connection.ExecuteAsync(statement, new
        {
            credential.PasswordHash,
            credential.PasswordSalt,
            HashAlgorithmType = credential.HashAlgorithmType.ToString(),
            credential.PreviousPasswordHash,
            ModifiedAt = DateTime.UtcNow,
        });
    }
}
