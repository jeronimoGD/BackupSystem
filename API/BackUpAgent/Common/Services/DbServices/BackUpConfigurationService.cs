using AutoMapper;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Data.Entities;
using Microsoft.Extensions.Logging;

namespace BackUpAgent.Common.Services.DbServices
{
    public class BackUpConfigurationService : BaseEntityService<BackUpConfiguration>, IBackUpConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BackUpConfigurationService> _logger;

        public BackUpConfigurationService(IUnitOfWork unitOfWork, ILogger<BackUpConfigurationService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task UpdateConfigurations(List<BackUpConfiguration> backUpConfigurations)
        {
            try
            {
                await _unitOfWork.BackUpConfigurations.DeleteAll();
                
                foreach (BackUpConfiguration bc in backUpConfigurations)
                {
                    bc.Id = 0;
                    await _unitOfWork.BackUpConfigurations.Create(bc);
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
}
