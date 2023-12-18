using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.LoginDTO;
using BackupSystem.DTO.RegisterDTO;
using BackupSystem.DTO.UserDTO;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Services
{
    public interface IUserService : IBaseEntityService<ApplicationUser>
    {
        Task<bool> DoesUserExists(string userName);
        Task<APIResponse> Register(RegisterRequestDTO registerRequestDTO);
        Task<APIResponse> LogIn(LoginRequestDTO loginRequestDTO);
        Task<APIResponse> UpdateUser(ApplicationUserUpdateDTO updateDTO, Expression<Func<ApplicationUser, bool>>? filtro = null);
    }
}
