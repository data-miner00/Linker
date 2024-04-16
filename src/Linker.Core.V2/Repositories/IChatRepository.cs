namespace Linker.Core.V2.Repositories;

using Linker.Core.V2.Models;
using System;

public interface IChatRepository
{
    Task InsertChatMessageAsync(ChatMessage message, CancellationToken cancellationToken);

    Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string workspaceId, CancellationToken cancellationToken);

    Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(string workspaceId, int limit, CancellationToken cancellationToken);

    Task EditChatMessageAsync(ChatMessage message, CancellationToken cancellationToken);

    Task SoftDeleteChatMessageAsync(string chatId, CancellationToken cancellationToken);

    Task HousekeepDeletedChatMessageOlderThan(TimeSpan timeSpan, CancellationToken cancellationToken); 
}
