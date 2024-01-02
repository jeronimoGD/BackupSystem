using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;

namespace BackupSystem.Common.Interfaces.Repository
{
    public interface IRepository<TEntity>
    {
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes);
        Task Create(TEntity entity);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
        Task Save();
    }
}
