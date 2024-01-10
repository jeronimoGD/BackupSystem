using AutoMapper;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using BackupSystem.DTO.BackUpHistoryDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace BackupSystem.Common.Services
{
    public class BackUpConfigurationService : BaseEntityService<BackUpConfiguration>, IBackUpConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public BackUpConfigurationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse> AddBackUpConfiguration(BackUpConfigurationCreateDTO createDto)
        {
            APIResponse response = new APIResponse();

            try
            {
                if (!await DoesEntityExists(a => a.ConfigurationName == createDto.ConfigurationName))
                {
                    BackUpConfiguration newBackUpConf = _mapper.Map<BackUpConfiguration>(createDto);
                    newBackUpConf.ExcludedTablesJsonList = createDto.ExcludedTablesList == null ? null : JsonConvert.SerializeObject(createDto.ExcludedTablesList);
                    await _unitOfWork.BackUpConfigurations.Create(newBackUpConf);
                    response = APIResponse.Ok(newBackUpConf);
                }
                else
                {
                    response = APIResponse.NotFound($"BackUp configuration with this name already exists");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }
    }
}
