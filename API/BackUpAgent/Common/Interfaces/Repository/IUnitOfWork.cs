using BackUpAgent.Data.Entities;
namespace BackUpAgent.Common.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        public IRepository<BackUpConfiguration> BackUpConfigurations { get; }
        public IRepository<BackUpHistory> BackUpHistory { get; }
        public void Save();
    }
}