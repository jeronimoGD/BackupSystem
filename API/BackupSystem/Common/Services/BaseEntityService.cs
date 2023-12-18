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
using System.Formats.Tar;
using System.Linq.Expressions;
using System.Reflection;

namespace BackupSystem.Common.Services
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

        public async Task<APIResponse> GetAll()
        {
            APIResponse response = new APIResponse();

            try
            {
                IEnumerable<object> users = await _repository.GetAll();

                if (users != null && users.Count() > 0)
                {
                    response = APIResponse.Ok(users);
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

        public async Task<APIResponse> Delete(Expression<Func<TEntity, bool>> filtro)
        {
            APIResponse response = new APIResponse();

            try
            {
                var userToDelete = await _repository.Get(filtro);

                if (userToDelete != null)
                {
                    await _repository.Delete(userToDelete);
                    response = APIResponse.NoContent();
                }
                else
                {
                    response = APIResponse.NotFound($"Not found");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Get(Expression<Func<TEntity, bool>> filtro)
        {
            APIResponse response = new APIResponse();

            try
            {
                var usrByName = await _repository.Get(filtro);

                if (usrByName != null)
                {
                    response = APIResponse.Ok(usrByName);
                }
                else
                {
                    response = APIResponse.NotFound($"Not found.");
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
        public async Task<APIResponse> Add(GenericCreateDTO createDTO, Expression<Func<TEntity, bool>> checkIfEntityExistsFilter)
        {
            APIResponse response = new APIResponse();

            try
            {
                if(!await DoesEntityExists(checkIfEntityExistsFilter))
                {
                    TEntity newEntity = _mapper.Map<TEntity>(createDTO);
                    await _repository.Create(newEntity);
                    response = APIResponse.Created(newEntity);
                }
                else
                {
                    response = APIResponse.NotFound($"Entity already exists");
                }
               
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> Update(GenericUpdateDTO updateDTO, Expression<Func<TEntity, bool>> checkIfEntityExistsFilter)
        {
            APIResponse response = new APIResponse();

            try
            {
                var userToUpdate = await _repository.Get(checkIfEntityExistsFilter, false);

                if (userToUpdate != null)
                {
                    TEntity newEntityData = _mapper.Map<TEntity>(updateDTO);
                    PropertyInfo idProperty = userToUpdate.GetType().GetProperty("Id");
                    newEntityData.GetType().GetProperty("Id")?.SetValue(newEntityData, idProperty.GetValue(userToUpdate));
                    await _repository.Update(newEntityData);
                    response = APIResponse.Ok(newEntityData);
                }
                else
                {
                    response = APIResponse.NotFound($"Entity not found");
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
            return await _repository.Get(filtro) != null;
        }
    }
}
