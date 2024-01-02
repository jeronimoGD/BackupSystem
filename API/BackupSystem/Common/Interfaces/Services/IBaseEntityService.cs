using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.DTO.GenericDTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Services
{
    public interface IBaseEntityService<TEntity>
    {
        Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, params Expression<Func<TEntity, object>>[] includes);
        Task<APIResponse> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes);
        Task<APIResponse> Add(TEntity entity);
        Task<APIResponse> Add(GenericCreateDTO createDTO, Expression<Func<TEntity, bool>> filtro);
        Task<APIResponse> Delete(Expression<Func<TEntity, bool>> filtro);
        Task<APIResponse> Update(TEntity entity);
        Task<APIResponse> Update(GenericUpdateDTO updateDTO, Expression<Func<TEntity, bool>> filtro);
        Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro);

    }
}
