using Microsoft.AspNetCore.SignalR;

namespace BackupSystem.Common.Hubs
{
    public class AgentConfigurationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;

            await Clients.All.SendAsync("ReceiveConfiguration", $"{Context.ConnectionId} has joined.");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;

            await Clients.All.SendAsync("ReceiveConfiguration", $"{Context.ConnectionId} has joined.");
        }

        public async Task SendConfiguration(string msg)
        {
            await Clients.All.SendAsync("ReceiveConfiguration", msg);
        }
    }
}
