using BackUpAgent.Data.Entities;

namespace BackUpAgent.Common.Interfaces.DbServices
{
    public interface IBackUpConfigurationService : IBaseEntityService<BackUpConfiguration>
    {
        Task UpdateConfigurations(List<BackUpConfiguration> backUpConfigurations);
    }
}
