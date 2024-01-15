using BackUpAgent.Common.Interfaces;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Common.Services.ApiRequest.DTO;
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
        private readonly IBackUpSystemApiRequestService _backUpSystemApiRequestService;
        private readonly ISignalRService _signalRService;
        private readonly IBackUpScheduler _backUpScheduler;
        private readonly IBackUpConfigurationService _backUpConfigurationService;
        private readonly ILogger<StartUp> _logger;

        public StartUp(IOptions<AppSettings> appSettings, IBackUpSystemApiRequestService backUpSystemApiRequestService, ISignalRService signalRService, IBackUpScheduler backUpScheduler, IBackUpConfigurationService backUpConfigurationService, ILogger<StartUp> logger)
        {
            _appSettings = appSettings.Value;
            _backUpSystemApiRequestService = backUpSystemApiRequestService;
            _signalRService = signalRService;
            _backUpScheduler = backUpScheduler;
            _backUpConfigurationService = backUpConfigurationService;
            _logger = logger;
        }

        public async Task StartAgentAsync()
        {
            _logger.LogInformation($"Starting Agent with ID: {_appSettings.AgentConnectionKey}.");

            _logger.LogInformation($"Logging to API with URL: {_appSettings.AgentManagerApiUrl}.");

            APIResponse logingResponse = await _backUpSystemApiRequestService.APILoging<APIResponse>();
            
            if (logingResponse.IsSuccesful)
            {
                LoginResponseDTO logingsResDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(logingResponse.Result.ToString());
                _backUpSystemApiRequestService.SetSessionToken(logingsResDTO.Token);
                _logger.LogInformation($"User {logingsResDTO} logged in.");
                _logger.LogInformation($"Asking for authtorization.");

                APIResponse authorizationResponse = await _backUpSystemApiRequestService.GetAuthorizationToConnect<APIResponse>(Guid.Parse(_appSettings.AgentConnectionKey));

                if (authorizationResponse.IsSuccesful)
                {
                    _logger.LogInformation($"Authorized to start agent.");
                    _logger.LogInformation($"Asking for configuration.");
                    APIResponse configurationResponse = await _backUpSystemApiRequestService.GetBackUpConfigurations<APIResponse>(Guid.Parse(_appSettings.AgentConnectionKey));

                    if (configurationResponse.IsSuccesful)
                    {
                        _logger.LogInformation($"Configuration received.");
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
                        _logger.LogError(configurationResponse.ErrorMessages);
                    }
                }
                else
                {
                    _logger.LogError(authorizationResponse.ErrorMessages);
                }
            }
            else
            {
                _logger.LogError(logingResponse.ErrorMessages);
            }
        }
    }
}
