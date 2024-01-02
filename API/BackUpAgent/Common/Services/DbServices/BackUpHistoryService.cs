using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Extensions.Options;

namespace BackUpAgent.Common.Services.DbServices
{
    public class BackUpHistoryService : BaseEntityService<BackUpHistory>, IBackUpHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public BackUpHistoryService(IUnitOfWork unitOfWork, IOptions<AppSettings> apiSettings) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appSettings = apiSettings.Value;
        }
    }
}
