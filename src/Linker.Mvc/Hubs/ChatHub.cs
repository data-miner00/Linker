namespace Linker.Mvc.Hubs;

using Linker.Core.V2.ApiModels;
using Linker.Core.V2.Models;
using Linker.Core.V2.Repositories;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private const string ServerIdentifier = "server";
    private readonly ConnectionManager connectionManager;
    private readonly IChatRepository repository;

    public sealed record EditChatMessage(string WorkspaceId, string ChatId, string UpdatedContent);

    public ChatHub(ConnectionManager connectionManager, IChatRepository repository)
    {
        ArgumentNullException.ThrowIfNull(connectionManager);
        ArgumentNullException.ThrowIfNull(repository);

        this.connectionManager = connectionManager;
        this.repository = repository;
    }

    public Task SendMessage(CreateChatMessage createMessage)
    {
        if (!this.connectionManager.Connections.TryGetValue(this.Context.ConnectionId, out var connection))
        {
            return Task.CompletedTask;
        }

        var message = new ChatMessage
        {
            Id = Guid.NewGuid().ToString(),
            AuthorId = createMessage.AuthorId,
            WorkspaceId = createMessage.WorkspaceId,
            Message = createMessage.Content,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            IsEdited = false,
        };

        IEnumerable<Task> tasks = [
            this.Clients.Group(connection.RoomId).SendAsync("ReceiveMessage", connection.Username, message),
            this.repository.InsertChatMessageAsync(message, default),
        ];

        return Task.WhenAll(tasks);
    }

    public Task JoinChat(ChatConnection chat)
    {
        return this.Clients.All.SendAsync("ReceiveGlobalMessage", ServerIdentifier, $"{chat.Username} has joined");
    }

    public async Task JoinSpecificChatRoom(ChatConnection chat)
    {
        this.KeepConnection(chat);

        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, chat.RoomId);
        await this.Clients.Group(chat.RoomId).SendAsync("JoinSpecificChatRoom", ServerIdentifier, $"{chat.Username} has joined {chat.RoomId}!");
    }

    public Task EditMessage(EditChatMessage edit)
    {
        if (!this.connectionManager.Connections.TryGetValue(this.Context.ConnectionId, out var connection))
        {
            return Task.CompletedTask;
        }

        var message = new ChatMessage
        {
            Id = edit.ChatId,
            Message = edit.UpdatedContent,
        };

        IEnumerable<Task> tasks = [
            this.repository.EditChatMessageAsync(message, default),
            this.Clients.Group(edit.WorkspaceId).SendAsync("EditMessage", message),
        ];

        return Task.WhenAll(tasks);
    }

    private void KeepConnection(ChatConnection connection)
    {
        this.connectionManager.Connections[this.Context.ConnectionId] = connection;
    }
}
