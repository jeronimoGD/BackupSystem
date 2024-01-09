using BackUpAgent.Common.Interfaces;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BackUpAgent
{
    public class StartUp : IStartUp
    {
        private readonly AppSettings _appSettings;
        private readonly IBackUpSystemApiRequestService _agentConnectionReqService;
        private readonly ISignalRService _signalRService;
        private readonly IBackUpScheduler _backUpScheduler;
        private readonly IBackUpConfigurationService _backUpConfigurationService;
        private readonly ILogger<StartUp> _logger;

        public StartUp(IOptions<AppSettings> appSettings, IBackUpSystemApiRequestService agentConnectionReqService, ISignalRService signalRService, IBackUpScheduler backUpScheduler, IBackUpConfigurationService backUpConfigurationService, ILogger<StartUp> logger)
        {
            _appSettings = appSettings.Value;
            _agentConnectionReqService = agentConnectionReqService;
            _signalRService = signalRService;
            _backUpScheduler = backUpScheduler;
            _backUpConfigurationService = backUpConfigurationService;
            _logger = logger;
        }

        public async Task StartAgentAsync()
        {
            _logger.LogInformation($"Starting Agent with ID: {_appSettings.AgentConnectionKey}");
            _logger.LogInformation($"Asking for authtorization!");

            APIResponse authorizationResponse = await _agentConnectionReqService.GetAuthorizationToConnect<APIResponse>(Guid.Parse(_appSettings.AgentConnectionKey));

            if (authorizationResponse.IsSuccesful)
            {
                _logger.LogInformation($"Authorized to start agent!");
                _logger.LogInformation($"Asking for configuration!");
                APIResponse configurationResponse = await _agentConnectionReqService.GetBackUpConfigurations<APIResponse>(Guid.Parse(_appSettings.AgentConnectionKey));

                if (configurationResponse.IsSuccesful)
                {
                    _logger.LogInformation($"Configuration received");
                    try
                    {
                        configurationResponse.Result = JsonConvert.DeserializeObject<List<BackUpConfiguration>>(configurationResponse.Result.ToString());
                        List<BackUpConfiguration> backUpConfigurations = (List<BackUpConfiguration>)configurationResponse.Result;
                        await _backUpConfigurationService.UpdateConfigurations(backUpConfigurations);
                        
                        // Signal R
                        _signalRService.ConfigureHubConnection();
                        _signalRService.StartAsync();

                        _backUpScheduler.StartAllConfiguredBackUpsAsync(backUpConfigurations);

                        
                        do
                        {

                        } while (Console.ReadKey().Key != ConsoleKey.Escape);

                        _signalRService.StopAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
                else
                {

                }
            }
            else
            {
                _logger.LogError(authorizationResponse.ErrorMessages);
            }
        }
    }
}
