using BackUpAgent.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces.ScheduledTasks
{
    public interface IBackUpScheduler
    {
        void AddBackgroundTask(BackUpConfiguration configuration);
        void CancelTask(string taskName);
        Task StartAllConfiguredBackUpsAsync(List<BackUpConfiguration> backUpConfigurations);
    }
}
