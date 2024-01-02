using System.Linq.Expressions;

namespace BackUpAgent.Common.Interfaces.Repository
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes);
        Task Create(TEntity entity);
        Task CreateN(IEnumerable<TEntity> entity);
        Task Delete(TEntity entity);
        Task DeleteAll();
        Task Update(TEntity entity);
        Task Save();
    }
}
