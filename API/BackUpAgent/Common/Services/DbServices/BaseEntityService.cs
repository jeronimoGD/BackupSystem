using AutoMapper.Internal;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackupSystem.Common.Repository;
using System.Linq.Expressions;
using System.Reflection;

namespace BackUpAgent.Common.Services.DbServices
{
    public class BaseEntityService<TEntity> : IBaseEntityService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;

        public BaseEntityService(IUnitOfWork unitOfWork)
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
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0,  params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> entities = null;

            try
            {
                entities = await _repository.Get(filtro, tracked, amountOfEntities,includes);

                if (entities == null)
                {
                    Console.WriteLine("Not found.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return entities;
        }

        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> entities = await _repository.Get(filtro, tracked, 1,includes);
            return entities.FirstOrDefault();
        }

        public async Task Delete(Expression<Func<TEntity, bool>> filtro)
        {

            try
            {
                var entityToDelete = await GetSingle(filtro);
                if (entityToDelete != null)
                {
                    await _repository.Delete(entityToDelete);
                }
                else
                {
                    Console.WriteLine("Not found.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public async Task Update(TEntity entity)
        {
            try
            {
                await _repository.Update(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
            }
        }

        public async Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro = null)
        {
            return await this.GetSingle(filtro) != null;
        }
    }
}
