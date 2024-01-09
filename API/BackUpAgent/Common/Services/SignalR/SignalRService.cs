using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Common.Services.ScheduledTasks;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace BackUpAgent.Common.Services
{
    public class SignalRService : ISignalRService, IDisposable
    {
        private readonly IBackUpSystemApiRequestService _agentConnectionReqService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackUpConfigurationService _backUpConfigurationService;
        private HubConnection _hubConnection;
        private readonly IBackUpScheduler _backUpScheduler;
        private readonly AppSettings _appSettings;
        private Timer _keepAliveTimer;
        private readonly ILogger<SignalRService> _logger;

        public SignalRService(IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork, IBackUpScheduler backUpScheduler, IBackUpSystemApiRequestService agentConnectionReqService, IBackUpConfigurationService backUpConfigurationService, ILogger<SignalRService> logger)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _backUpScheduler = backUpScheduler;
            _agentConnectionReqService = agentConnectionReqService;
            _backUpConfigurationService = backUpConfigurationService;
            _logger = logger;
        }

        public void ConfigureHubConnection()
        {

            _logger.LogInformation($"Configuring Signal R Hub");
            _hubConnection = new HubConnectionBuilder()
                             .WithUrl(_appSettings.AgentManagerApiUrl+"/agentConfigurationHub?connectionKey=" + Guid.Parse(_appSettings.AgentConnectionKey))
                             .Build();

            SetNewConfigurationAvailableAction();
            SetConfigurationDeletedAction();
            SetPeriodicKeppALive();
        }

        public async Task StartAsync()
        {
            _logger.LogInformation($"Trying to Start hub connection!");

            try
            {
                await _hubConnection.StartAsync();
                _logger.LogInformation($"Hub connection Started!");
            }
            catch( Exception ex ) 
            {
                _logger.LogError("Error starting hub connection:", ex.ToString());            
            }
        }

        public async Task StopAsync()
        {

            _logger.LogInformation($"Stoping hub connection!");
            await _hubConnection.StopAsync();
        }

        private void SetNewConfigurationAvailableAction()
        {

            _hubConnection.On<string>("NewBackUpConfigurationAvaialable", async (confName) =>
            {
                _logger.LogInformation($"Asking for new configuration");

                APIResponse res = await _agentConnectionReqService.GetBackUpConfigurationByName<APIResponse>(confName);

                if (res.IsSuccesful)
                {
                    _logger.LogInformation($"New configuration received {res.Result}");
                    BackUpConfiguration newBackUpConfiguration = JsonConvert.DeserializeObject<BackUpConfiguration>(res.Result.ToString());
                    var doesBcConfigExist = (await _unitOfWork.BackUpConfigurations.Get(bc => bc.ConfigurationName == newBackUpConfiguration.ConfigurationName)).FirstOrDefault() != null;

                    if (!doesBcConfigExist)
                    {
                        newBackUpConfiguration.Id = 0;
                        await _unitOfWork.BackUpConfigurations.Create(newBackUpConfiguration);
                        _backUpScheduler.AddBackgroundTask(newBackUpConfiguration);
                    }
                    else
                    {
                        _logger.LogWarning($"Configuration with name {newBackUpConfiguration.ConfigurationName} already exist.");
                    }
                }
                else
                {
                    _logger.LogError(res.ErrorMessages);
                }
            });
        }

        private void SetConfigurationDeletedAction()
        {

            _hubConnection.On<string>("BackUpConfigurationDeleted", async (confName) =>
            {
                _logger.LogInformation($"Deleting {confName} back up configuration!");
                _backUpScheduler.CancelTask(confName);
                var deletedBc = await _backUpConfigurationService.Delete(bc => bc.ConfigurationName == confName);
            });
        }

        private void SetPeriodicKeppALive()
        {
            _keepAliveTimer = new Timer(async state =>
            {
                try
                {
                    await _hubConnection.InvokeAsync("ReceiveCheckAlive");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"Error al hacer ping al servidor: {ex.Message}");
                    StartAsync();
                }
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
        }

        public void Dispose()
        {
            _keepAliveTimer.Dispose();
        }
    }
}
