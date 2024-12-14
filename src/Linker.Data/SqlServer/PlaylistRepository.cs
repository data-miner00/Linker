namespace Linker.Data.SqlServer;

using Dapper;
using Linker.Common.Helpers;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
    public async Task<IEnumerable<Link>> GetPlaylistLinksAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"
            SELECT LinkId FROM [dbo].[PlaylistLinks]
            WHERE PlaylistId = @Id;";

        var linkIds = await this.connection.QueryAsync<string>(query, new { Id = id }).ConfigureAwait(false);

        if (!linkIds.Any())
        {
            return Enumerable.Empty<Link>();
        }

        var queryBuilder = new StringBuilder();

        foreach (var linkId in linkIds.SkipLast(1))
        {
            queryBuilder.Append($"Id = '{linkId}' OR ");
        }

        var queryLinks = $"SELECT * FROM [dbo].[Links] WHERE {queryBuilder}Id = '{linkIds.Last()}';";

        var links = await this.connection.QueryAsync<Link>(queryLinks).ConfigureAwait(false);

        var selectFromLinksTagsQuery = @"SELECT * FROM [dbo].[LinksTags] WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM [dbo].[Tags] WHERE Id = @Id;";

        foreach (var link in links)
        {
            var tags = new List<string>();

            var tagPairs = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = link.Id });

            foreach (var pair in tagPairs)
            {
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = pair.TagId });
                tags.Add(tag.Name);
            }

            link.Tags = tags;
        }

        return links;
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

    /// <inheritdoc/>
    public Task AddPlaylistLinkAsync(string playlistId, string linkId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var addPlaylistLink = @"
            INSERT INTO [dbo].[PlaylistLinks] (
                PlaylistId,
                LinkId,
                CreatedAt
            ) VALUES (
                @PlaylistId,
                @LinkId,
                @CreatedAt
            );
        ";

        return this.connection.ExecuteAsync(addPlaylistLink, new
        {
            PlaylistId = playlistId,
            LinkId = linkId,
            CreatedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task RemovePlaylistLinkAsync(string playlistId, string linkId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var deleteOperation = "DELETE FROM [dbo].[PlaylistLinks] WHERE PlaylistId = @PlaylistId AND LinkId = @LinkId;";

        return this.connection.ExecuteAsync(deleteOperation, new
        {
            PlaylistId = playlistId,
            LinkId = linkId,
        });
    }
}
