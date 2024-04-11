namespace Linker.Mvc.Hubs;

using Linker.Core.V2.Models;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private const string ServerIdentifier = "server";

    public Task SendMessage(string userId, string message)
    {
        return this.Clients.All.SendAsync("ReceiveMessage", userId, message);
    }

    public Task JoinChat(ChatConnection chat)
    {
        return this.Clients.All.SendAsync("ReceiveMessage", ServerIdentifier, $"{chat.Username} has joined");
    }

    public async Task JoinSpecificChatRoom(ChatConnection chat)
    {
        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, chat.RoomId);
        await this.Clients.Group(chat.RoomId).SendAsync("JoinSpecificChatRoom", ServerIdentifier, $"{chat.Username} has joined {chat.RoomId}!");
    }
}
