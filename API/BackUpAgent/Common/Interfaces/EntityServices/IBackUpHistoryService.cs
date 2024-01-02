using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;

namespace BackUpAgent.Common.Interfaces.NewFolder
{
    public interface IBackUpHistoryService : IBaseEntityService<BackUpHistory>
    {
        Task<APIResponse> AddConfigurationHistoryRecord(BackUpHistory createDto);
    }
}
