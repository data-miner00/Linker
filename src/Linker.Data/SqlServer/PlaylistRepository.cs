namespace Linker.Data.SqlServer;

using Dapper;
using Linker.Common.Helpers;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The repository layer for <see cref="Playlist"/>.
/// </summary>
public sealed class PlaylistRepository : IPlaylistRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaylistRepository"/> class.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    public PlaylistRepository(IDbConnection connection)
    {
        this.connection = Guard.ThrowIfNull(connection);
    }

    /// <inheritdoc/>
    public Task AddAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var statement = @"
            INSERT INTO [dbo].[Playlists]
            (
                [Id],
                [OwnerId],
                [Name],
                [Description],
                [Visibility],
                [CreatedAt],
                [ModifiedAt]
            )
            VALUES
            (
                @Id,
                @OwnerId,
                @Name,
                @Description,
                @Visibility,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        return this.connection.ExecuteAsync(statement, new
        {
            playlist.Id,
            playlist.OwnerId,
            playlist.Name,
            playlist.Description,
            Visibility = playlist.Visibility.ToString(),
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
        });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Playlist>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = "SELECT * FROM [dbo].[Playlists] WHERE [OwnerId] = @UserId;";

        return this.connection.QueryAsync<Playlist>(query, new { UserId = userId });
    }

    /// <inheritdoc/>
    public Task<Playlist> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = "SELECT * FROM [dbo].[Playlists] WHERE [Id] = @Id;";

        return this.connection.QueryFirstAsync<Playlist>(query, new { Id = id });
    }

    /// <inheritdoc/>
    public Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var command = "DELETE FROM [dbo].[Playlists] WHERE [Id] = @Id;";

        return this.connection.ExecuteAsync(command, new { Id = id });
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Playlist playlist, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var command = @"
            UPDATE [dbo].[Playlists]
            SET
                [Name] = @Name,
                [Description] = @Description,
                [Visibility] = @Visibility,
                [ModifiedAt] = @ModifiedAt
            WHERE [Id] = @Id;
        ";

        return this.connection.ExecuteAsync(command, new
        {
            playlist.Id,
            playlist.Name,
            playlist.Description,
            Visibility = playlist.Visibility.ToString(),
            ModifiedAt = DateTime.UtcNow,
        });
    }
}
