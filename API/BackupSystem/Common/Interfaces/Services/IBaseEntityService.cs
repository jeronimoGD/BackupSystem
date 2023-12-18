using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.DTO.GenericDTOs;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Services
{
    public interface IBaseEntityService<TEntity>
    {
        Task<APIResponse> GetAll();
        Task<APIResponse> Get(Expression<Func<TEntity, bool>> filtro);
        Task<APIResponse> Add(TEntity entity);
        Task<APIResponse> Add(GenericCreateDTO createDTO, Expression<Func<TEntity, bool>> checkIfEntityExistsFilter);
        Task<APIResponse> Delete(Expression<Func<TEntity, bool>> filtro);
        Task<APIResponse> Update(TEntity entity);
        Task<APIResponse> Update(GenericUpdateDTO updateDTO, Expression<Func<TEntity, bool>> checkIfEntityExistsFilter);
        Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro);

    }
}
