namespace Linker.Data.SqlServer;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Dapper;
using System.Data;
using EnsureThat;

/// <summary>
/// The repository for working with <see cref="Link"/>.
/// </summary>
public sealed class LinkRepository : ILinkRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkRepository"/> class.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    public LinkRepository(IDbConnection connection)
    {
        this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
    }

    /// <inheritdoc/>
    public async Task AddAsync(Link link, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(link);

        var randomId = Guid.NewGuid().ToString();

        var insertLinkStatement = @"
            INSERT INTO Links (
                Id,
                Url,
                Name,
                Type,
                Category,
                Description,
                Language,
                Rating,
                AddedBy,
                Domain,
                Aesthetics,
                IsSubdomain,
                IsMultilingual,
                IsResource,
                Country,
                KeyPersonName,
                Grammar,
                Visibility,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @Id,
                @Url,
                @Name,
                @Type,
                @Category,
                @Description,
                @Language,
                @Rating,
                @AddedBy,
                @Domain,
                @Aesthetics,
                @IsSubdomain,
                @IsMultilingual,
                @IsResource,
                @Country,
                @KeyPersonName,
                @Grammar,
                @Visibility,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        var selectTagByNameStatement = @"SELECT (Id) FROM Tags WHERE Name = @Name;";

        var insertTagStatement = @"
            INSERT INTO Tags (
                Id,
                Name,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @Id,
                @Name,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        var insertLinksTagsOperation = @"
            INSERT INTO LinksTags (
                LinkId,
                TagId
            ) VALUES (
                @LinkId,
                @TagId
            );
        ";

        await this.connection.ExecuteAsync(insertLinkStatement, new
        {
            Id = randomId,
            link.Url,
            link.Name,
            Type = link.Type.ToString(),
            Category = link.Category.ToString(),
            link.Description,
            Language = link.Language.ToString(),
            Rating = link.Rating.ToString(),
            link.AddedBy,
            link.Domain,
            Aesthetics = link.Aesthetics.ToString(),
            link.IsSubdomain,
            link.IsMultilingual,
            link.IsResource,
            link.Country,
            link.KeyPersonName,
            Grammar = link.Grammar.ToString(),
            Visibility = link.Visibility.ToString(),
            link.CreatedAt,
            link.ModifiedAt,
        }).ConfigureAwait(false);

        foreach (var tag in link.Tags)
        {
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("Name", tag, DbType.String);

            var result = await this.connection
                .QueryAsync<Tag>(selectTagByNameStatement, dynamicParams)
                .ConfigureAwait(false);

            if (!result.Any())
            {
                var randomTagId = Guid.NewGuid().ToString();
                await this.connection.ExecuteAsync(insertTagStatement, new
                {
                    Id = randomTagId,
                    Name = tag,
                    link.CreatedAt,
                    link.ModifiedAt,
                }).ConfigureAwait(false);
                await this.connection.ExecuteAsync(insertLinksTagsOperation, new
                {
                    LinkId = randomId,
                    TagId = randomTagId,
                }).ConfigureAwait(false);
            }
            else
            {
                await this.connection.ExecuteAsync(insertLinksTagsOperation, new { LinkId = randomId, TagId = result.SingleOrDefault()?.Id });
            }
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Link>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var selectLinkStatement = @"SELECT * FROM Links;";
        var selectLinksTagsQuery = @"SELECT * FROM LinksTags WHERE LinkId = @LinkId;";
        var selectTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        var links = await this.connection.QueryAsync<Link>(selectLinkStatement)
            .ConfigureAwait(false);

        foreach (var link in links)
        {
            var tags = new List<string>();

            var linkTagPairs = await this.connection
                .QueryAsync<LinkTagPair>(selectLinksTagsQuery, new { LinkId = link.Id })
                .ConfigureAwait(false);

            foreach (var pair in linkTagPairs)
            {
                var tag = await this.connection
                    .QueryFirstAsync<Tag>(selectTagsQuery, new { Id = pair.TagId })
                    .ConfigureAwait(false);
                tags.Add(tag.Name);
            }

            link.Tags = tags;
        }

        return links;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Link>> GetAllByCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return this.connection.QueryAsync<Link>("EXEC sp_GetLinksByCategory @Category;", new { Category = category.ToString() });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Link>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var selectLinkStatement = @"SELECT * FROM Links WHERE AddedBy = @UserId;";
        var selectLinksTagsQuery = @"SELECT * FROM LinksTags WHERE LinkId = @LinkId;";
        var selectTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        var links = await this.connection
            .QueryAsync<Link>(selectLinkStatement, new { UserId = userId })
            .ConfigureAwait(false);

        foreach (var link in links)
        {
            var tags = new List<string>();

            var linkTagPairs = await this.connection
                .QueryAsync<LinkTagPair>(selectLinksTagsQuery, new { LinkId = link.Id })
                .ConfigureAwait(false);

            foreach (var pair in linkTagPairs)
            {
                var tag = await this.connection
                    .QueryFirstAsync<Tag>(selectTagsQuery, new { Id = pair.TagId })
                    .ConfigureAwait(false);
                tags.Add(tag.Name);
            }

            link.Tags = tags;
        }

        return links;
    }

    /// <inheritdoc/>
    public async Task<Link> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tags = new List<string>();

        var selectFromLinksQuery = @"SELECT * FROM Links WHERE Id = @Id;";
        var selectFromLinksTagsQuery = @"SELECT * FROM LinksTags WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        var link = await this.connection
            .QueryFirstAsync<Link>(selectFromLinksQuery, new { Id = id })
            .ConfigureAwait(false);

        var tagPairs = await this.connection
            .QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = id })
            .ConfigureAwait(false);

        foreach (var pair in tagPairs)
        {
            var tag = await this.connection
                .QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = pair.TagId })
                .ConfigureAwait(false);
            tags.Add(tag.Name);
        }

        link.Tags = tags;

        return link;
    }

    /// <inheritdoc/>
    public async Task<Link> GetByUserAsync(string userId, string linkId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tags = new List<string>();

        var getByUserIdAndLinkIdQuery = @"
            SELECT * FROM Links
            WHERE
                Id = @LinkId AND
                AddedBy = @UserId;
        ";
        var selectFromLinksTagsQuery = @"SELECT * FROM LinksTags WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        var link = await this.connection.QueryFirstAsync<Link>(getByUserIdAndLinkIdQuery, new
        {
            LinkId = linkId,
            UserId = userId,
        }).ConfigureAwait(false);

        var tagPairs = await this.connection
            .QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = linkId })
            .ConfigureAwait(false);

        foreach (var pair in tagPairs)
        {
            var tag = await this.connection
                .QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = pair.TagId })
                .ConfigureAwait(false);
            tags.Add(tag.Name);
        }

        link.Tags = tags;

        return link;
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        var removeLinkStatement = @"DELETE FROM Links Where Id = @Id;";
        var removeLinkTagsStatement = @"DELETE FROM LinksTags WHERE LinkId = @Id;";

        await this.connection
            .ExecuteAsync(removeLinkTagsStatement, new { Id = id })
            .ConfigureAwait(false);

        await this.connection
            .ExecuteAsync(removeLinkStatement, new { Id = id })
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Link link, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var updateLinkStatement = @"
            UPDATE Links
            SET
                Url = @Url,
                Name = @Name,
                Type = @Type,
                Category = @Category,
                Description = @Description,
                Language = @Language,
                Rating = @Rating,
                AddedBy = @AddedBy,
                Domain = @Domain,
                Aesthetics = @Aesthetics,
                IsSubdomain = @IsSubdomain,
                IsMultilingual = @IsMultilingual,
                IsResource = @IsResource,
                Country = @Country,
                KeyPersonName = @KeyPersonName,
                Grammar = @Grammar,
                Visibility = @Visibility,
                ModifiedAt = @ModifiedAt
            WHERE
                Id = @Id;
        ";

        return this.connection.ExecuteAsync(updateLinkStatement, new
        {
            link.Id,
            link.Url,
            link.Name,
            Type = link.Type.ToString(),
            Category = link.Category.ToString(),
            link.Description,
            Language = link.Language.ToString(),
            Rating = link.Rating.ToString(),
            link.AddedBy,
            link.Domain,
            Aesthetics = link.Aesthetics.ToString(),
            link.IsSubdomain,
            link.IsMultilingual,
            link.IsResource,
            link.Country,
            link.KeyPersonName,
            Grammar = link.Grammar.ToString(),
            Visibility = link.Visibility.ToString(),
            link.ModifiedAt,
        });
    }
}
