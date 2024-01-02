using Azure;
using BackUpAgent.Common.Interfaces;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Data;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent
{
    public class StartUp : IStartUp
    {
        private readonly AppSettings _appSettings;
        private readonly IBackUpSystemApiRequestService _agentConnectionReqService;
        private readonly ISignalRService _signalRService;
        private readonly IBackUpScheduler _backUpScheduler;
        private readonly IBackUpConfigurationService _backUpConfigurationService;

        public StartUp(IOptions<AppSettings> appSettings, IBackUpSystemApiRequestService agentConnectionReqService, ISignalRService signalRService, IBackUpScheduler backUpScheduler, IBackUpConfigurationService backUpConfigurationService)
        {
            _appSettings = appSettings.Value;
            _agentConnectionReqService = agentConnectionReqService;
            _signalRService = signalRService;
            _backUpScheduler = backUpScheduler;
            _backUpConfigurationService = backUpConfigurationService;
        }

        public async Task StartAgentAsync()
        {

            Console.WriteLine($"Starting Agent {_appSettings.AgentConnectionKey}");
            Console.WriteLine($"Asking for authtorization!");

            APIResponse authorizationResponse= await _agentConnectionReqService.GetAuthorizationToConnect<APIResponse>(Guid.Parse(_appSettings.AgentConnectionKey));

            if (authorizationResponse.IsSuccesful)
            {
                Console.WriteLine($"Authorized to start agent!");
                Console.WriteLine($"Asking for configuration!");
                APIResponse configurationResponse = await _agentConnectionReqService.GetBackUpConfigurations<APIResponse>(Guid.Parse(_appSettings.AgentConnectionKey));
                
                if (configurationResponse.IsSuccesful)
                {
                    Console.WriteLine($"Configuration received");
                    try
                    {
                        configurationResponse.Result = JsonConvert.DeserializeObject<List<BackUpConfiguration>>(configurationResponse.Result.ToString());
                        List<BackUpConfiguration> backUpConfigurations = (List<BackUpConfiguration>)configurationResponse.Result;
                        await _backUpConfigurationService.UpdateConfigurations(backUpConfigurations);
                        _backUpScheduler.StartAllConfiguredBackUpsAsync(backUpConfigurations);

                        // Signal R
                        _signalRService.ConfigureHubConnection();
                        _signalRService.SetNewConfigurationAvailableAction();
                        _signalRService.StartAsync();
                        _signalRService.SetPeriodicKeppALive();

                        Console.WriteLine($"Press any key to stop agent!");
                        do
                        {

                        } while (Console.ReadKey().Key != ConsoleKey.Escape);

                        _signalRService.StopAsync();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {

                }
            }
            else
            {
                Console.WriteLine(authorizationResponse.ErrorMessages);
            }
        }
    }
}
