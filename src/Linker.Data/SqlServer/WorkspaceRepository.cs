namespace Linker.Data.SqlServer;

using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
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
    public Task AddWorkspaceMembershipAsync(WorkspaceMembership membership, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var addWorkspaceMembership = @"
            INSERT INTO WorkspaceMemberships (
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
            INSERT INTO WorkspaceMemberships (
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
        var deleteOperation = "DELETE FROM WorkspaceMemberships WHERE WorkspaceId = @WorkspaceId AND UserId = @UserId;";

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

        var getMemberships = "SELECT WorkspaceId FROM WorkspaceMemberships WHERE UserId = @UserId;";

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
            queryBuilder.Append($"Id = '{workspaceId}' OR ");
        }

        var getWorkspaces = $"SELECT * FROM Workspaces WHERE {queryBuilder}Id = '{workspaceIds.Last()}';";

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
                SELECT WorkspaceId FROM WorkspaceMemberships
                WHERE UserId = @UserId
            );
        ";

        return this.connection.QueryAsync<Workspace>(query, new { UserId = userId });
    }

    /// <inheritdoc/>
    public Task<IEnumerable<WorkspaceMembership>> GetAllWorkspaceMembershipsAsync(string id, CancellationToken cancellationToken)
    {
        var query = @"SELECT * FROM WorkspaceMemberships WHERE WorkspaceId = @WorkspaceId;";

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
    public Task<WorkspaceMembership> GetWorkspaceMembershipAsync(string workspaceId, string userId, CancellationToken cancellationToken)
    {
        var query = @"
            SELECT * FROM WorkspaceMemberships
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
    public async Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        cancellationToken.ThrowIfCancellationRequested();

        var deleteWorkspaceOperation = @"DELETE FROM Workspaces WHERE Id = @Id;";
        var deleteWorkspaceMember = @"DELETE FROM WorkspaceMemberships WHERE WorkspaceId = @Id;";
        var deleteWorkspaceLink = @"DELETE FROM WorkspaceLinks WHERE WorkspaceId = @Id;";

        IEnumerable<Task> tasks = [
            this.connection.ExecuteAsync(deleteWorkspaceMember, new { Id = id }),
            this.connection.ExecuteAsync(deleteWorkspaceLink, new { Id = id }),
        ];

        await Task.WhenAll(tasks).ConfigureAwait(false);
        await this.connection
            .ExecuteAsync(deleteWorkspaceOperation, new { Id = id })
            .ConfigureAwait(false);
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
            UPDATE WorkspaceMemberships
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

        var queryMemberships = "SELECT UserId FROM WorkspaceMemberships WHERE WorkspaceId = @WorkspaceId;";

        var userIds = await this.connection
            .QueryAsync<string>(queryMemberships, new { WorkspaceId = workspaceId })
            .ConfigureAwait(false);

        var queryUser = "SELECT * FROM Users WHERE Id = @Id;";

        var userTasks = userIds.Select(id => this.connection.QueryFirstAsync<User>(queryUser, new { Id = id }));

        var users = await Task.WhenAll(userTasks).ConfigureAwait(false);

        return users;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Link>> GetWorkspaceLinksAsync(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var query = @"
            SELECT LinkId FROM WorkspaceLinks
            WHERE WorkspaceId = @Id;";

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

        var queryLinks = $"SELECT * FROM Links WHERE {queryBuilder}Id = '{linkIds.Last()}';";

        var links = await this.connection.QueryAsync<Link>(queryLinks).ConfigureAwait(false);

        var selectFromLinksTagsQuery = @"SELECT * FROM LinksTags WHERE LinkId = @LinkId;";
        var selectFromTagsQuery = @"SELECT * FROM Tags WHERE Id = @Id;";

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
    public Task AddWorkspaceLinkAsync(string workspaceId, string linkId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var addWorkspaceLink = @"
            INSERT INTO WorkspaceLinks (
                WorkspaceId,
                LinkId,
                CreatedAt
            ) VALUES (
                @WorkspaceId,
                @LinkId,
                @CreatedAt
            );";

        return this.connection.ExecuteAsync(addWorkspaceLink, new
        {
            WorkspaceId = workspaceId,
            LinkId = linkId,
            CreatedAt = DateTime.Now,
        });
    }

    /// <inheritdoc/>
    public Task RemoveWorkspaceLinkAsync(string workspaceId, string linkId, CancellationToken cancellationToken)
    {
        var deleteOperation = @"DELETE FROM WorkspaceLinks WHERE WorkspaceId = @WorkspaceId AND LinkId = @LinkId;";

        return this.connection.ExecuteAsync(deleteOperation, new
        {
            WorkspaceId = workspaceId,
            LinkId = linkId,
        });
    }
}
