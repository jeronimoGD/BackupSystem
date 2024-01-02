using BackUpAgent.Common.Interfaces.DbServices;
using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Data.Entities;

namespace BackUpAgent.Common.Services.DbServices
{
    public class BackUpConfigurationService : BaseEntityService<BackUpConfiguration>, IBackUpConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BackUpConfigurationService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                //await _unitOfWork.BackUpConfigurations.CreateN(backUpConfigurations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
