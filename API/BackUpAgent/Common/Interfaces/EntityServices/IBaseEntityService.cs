using System.Linq.Expressions;

namespace BackUpAgent.Common.Interfaces.NewFolder
{
    public interface IBaseEntityService<TEntity>
    {
        Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> Add(TEntity entity);
        // Task<TEntity> Add(GenericCreateDTO createDTO, Expression<Func<TEntity, bool>> filtro);
        Task<TEntity> Delete(Expression<Func<TEntity, bool>> filtro);
        Task<TEntity> Update(TEntity entity);
        // Task<TEntity> Update(GenericUpdateDTO updateDTO, Expression<Func<TEntity, bool>> filtro);
        Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro);

    }
}
