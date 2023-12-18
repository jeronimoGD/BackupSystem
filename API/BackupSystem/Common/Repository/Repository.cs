using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BackupSystem.Common.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<TEntity> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();
        }


        public async Task Create(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await Save();
        }

        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            await Save();
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            await Save();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
