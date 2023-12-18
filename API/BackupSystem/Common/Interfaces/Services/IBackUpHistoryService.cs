using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.AgentDTOs;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using BackupSystem.DTO.BackUpHistoryDTO;
using BackupSystem.DTO.BackUpHistoryDTOs;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Services
{
    public interface IBackUpHistoryService : IBaseEntityService<BackUpHistory>
    {
        Task<APIResponse> AddConfigurationHistoryRecord(BackUpHistoryCreateDTO createDto);
    }
}
