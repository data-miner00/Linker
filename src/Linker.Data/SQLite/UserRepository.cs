namespace Linker.Data.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Dapper;
    using EnsureThat;
    using Linker.Core.Models;
    using Linker.Core.Repositories;

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

            var randomId = Guid.NewGuid().ToString();

            var insertToUsersOperation = @"
                INSERT INTO Users (
                    Id,
                    Username,
                    PhotoUrl,
                    Password,
                    Role,
                    Status,
                    DateOfBirth,
                    CreatedAt,
                    ModifiedAt
                ) VALUES (
                    @Id,
                    @Username,
                    @PhotoUrl,
                    @Password,
                    @Role,
                    @Status,
                    @DateOfBirth,
                    @CreatedAt,
                    @ModifiedAt
                );
            ";

            return this.connection.ExecuteAsync(insertToUsersOperation, new
            {
                Id = randomId,
                user.Username,
                user.PhotoUrl,
                user.Password,
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
        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await this.GetByIdAsync(user.Id, cancellationToken);

            var updateUsersOperation = @"
                UPDATE Users
                SET
                    Password = @Password,
                    Role = @Role,
                    Status = @Status,
                    PhotoUrl = @PhotoUrl,
                    ModifiedAt = @ModifiedAt
                WHERE
                    Id = @Id;
            ";

            await this.connection.ExecuteAsync(updateUsersOperation, new
            {
                user.Password,
                Role = user.Role.ToString(),
                Status = user.Status.ToString(),
                user.PhotoUrl,
                ModifiedAt = DateTime.Now,
                user.Id,
            });
        }

        /// <inheritdoc/>
        public Task<User> GetByUsernameAndPasswordAsync(string username, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var selectFromUsersQuery = @"
                SELECT * FROM Users
                WHERE
                    Username = @Username AND
                    Password = @Password;
            ";

            return this.connection.QueryFirstAsync<User>(selectFromUsersQuery, param: new
            {
                Username = username,
                Password = password,
            });
        }
    }
}
