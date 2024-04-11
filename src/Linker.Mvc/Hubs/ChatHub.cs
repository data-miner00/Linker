namespace Linker.Mvc.Hubs;

using Linker.Core.V2.Models;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private const string ServerIdentifier = "server";
    private readonly ConnectionManager connectionManager;

    public ChatHub(ConnectionManager connectionManager)
    {
        this.connectionManager = connectionManager;
    }

    public Task SendMessage(string message)
    {
        if (!this.connectionManager.Connections.TryGetValue(this.Context.ConnectionId, out var connection))
        {
            return Task.CompletedTask;
        }

        return this.Clients.Group(connection.RoomId).SendAsync("ReceiveMessage", connection.Username, message);
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

    private void KeepConnection(ChatConnection chat)
    {
        this.connectionManager.Connections[this.Context.ConnectionId] = chat;
    }
}
