using AutoMapper;
using AutoMapper.Internal;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.AgentDTOs;
using BackupSystem.DTO.GenericDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Formats.Tar;
using System.Linq.Expressions;
using System.Reflection;

namespace BackupSystem.Common.Services.DbManagementServices
{
    public class BaseEntityService<TEntity> : IBaseEntityService<TEntity> where TEntity : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;
        private IMapper _mapper;

        public BaseEntityService(IUnitOfWork unitOfWork, IMapper mapper)
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
            _mapper = mapper;
        }

        public async Task<APIResponse> Get(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, int amountOfEntities = 0, params Expression<Func<TEntity, object>>[] includes)
        {
            APIResponse response = new APIResponse();

            try
            {
                IEnumerable<TEntity> entities = await _repository.Get(filtro, tracked, amountOfEntities, includes);

                if (entities != null && entities.Count() > 0)
                {
                    if (amountOfEntities == 1)
                    {
                        response = APIResponse.Ok(entities.FirstOrDefault());
                    }
                    else
                    {
                        response = APIResponse.Ok(entities);
                    }
                }
                else
                {
                    response = APIResponse.NotFound("Not found.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> filtro = null, bool tracked = true, params Expression<Func<TEntity, object>>[] includes)
        {
            IEnumerable<TEntity> entities = await _repository.Get(filtro, tracked, 1, includes);
            return entities.FirstOrDefault();
        }

        public async Task<APIResponse> Delete(TEntity entity)
        {
            APIResponse response = new APIResponse();

            try
            {
                await _repository.Delete(entity);
                response = APIResponse.Ok(entity);
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Delete(Expression<Func<TEntity, bool>> filtro)
        {
            APIResponse response = new APIResponse();

            try
            {
                var entityToDelete = await GetSingle(filtro);
                if (entityToDelete != null)
                {
                    await _repository.Delete(entityToDelete);
                    response = APIResponse.Ok(entityToDelete);
                }
                else
                {
                    response = APIResponse.NotFound($"Unable to delete entity. Entity not found.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Update(TEntity entity)
        {
            APIResponse response = new APIResponse();

            try
            {
                await _repository.Update(entity);
                response = APIResponse.Ok(entity);
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Add(TEntity entity)
        {
            APIResponse response = new APIResponse();

            try
            {
                await _repository.Create(entity);
                response = APIResponse.Created(entity);
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Add(GenericCreateDTO createDTO, Expression<Func<TEntity, bool>> filtro)
        {
            APIResponse response = new APIResponse();

            try
            {
                if (!await DoesEntityExists(filtro))
                {
                    TEntity newEntity = _mapper.Map<TEntity>(createDTO);
                    await _repository.Create(newEntity);
                    response = APIResponse.Created(newEntity);
                }
                else
                {
                    response = APIResponse.NotFound($"Unable to create entity. Entity already exists.");
                }

            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Update(GenericUpdateDTO updateDTO, Expression<Func<TEntity, bool>> filtro)
        {
            APIResponse response = new APIResponse();

            try
            {
                var entityToUpdate = await GetSingle(filtro, false);
                if (entityToUpdate != null)
                {

                    _mapper.Map(updateDTO, entityToUpdate);
                    await _repository.Update(entityToUpdate);
                    response = APIResponse.Ok(entityToUpdate);
                }
                else
                {
                    response = APIResponse.NotFound($"Unable to update entity. Entity not found.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<bool> DoesEntityExists(Expression<Func<TEntity, bool>> filtro = null)
        {
            return await GetSingle(filtro) != null;
        }
    }
}
