using AutoMapper.Internal;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackupSystem.Common.Repository;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace BackUpAgent.Common.Services.DbServices
{
    public class BaseEntityService<TEntity> : IBaseEntityService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;
        private readonly ILogger<BaseEntityService<TEntity>> _logger;

        public BaseEntityService(IUnitOfWork unitOfWork, ILogger<BaseEntityService<TEntity>> logger)
        {
            _unitOfWork = unitOfWork;


            // Obtiene las propiedades del objeto
            PropertyInfo[] LasPropiedades = _unitOfWork.GetType().GetProperties();

            for (int i = 0; i < LasPropiedades.Length; i++)
            {
                var type = LasPropiedades[i].GetMemberType();
                if (type == typeof(IRepository<TEntity>))
                {
                    _repository = (IRepository<TEntity>?)LasPropiedades[i].GetValue(_unitOfWork);
                }
            }
            _logger = logger;
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0,  params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> entities = null;

            try
            {
                entities = await _repository.Get(filtro, tracked, amountOfEntities,includes);

                if (entities == null)
                {
                    _logger.LogInformation("Not found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return entities;
        }

        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> entities = await _repository.Get(filtro, tracked, 1,includes);
            return entities.FirstOrDefault();
        }

        public async Task<TEntity> Delete(Expression<Func<TEntity, bool>> filtro)
        {
            TEntity entity = null;
            try
            {
                var entityToDelete = await GetSingle(filtro);
                if (entityToDelete != null)
                {
                    await _repository.Delete(entityToDelete);
                    entity = entityToDelete;
                }
                else
                {
                    _logger.LogWarning("Not found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return entity;
        }

        public async Task Update(TEntity entity)
        {
            try
            {
                await _repository.Update(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public async Task Add(TEntity entity)
        {
            try
            {
                await _repository.Create(entity);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }

        public async Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro = null)
        {
            return await this.GetSingle(filtro) != null;
        }
    }
}
