using BackUpAgent.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces.SignalR
{
    public interface ISignalRService
    {
        void ConfigureHubConnection();
        Task StartAsync();
        Task StopAsync();
    }
}
