using BackupSystem.Common.Interfaces.Hubs;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Data.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackupSystem.Common.Hubs
{
    public class AgentConfigurationHubService : IAgentConfigurationHubService
    {
        private readonly ISignalRConnectionManager _signalRConnectionsManager;
        private IHubContext<AgentConfigurationHub> _hubContext;

        public AgentConfigurationHubService(IHubContext<AgentConfigurationHub> hubContext, ISignalRConnectionManager signalRConnectionsManager)
        {
            _hubContext = hubContext;
            _signalRConnectionsManager = signalRConnectionsManager;
           
        }

        public async Task NotifyNewConfiguration(Guid connectionKey, string confName)
        {
            foreach (var connectionId in _signalRConnectionsManager.GetConnections(connectionKey))
            {
                _hubContext.Clients.Client(connectionId).SendAsync("NewBackUpConfigurationAvaialable", confName);
            }
        }
    }
}
