using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Data;
using BackupSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes)
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

            if (includes.Count() > 0)
            {
                query = includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            if (amountOfEntities > 0)
            {
                query = query.Take(amountOfEntities);
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
