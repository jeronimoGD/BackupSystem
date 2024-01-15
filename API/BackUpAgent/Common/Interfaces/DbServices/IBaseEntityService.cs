using System.Linq.Expressions;

namespace BackUpAgent.Common.Interfaces.DbServices
{
    public interface IBaseEntityService<TEntity>
    {
        Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes);
        Task Add(TEntity entity);
        Task<TEntity> Delete(Expression<Func<TEntity, bool>> filtro);
        Task<TEntity> Update(TEntity entityChanges, Expression<Func<TEntity, bool>> filtro);
        Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro);
    }
}
