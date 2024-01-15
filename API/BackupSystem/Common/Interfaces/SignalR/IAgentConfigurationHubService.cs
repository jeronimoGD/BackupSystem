using BackupSystem.Data.Entities;

namespace BackupSystem.Common.Interfaces.SignalR
{
    public interface IAgentConfigurationHubService
    {
        Task NotifyNewConfiguration(Guid connectionKey, string confName);
        Task NotifyConfigurationDeleted(Guid connectionKey, string confName);
        Task NotifyConfigurationUpdated(Guid connectionKey, string confName);
        Task NotifyAgentIsDeleted(Guid connectionKey);
    }
}
