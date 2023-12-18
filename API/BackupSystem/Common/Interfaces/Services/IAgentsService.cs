using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.AgentDTOs;
using BackupSystem.DTO.GenericDTOs;
using BackupSystem.DTO.LoginDTO;
using BackupSystem.DTO.RegisterDTO;
using BackupSystem.DTO.UserDTO;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Services
{
    public interface IAgentsService : IBaseEntityService<Agent>
    {
        Task<APIResponse> GetOnlineAgents();
        Task<APIResponse> AddAgent(AgentCreateDTO createDto);
    }
}
