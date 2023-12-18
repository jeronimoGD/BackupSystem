using AutoMapper;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.BackUpConfigurationDTOs;
using BackupSystem.DTO.BackUpHistoryDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Linq.Expressions;

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
    }
}
