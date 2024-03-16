namespace Linker.Data.SQLite;

using Linker.Core.Models;
using Linker.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using EnsureThat;

/// <summary>
/// The workspace repository implementation.
/// </summary>
public sealed class WorkspaceRepository : IWorkspaceRepository
{
    private readonly IDbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkspaceRepository"/> class.
    /// </summary>
    /// <param name="connection">The DB connection.</param>
    public WorkspaceRepository(IDbConnection connection)
    {
        this.connection = EnsureArg.IsNotNull(connection, nameof(connection));
    }

    /// <inheritdoc/>
    public Task AddWorkspaceArticleAsync(string workspaceId, string articleId, CancellationToken cancellationToken)
    {
        var addWorkspaceArticle = @"
            INSERT INTO Workspace_Articles (
                WorkspaceId,
                ArticleId,
                CreatedAt
            ) VALUES (
                @WorkspaceId,
                @ArticleId,
                @CreatedAt
            );";

        return this.connection.ExecuteAsync(addWorkspaceArticle, new
        {
            WorkspaceId = workspaceId,
            ArticleId = articleId,
            CreatedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task AddWorkspaceWebsiteAsync(string workspaceId, string websiteId, CancellationToken cancellationToken)
    {
        var addWorkspaceWebsite = @"
            INSERT INTO Workspace_Websites (
                WorkspaceId,
                WebsiteId,
                CreatedAt
            ) VALUES (
                @WorkspaceId,
                @WebsiteId,
                @CreatedAt
            );";

        return this.connection.ExecuteAsync(addWorkspaceWebsite, new
        {
            WorkspaceId = workspaceId,
            WebsiteId = websiteId,
            CreatedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task AddWorkspaceYoutubeAsync(string workspaceId, string youtubeId, CancellationToken cancellationToken)
    {
        var addWorkspaceYoutube = @"
            INSERT INTO Workspace_Youtube (
                WorkspaceId,
                YoutubeId,
                CreatedAt
            ) VALUES (
                @WorkspaceId,
                @YoutubeId,
                @CreatedAt
            );";

        return this.connection.ExecuteAsync(addWorkspaceYoutube, new
        {
            WorkspaceId = workspaceId,
            YoutubeId = youtubeId,
            CreatedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task AddWorkspaceMembershipAsync(WorkspaceMembership membership, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var addWorkspaceMembership = @"
            INSERT INTO Workspace_Memberships (
                WorkspaceId,
                Userid,
                WorkspaceRole,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @WorkspaceId,
                @UserId,
                @WorkspaceRole,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        return this.connection.ExecuteAsync(addWorkspaceMembership, new
        {
            membership.WorkspaceId,
            membership.UserId,
            WorkspaceRole = membership.WorkspaceRole.ToString(),
            membership.CreatedAt,
            membership.ModifiedAt,
        });
    }

    /// <inheritdoc/>
    public async Task CreateAsync(Workspace workspace, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var insertToWorkspaceOperation = @"
            INSERT INTO Workspaces (
                Id,
                Handle,
                Name,
                OwnerId,
                Description,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @Id,
                @Handle,
                @Name,
                @OwnerId,
                @Description,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        var insertToWorkspaceMembership = @"
            INSERT INTO Workspace_Memberships (
                WorkspaceId,
                UserId,
                WorkspaceRole,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @WorkspaceId,
                @UserId,
                @WorkspaceRole,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        await this.connection.ExecuteAsync(insertToWorkspaceOperation, new
        {
            workspace.Id,
            workspace.Handle,
            workspace.Name,
            workspace.OwnerId,
            workspace.Description,
            workspace.CreatedAt,
            workspace.ModifiedAt,
        }).ConfigureAwait(false);

        await this.connection.ExecuteAsync(insertToWorkspaceMembership, new
        {
            WorkspaceId = workspace.Id,
            UserId = workspace.OwnerId,
            WorkspaceRole = nameof(WorkspaceRole.Owner),
            workspace.CreatedAt,
            workspace.ModifiedAt,
        }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task DeleteWorkspaceMembershipAsync(string workspaceId, string userId, CancellationToken cancellationToken)
    {
        var deleteOperation = "DELETE FROM Workspace_Memberships WHERE WorkspaceId = @WorkspaceId AND UserId = @UserId;";

        return this.connection.ExecuteAsync(deleteOperation, new
        {
            WorkspaceId = workspaceId,
            UserId = userId,
        });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Workspace>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = "SELECT * FROM Workspaces;";

        return this.connection.QueryAsync<Workspace>(query);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Workspace>> GetAllByUserAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var getMemberships = "SELECT WorkspaceId FROM Workspace_Memberships WHERE UserId = @UserId;";

        var workspaceIds = await this.connection
            .QueryAsync<string>(getMemberships, new { UserId = userId })
            .ConfigureAwait(false);

        if (!workspaceIds.Any())
        {
            return Enumerable.Empty<Workspace>();
        }

        var queryBuilder = new StringBuilder();

        foreach (var workspaceId in workspaceIds.SkipLast(1))
        {
            queryBuilder.Append($"Id = \"{workspaceId}\" OR ");
        }

        var getWorkspaces = $"SELECT * FROM Workspaces WHERE {queryBuilder}Id = \"{workspaceIds.Last()}\";";

        var workspaces = await this.connection
            .QueryAsync<Workspace>(getWorkspaces)
            .ConfigureAwait(false);

        return workspaces;
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Workspace>> GetAllUnjoinedByUserAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"
            SELECT * FROM Workspaces WHERE Id NOT IN (
                SELECT WorkspaceId FROM Workspace_Memberships
                WHERE UserId = @UserId
            );
        ";

        return this.connection.QueryAsync<Workspace>(query, new { UserId = userId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<WorkspaceMembership>> GetAllWorkspaceMembershipsAsync(string id, CancellationToken cancellationToken)
    {
        var query = @"SELECT * FROM Workspace_Memberships WHERE WorkspaceId = @WorkspaceId;";

        return this.connection.QueryAsync<WorkspaceMembership>(query, new
        {
            WorkspaceId = id,
        });
    }

    /// <inheritdoc/>
    public Task<Workspace> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"SELECT * FROM Workspaces WHERE Id = @Id;";

        return this.connection.QueryFirstAsync<Workspace>(query, new { Id = id });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Article>> GetWorkspaceArticlesAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"
            SELECT ArticleId FROM Workspace_Articles
            WHERE WorkspaceId = @Id;";

        var articleIds = await this.connection.QueryAsync<string>(query, new { Id = id }).ConfigureAwait(false);

        if (!articleIds.Any())
        {
            return Enumerable.Empty<Article>();
        }

        var queryBuilder = new StringBuilder();

        foreach (var articleId in articleIds.SkipLast(1))
        {
            queryBuilder.Append($"Id = \"{articleId}\" OR ");
        }

        var queryArticles = $"SELECT * FROM Articles WHERE {queryBuilder}Id = \"{articleIds.Last()}\";";

        var articles = await this.connection.QueryAsync<Article>(queryArticles).ConfigureAwait(false);

        var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        foreach (var article in articles)
        {
            var tags = new List<string>();

            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = article.Id });

            foreach (var tagz in tagsz)
            {
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            article.Tags = tags;
        }

        return articles;
    }

    /// <inheritdoc/>
    public Task<WorkspaceMembership> GetWorkspaceMembershipAsync(string workspaceId, string userId, CancellationToken cancellationToken)
    {
        var query = @"
            SELECT * FROM Workspace_Memberships
            WHERE WorkspaceId = @WorkspaceId
            AND   UserId = @UserId;
        ";

        return this.connection.QueryFirstAsync<WorkspaceMembership>(query, new
        {
            WorkspaceId = workspaceId,
            UserId = userId,
        });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Website>> GetWorkspaceWebsitesAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"
            SELECT WebsiteId FROM Workspace_Websites
            WHERE WorkspaceId = @Id;";

        var websiteIds = await this.connection.QueryAsync<string>(query, new { Id = id }).ConfigureAwait(false);

        if (!websiteIds.Any())
        {
            return Enumerable.Empty<Website>();
        }

        var queryBuilder = new StringBuilder();

        foreach (var websiteId in websiteIds.SkipLast(1))
        {
            queryBuilder.Append($"Id = \"{websiteId}\" OR ");
        }

        var queryWebsites = $"SELECT * FROM Websites WHERE {queryBuilder}Id = \"{websiteIds.Last()}\";";

        var websites = await this.connection.QueryAsync<Website>(queryWebsites).ConfigureAwait(false);

        var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        foreach (var website in websites)
        {
            var tags = new List<string>();

            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = website.Id });

            foreach (var tagz in tagsz)
            {
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            website.Tags = tags;
        }

        return websites;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Youtube>> GetWorkspaceYoutubesAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"
            SELECT YoutubeId FROM Workspace_Youtube
            WHERE WorkspaceId = @Id;";

        var youtubeIds = await this.connection.QueryAsync<string>(query, new { Id = id }).ConfigureAwait(false);

        if (!youtubeIds.Any())
        {
            return Enumerable.Empty<Youtube>();
        }

        var queryBuilder = new StringBuilder();

        foreach (var youtubeId in youtubeIds.SkipLast(1))
        {
            queryBuilder.Append($"Id = \"{youtubeId}\" OR ");
        }

        var queryYoutubes = $"SELECT * FROM Youtube WHERE {queryBuilder}Id = \"{youtubeIds.Last()}\";";

        var youtubes = await this.connection.QueryAsync<Youtube>(queryYoutubes).ConfigureAwait(false);

        var selectFromLinksTagsQuery = @"SELECT * FROM Links_Tags WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

        foreach (var youtube in youtubes)
        {
            var tags = new List<string>();

            var tagsz = await this.connection.QueryAsync<LinkTagPair>(selectFromLinksTagsQuery, new { LinkId = youtube.Id });

            foreach (var tagz in tagsz)
            {
                var tag = await this.connection.QueryFirstAsync<Tag>(selectFromTagsQuery, new { Id = tagz.TagId });
                tags.Add(tag.Name);
            }

            youtube.Tags = tags;
        }

        return youtubes;
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        cancellationToken.ThrowIfCancellationRequested();

        var deleteWorkspaceOperation = @"DELETE FROM Workspaces WHERE Id = @Id;";
        var deleteWorkspaceMember = @"DELETE FROM Workspace_Memberships WHERE WorkspaceId = @Id;";
        var deleteWorkspaceArticle = @"DELETE FROM Workspace_Articles WHERE WorkspaceId = @Id;";
        var deleteWorkspaceWebsite = @"DELETE FROM Workspace_Websites WHERE WorkspaceId = @Id;";
        var deleteWorkspaceYoutube = @"DELETE FROM Workspace_Youtube WHERE WorkspaceId = @Id;";

        IEnumerable<Task> tasks = [
            this.connection.ExecuteAsync(deleteWorkspaceMember, new { Id = id }),
            this.connection.ExecuteAsync(deleteWorkspaceArticle, new { Id = id }),
            this.connection.ExecuteAsync(deleteWorkspaceWebsite, new { Id = id }),
            this.connection.ExecuteAsync(deleteWorkspaceYoutube, new { Id = id }),
        ];

        await Task.WhenAll(tasks).ConfigureAwait(false);
        await this.connection
            .ExecuteAsync(deleteWorkspaceOperation, new { Id = id })
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task RemoveWorkspaceArticleAsync(string workspaceId, string articleId, CancellationToken cancellationToken)
    {
        var deleteOperation = @"DELETE FROM Workspace_Articles WHERE WorkspaceId = @WorkspaceId AND ArticleId = @ArticleId;";

        return this.connection.ExecuteAsync(deleteOperation, new
        {
            WorkspaceId = workspaceId,
            ArticleId = articleId,
        });
    }

    /// <inheritdoc/>
    public Task RemoveWorkspaceWebsiteAsync(string workspaceId, string websiteId, CancellationToken cancellationToken)
    {
        var deleteOperation = @"DELETE FROM Workspace_Websites WHERE WorkspaceId = @WorkspaceId AND WebsiteId = @WebsiteId;";

        return this.connection.ExecuteAsync(deleteOperation, new
        {
            WorkspaceId = workspaceId,
            WebsiteId = websiteId,
        });
    }

    /// <inheritdoc/>
    public Task RemoveWorkspaceYoutubeAsync(string workspaceId, string youtubeId, CancellationToken cancellationToken)
    {
        var deleteOperation = @"DELETE FROM Workspace_Youtube WHERE WorkspaceId = @WorkspaceId AND YoutubeId = @YoutubeId;";

        return this.connection.ExecuteAsync(deleteOperation, new
        {
            WorkspaceId = workspaceId,
            YoutubeId = youtubeId,
        });
    }

    /// <inheritdoc/>
    public Task UpdateAsync(Workspace workspace, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var updateOperation = @"
            UPDATE Workspaces
            SET
                Handle = @Handle,
                Name = @Name,
                Description = @Description,
                ModifiedAt = @ModifiedAt
            WHERE
                Id = @Id;
        ";

        return this.connection.ExecuteAsync(updateOperation, new
        {
            workspace.Handle,
            workspace.Name,
            workspace.Description,
            workspace.Id,
            ModifiedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task UpdateWorkspaceMembershipAsync(WorkspaceMembership workspaceMembership, CancellationToken cancellationToken)
    {
        var updateOperation = @"
            UPDATE Workspace_Memberships
            SET WorkspaceRole = @WorkspaceRole
            WHERE
                WorkspaceId = @WorkspaceId AND
                UserId = @UserId;
        ";

        return this.connection.ExecuteAsync(updateOperation, new
        {
            WorkspaceRole = workspaceMembership.WorkspaceRole.ToString(),
            workspaceMembership.WorkspaceId,
            workspaceMembership.UserId,
        });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<User>> GetWorkspaceMembersAsync(string workspaceId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var queryMemberships = "SELECT UserId FROM Workspace_Memberships WHERE WorkspaceId = @WorkspaceId;";

        var userIds = await this.connection
            .QueryAsync<string>(queryMemberships, new { WorkspaceId = workspaceId })
            .ConfigureAwait(false);

        var queryUser = "SELECT * FROM Users WHERE Id = @Id;";

        var userTasks = userIds.Select(id => this.connection.QueryFirstAsync<User>(queryUser, new { Id = id }));

        var users = await Task.WhenAll(userTasks).ConfigureAwait(false);

        return users;
    }
}
