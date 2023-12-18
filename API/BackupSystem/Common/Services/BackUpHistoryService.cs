using AutoMapper;
using BackupSystem.ApplicationSettings;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.AgentDTOs;
using BackupSystem.DTO.BackUpHistoryDTO;
using BackupSystem.DTO.BackUpHistoryDTOs;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace BackupSystem.Common.Services
{
    public class BackUpHistoryService : BaseEntityService<BackUpHistory>, IBackUpHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APISettings _apiSettings;

        public BackUpHistoryService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<APISettings> apiSettings) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiSettings = apiSettings.Value;
        }

        public async Task<APIResponse> AddConfigurationHistoryRecord(BackUpHistoryCreateDTO createDto)
        {
            APIResponse response = new APIResponse();

            try
            {
                if (!await DoesEntityExists(a => a.BackUpName == createDto.BackUpName))
                {
                    BackUpHistory newBackUpRecord= _mapper.Map<BackUpHistory>(createDto);
                    newBackUpRecord.BuckUpDate = DateTime.UtcNow;
                    await _unitOfWork.BackUpHistory.Create(newBackUpRecord);
                    response = APIResponse.Ok(newBackUpRecord);
                }
                else
                {
                    response = APIResponse.NotFound($"BackUp with this name already exists");
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
