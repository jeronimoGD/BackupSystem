using BackupSystem.Data.Entities;

namespace BackupSystem.Common.Interfaces.Hubs
{
    public interface IAgentConfigurationHubService
    {
        Task NotifyNewConfiguration(Guid connectionKey, string confName);
    }
}
