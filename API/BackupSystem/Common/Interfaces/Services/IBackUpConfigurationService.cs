using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using BackupSystem.DTO.BackUpHistoryDTO;
using BackupSystem.DTO.BackUpHistoryDTOs;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Services
{
    public interface IBackUpConfigurationService : IBaseEntityService<BackUpConfiguration>
    {
    }
}
