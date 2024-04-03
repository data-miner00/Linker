namespace Linker.Data.SqlServer;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using EnsureThat;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;

/// <summary>
/// The implementation of the user repository.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="connection">The SQLite connection.</param>
    /// <exception cref="ArgumentNullException">Throws when <see cref="IDbConnection"/> provided is null.</exception>
    public UserRepository(IDbConnection connection)
    {
        this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">The operation cancelled exception.</exception>
    public Task AddAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var insertToUsersOperation = @"
            INSERT INTO Users (
                Id,
                Username,
                Email,
                PhotoUrl,
                Description,
                Role,
                Status,
                DateOfBirth,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @Id,
                @Username,
                @Email,
                @PhotoUrl,
                @Description,
                @Role,
                @Status,
                @DateOfBirth,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        return this.connection.ExecuteAsync(insertToUsersOperation, new
        {
            user.Id,
            user.Username,
            user.Email,
            user.PhotoUrl,
            user.Description,
            Role = user.Role.ToString(),
            Status = user.Status.ToString(),
            user.DateOfBirth,
            user.CreatedAt,
            user.ModifiedAt,
        });
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">The operation cancelled exception.</exception>
    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var selectFromUsersQuery = @"SELECT * FROM Users;";

        return this.connection.QueryAsync<User>(selectFromUsersQuery, cancellationToken);
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">The operation cancelled exception.</exception>
    /// <exception cref="InvalidOperationException">Throws when user is not found.</exception>
    public Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var selectFromUsersQuery = @"SELECT * FROM Users WHERE Username = @Username;";

        return this.connection.QueryFirstAsync<User>(selectFromUsersQuery, param: new
        {
            Username = username,
        });
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">The operation cancelled exception.</exception>
    public Task<User> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var selectFromUsersQuery = @"SELECT * FROM Users WHERE Id = @Id;";

        return this.connection.QueryFirstAsync<User>(selectFromUsersQuery, param: new
        {
            Id = id,
        });
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">The operation cancelled exception.</exception>
    public Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var deleteFromUsersOperation = @"DELETE FROM Users Where Id = @Id;";

        return this.connection.ExecuteAsync(deleteFromUsersOperation, new
        {
            Id = id,
        });
    }

    /// <inheritdoc/>
    /// <exception cref="OperationCanceledException">The operation cancelled exception.</exception>
    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var updateUsersOperation = @"
            UPDATE Users
            SET
                Email = @Email,
                Role = @Role,
                Description = @Description,
                Status = @Status,
                PhotoUrl = @PhotoUrl,
                ModifiedAt = @ModifiedAt
            WHERE
                Id = @Id;
        ";

        return this.connection.ExecuteAsync(updateUsersOperation, new
        {
            user.Email,
            Role = user.Role.ToString(),
            user.Description,
            Status = user.Status.ToString(),
            user.PhotoUrl,
            ModifiedAt = DateTime.Now,
            user.Id,
        });
    }
}
