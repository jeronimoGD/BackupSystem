using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Interfaces.SignalR;
using BackupSystem.Data.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackupSystem.Common.Services.SignalR
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

        public async Task NotifyConfigurationDeleted(Guid connectionKey, string confName)
        {
            foreach (var connectionId in _signalRConnectionsManager.GetConnections(connectionKey))
            {
                _hubContext.Clients.Client(connectionId).SendAsync("BackUpConfigurationDeleted", confName);
            }
        }
        public async Task NotifyConfigurationUpdated(Guid connectionKey, string confName)
        {
            foreach (var connectionId in _signalRConnectionsManager.GetConnections(connectionKey))
            {
                _hubContext.Clients.Client(connectionId).SendAsync("BackUpConfigurationUpdated", confName);
            }
        }

        public async Task NotifyNewConfiguration(Guid connectionKey, string confName)
        {
            foreach (var connectionId in _signalRConnectionsManager.GetConnections(connectionKey))
            {
                _hubContext.Clients.Client(connectionId).SendAsync("NewBackUpConfigurationAvaialable", confName);
            }
        }

        public async Task NotifyAgentIsDeleted(Guid connectionKey)
        {
            foreach (var connectionId in _signalRConnectionsManager.GetConnections(connectionKey))
            {
                _hubContext.Clients.Client(connectionId).SendAsync("AgentDeleted");
            }
        }
    }
}
