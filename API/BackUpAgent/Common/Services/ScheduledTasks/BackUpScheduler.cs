using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.Utils;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BackUpAgent.Common.Services.ScheduledTasks
{
    public class BackUpScheduler : IHostedService, IDisposable, IBackUpScheduler
    {
        private readonly Dictionary<string, (Timer timer, CancellationTokenSource cancellationTokenSource)> _backUpTasksDictionary = new Dictionary<string, (Timer, CancellationTokenSource)>();
        private readonly object _backUpTasksLock = new object();
        private readonly IBackUpManager _backUpManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUtils _utils;
        private readonly ILogger<BackUpScheduler> _logger;

        public BackUpScheduler(IBackUpManager backUpManager, IUtils utils, IServiceProvider serviceProvider, ILogger<BackUpScheduler> logger)
        {
            _backUpManager = backUpManager;
            _utils = utils;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task StartAllConfiguredBackUpsAsync(List<BackUpConfiguration> backUpConfigurations)
        {
            foreach (var backUpConfiguration in backUpConfigurations)
            {
                AddBackgroundTask(backUpConfiguration);
            }
        }

        public void AddBackgroundTask(BackUpConfiguration configuration)
        {
            var cancellationTokenSource = new CancellationTokenSource();
          
            var timer = new Timer(
                state => BackUpTimersCallbackAsync((BackUpTimerCallbackParams)state), 
                new BackUpTimerCallbackParams { BackUpConfig = configuration},
                TimeSpan.Zero,
                TimeSpan.FromSeconds(_utils.GetAmountOfDaysFromPeriodicity(configuration.Periodicity))
            );
            // TODO:  TimeSpan.FromDays(_utils.GetAmountOfDaysFromPeriodicity(configuration.Periodicity)));

            lock (_backUpTasksLock)
            {
                _backUpTasksDictionary.Add(configuration.ConfigurationName, (timer, cancellationTokenSource));
            }
        }

        private async Task BackUpTimersCallbackAsync(BackUpTimerCallbackParams timerParams)
        {
            _logger.LogInformation($"Iniciando {timerParams.BackUpConfig.ConfigurationName} back up.");
            BackUpHistory bcRecord = await _backUpManager.DoBackUp(timerParams.BackUpConfig);
            
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var backUpHistoryService = scope.ServiceProvider.GetRequiredService<IBackUpHistoryService>();
                try
                {
                    await backUpHistoryService.Add(bcRecord);
                }
                catch(Exception ex) 
                {
                    _logger.LogWarning(ex.Message);
                }                
            }
            
          
            using (var scope = _serviceProvider.CreateScope())
            {
                var backUpSystemApiRequestService = scope.ServiceProvider.GetRequiredService<IBackUpSystemApiRequestService>();
                APIResponse res = await backUpSystemApiRequestService.RegisterBackUpHistoryRecord<APIResponse>(bcRecord);
                if (!res.IsSuccesful)
                {
                    _logger.LogError($"Error al registrar el back up en el historial general {res.ErrorMessages}.");
                }
            }
        }

        public void CancelTask(string taskName)
        {
            lock (_backUpTasksLock)
            {
                if (_backUpTasksDictionary.TryGetValue(taskName, out var task))
                {
                    task.cancellationTokenSource?.Cancel();
                    task.timer?.Change(Timeout.Infinite, 0);
                    task.timer?.Dispose();
                    _backUpTasksDictionary.Remove(taskName);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            lock (_backUpTasksLock)
            {
                foreach (var (timer, cancellationTokenSource) in _backUpTasksDictionary.Values)
                {
                    if (timer != null)
                    {
                        timer?.Change(Timeout.Infinite, 0);
                        cancellationTokenSource?.Cancel();
                        timer?.Dispose();
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            lock (_backUpTasksLock)
            {
                foreach (var (timer, cancellationTokenSource) in _backUpTasksDictionary.Values)
                {
                    timer?.Dispose();
                    cancellationTokenSource?.Dispose();
                }

                _backUpTasksDictionary.Clear();
            }
        }
    }

    public class BackUpTimerCallbackParams
    {
        public BackUpConfiguration BackUpConfig { get; set; }
    }
}
