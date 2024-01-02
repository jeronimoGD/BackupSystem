using BackupSystem.Common.Interfaces.Hubs;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Common.Services;
using BackupSystem.Data.Entities;
using BackupSystem.Data.Migrations;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackupSystem.Common.Hubs
{
    public class AgentConfigurationHub : Hub
    {
        private readonly IAgentsService _agentService;
        private readonly ISignalRConnectionManager _signalRConnectionsManager;
        private readonly ICheckAliveTimeoutsManager _checkAliveTimeoutsManager;

        private readonly Dictionary<Guid, Timer> _checkAliveTimouts = new Dictionary<Guid, Timer>();

        public AgentConfigurationHub(IAgentsService agentService, ISignalRConnectionManager signalRConnectionsManager, ICheckAliveTimeoutsManager checkAliveTimeoutsManager)
        {
            _agentService = agentService;
            _signalRConnectionsManager = signalRConnectionsManager;
            _checkAliveTimeoutsManager = checkAliveTimeoutsManager;
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Guid clientKey = Guid.Parse(Context.GetHttpContext().Request.Query["connectionKey"]);

            if (clientKey != null)
            {
                _signalRConnectionsManager.Add(clientKey, connectionId);
                await _agentService.SetOnlineStatus(clientKey, true);

                _checkAliveTimeoutsManager.AddTimer(
                                          clientKey,
                                          state => TimerCallbackAsync((TimerCallbackParams)state),
                                          new TimerCallbackParams { clientKey = clientKey, connectionId = connectionId},
                                          TimeSpan.FromSeconds(30),
                                          TimeSpan.FromSeconds(30)
                 );
            }
        }

        private async Task TimerCallbackAsync(TimerCallbackParams timerParams)
        {
            await _agentService.SetOnlineStatus(timerParams.clientKey, false);
            _signalRConnectionsManager.Remove(timerParams.clientKey, timerParams.connectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            Guid clientKey = Guid.Parse(Context.GetHttpContext().Request.Query["connectionKey"]);

            if (clientKey != null)
            {
                _signalRConnectionsManager.Remove(clientKey, connectionId);
                _checkAliveTimeoutsManager.CancelTimer(clientKey);
                var agent = await _agentService.SetOnlineStatus(clientKey, false);
            }
        }

        public async Task ReceiveCheckAlive()
        {
            string connectionId = Context.ConnectionId;
            Guid connectionKey = _signalRConnectionsManager.GetConnectionKey(connectionId);

            _checkAliveTimeoutsManager.ResetTimer(connectionKey, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
            await _agentService.SetOnlineStatus(connectionKey, true);
        }
    }
}
