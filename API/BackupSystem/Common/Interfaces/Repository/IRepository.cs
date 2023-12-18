using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Repository
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true);
        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filtro = null);
        Task Create(TEntity entity);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
        Task Save();
    }
}
