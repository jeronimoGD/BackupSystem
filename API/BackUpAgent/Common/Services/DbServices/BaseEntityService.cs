using AutoMapper;
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
        private readonly IMapper _mapper;
        public BaseEntityService(IUnitOfWork unitOfWork, ILogger<BaseEntityService<TEntity>> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            PropertyInfo[] properties = _unitOfWork.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var type = properties[i].GetMemberType();
                if (type == typeof(IRepository<TEntity>))
                {
                    _repository = (IRepository<TEntity>?)properties[i].GetValue(_unitOfWork);
                }
            }

            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0,  params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> entities = null;

            try
            {
                entities = await _repository.Get(filtro, tracked, amountOfEntities,includes);

                if (entities == null)
                {
                    _logger.LogWarning("Unable to get entity. Entity not found.");
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
                    _logger.LogWarning("Unable to delete entity. Entity not found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return entity;
        }

        public async Task<TEntity> Update(TEntity entityChanges, Expression<Func<TEntity, bool>> filtro)
        {

            TEntity entity = null;

            try
            {
                var entityToUpdate = await GetSingle(filtro, true);
                if (entityToUpdate != null)
                {

                    _mapper.Map(entityChanges, entityToUpdate);
                    await _repository.Update(entityToUpdate);
                    entity = entityToUpdate;
                }
                else
                {
                    _logger.LogError($"Unable to update entity. Entity not found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return entity;
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
