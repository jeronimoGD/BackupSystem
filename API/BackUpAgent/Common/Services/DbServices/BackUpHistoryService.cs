using AutoMapper;
using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BackUpAgent.Common.Services.DbServices
{
    public class BackUpHistoryService : BaseEntityService<BackUpHistory>, IBackUpHistoryService
    {
        public BackUpHistoryService(IUnitOfWork unitOfWork, ILogger<BackUpHistoryService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }
    }
}
