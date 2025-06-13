using Microsoft.AspNetCore.SignalR;

namespace Exercici5API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            var fullMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} > {user}: {message}";
            await Clients.All.SendAsync("ReceiveMessage", fullMessage);
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Nou client: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine($"Client desconnectat: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(ex);
        }

    }
}
