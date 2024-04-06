namespace Linker.Mvc.Hubs;

using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public Task SendMessage(string userId, string message)
    {
        return this.Clients.All.SendAsync("ReceiveMessage", userId, message);
    }
}
