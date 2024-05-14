namespace Linker.Data.SqlServer;

using Dapper;
using EnsureThat;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

public sealed class ChatRepository : IChatRepository
{
    private readonly IDbConnection connection;

    public ChatRepository(IDbConnection connection)
    {
        this.connection = EnsureArg.IsNotNull(connection);
    }

    public Task SoftDeleteChatMessageAsync(string chatId, CancellationToken cancellationToken)
    {
        var softDeleteStatement = @"
            UPDATE ChatMessages
            SET
                IsDeleted = 1,
                ModifiedAt = GETUTCDATE()
            WHERE
                Id = @Id;
        ";

        return this.connection.ExecuteAsync(softDeleteStatement, new { Id = chatId });
    }

    public Task EditChatMessageAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        var statement = @"
            UPDATE ChatMessages
            SET
                Message = @Message,
                IsEdited = 1,
                ModifiedAt = GETUTCDATE()
            WHERE
                Id = @Id;
        ";

        return this.connection.ExecuteAsync(statement, new
        {
            message.Id,
            message.Message,
        });
    }

    public Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string workspaceId, CancellationToken cancellationToken)
    {
        var query = @"SELECT * FROM ChatMessages WHERE WorkspaceID = @WorkspaceId ORDER BY CreatedAt ASC;";

        return this.connection.QueryAsync<ChatMessage>(query, new { WorkspaceId = workspaceId });
    }

    public Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string workspaceId, int limit, CancellationToken cancellationToken)
    {
        var query = @"
            SELECT * FROM (
                SELECT TOP(@Limit) *
                FROM ChatMessages
                WHERE WorkspaceID = @WorkspaceId
                ORDER BY CreatedAt DESC
            ) LatestRecord ORDER BY CreatedAt ASC;
        ";

        return this.connection.QueryAsync<ChatMessage>(query, new { WorkspaceId = workspaceId, Limit = limit });
    }

    public Task InsertChatMessageAsync(ChatMessage message, CancellationToken cancellationToken)
    {
        var statement = @"
            INSERT INTO ChatMessages (
                Id,
                WorkspaceId,
                AuthorId,
                Message,
                IsEdited,
                IsDeleted,
                CreatedAt,
                ModifiedAt
            ) VALUES (
                @Id,
                @WorkspaceId,
                @AuthorId,
                @Message,
                @IsEdited,
                @IsDeleted,
                @CreatedAt,
                @ModifiedAt
            );
        ";

        return this.connection.ExecuteAsync(statement, message);
    }

    public Task HousekeepDeletedChatMessageOlderThan(TimeSpan timeSpan, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
