using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.SignalR;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.AspNetCore.SignalR.Client;
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
        private HubConnection _hubConnection;
        private readonly IBackUpScheduler _backUpScheduler;
        private readonly AppSettings _appSettings;
        private Timer _keepAliveTimer;

        public SignalRService(IOptions<AppSettings> appSettings, IUnitOfWork unitOfWork, IBackUpScheduler backUpScheduler, IBackUpSystemApiRequestService agentConnectionReqService)
        {
            _appSettings = appSettings.Value;
            _unitOfWork = unitOfWork;
            _backUpScheduler = backUpScheduler;
            _agentConnectionReqService = agentConnectionReqService;
        }

        public void ConfigureHubConnection()
        {

            Console.WriteLine($"Configuring Signal R Hub");
            _hubConnection = new HubConnectionBuilder()
                             .WithUrl(_appSettings.AgentManagerApiUrl+"/agentConfigurationHub?connectionKey=" + Guid.Parse(_appSettings.AgentConnectionKey))
                             .Build();
        }

        public async Task StartAsync()
        {
            Console.WriteLine($"Trying to Start hub connection!");

            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine($"Hub connection Started!");
            }
            catch( Exception ex ) 
            {
                Console.WriteLine("Error starting hub connection:", ex.ToString());            
            }
        }

        public async Task StopAsync()
        {

            Console.WriteLine($"Stoping hub connection!");
            await _hubConnection.StopAsync();
        }

        public void SetNewConfigurationAvailableAction()
        {

            _hubConnection.On<string>("NewBackUpConfigurationAvaialable", async (confName) =>
            {
                Console.WriteLine($"Asking for new configuration");

                APIResponse res = await _agentConnectionReqService.GetBackUpConfigurationByName<APIResponse>(confName);

                if (res.IsSuccesful)
                {
                    Console.WriteLine($"New configuration received {res.Result}");
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
                        Console.WriteLine($"Configuration with name {newBackUpConfiguration.ConfigurationName} already exist.");
                    }
                }
                else
                {
                    Console.WriteLine(res.ErrorMessages);
                }
            });
        }

        public void SetPeriodicKeppALive()
        {
            _keepAliveTimer = new Timer(async state =>
            {
                try
                {
                    await _hubConnection.InvokeAsync("ReceiveCheckAlive");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al hacer ping al servidor: {ex.Message}");
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
