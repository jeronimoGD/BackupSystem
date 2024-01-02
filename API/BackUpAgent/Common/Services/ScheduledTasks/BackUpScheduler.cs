using BackUpAgent.Common.Interfaces.BackUpManaging;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Interfaces.ScheduledTasks;
using BackUpAgent.Common.Interfaces.Utils;
using BackUpAgent.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Services.ScheduledTasks
{
    public class BackUpScheduler : IHostedService, IDisposable, IBackUpScheduler
    {
        private readonly Dictionary<string, (Timer timer, CancellationTokenSource cancellationTokenSource)> _backUpTasksDictionary = new Dictionary<string, (Timer, CancellationTokenSource)>();
        private readonly object _backUpTasksLock = new object();
        private readonly IBackUpManager _backUpManager;
        private readonly IUtils _utils;
        public BackUpScheduler(IBackUpManager backUpManager, IUtils utils)
        {
            _backUpManager = backUpManager;
            _utils = utils;
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
            var timer = new Timer(state =>
            {
                Console.WriteLine($"[Back up :{configuration.ConfigurationName} ({DateTime.Now})]: Iniciando ejecucioon en segundo plano.");
                // TODO: _backUpManager.DoBackUp(configuration);
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            // TODO:  TimeSpan.FromDays(_utils.GetAmountOfDaysFromPeriodicity(configuration.Periodicity)));

            lock (_backUpTasksLock)
            {
                _backUpTasksDictionary.Add(configuration.ConfigurationName, (timer, cancellationTokenSource));
            }
        }

        public void CancelTask(string taskName)
        {
            // Cancelar una tarea en segundo plano específica
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
            // Detener todas las tareas en segundo plano
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
            // Liberar recursos si es necesario
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
}
