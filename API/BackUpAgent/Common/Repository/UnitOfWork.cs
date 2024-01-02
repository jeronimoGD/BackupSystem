using BackUpAgent.Common.Interfaces.Repository;
using BackUpAgent.Common.Repository;
using BackUpAgent.Data;
using BackUpAgent.Data.Entities;

namespace BackupSystem.Common.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<BackUpConfiguration> _backUpConfigurations { get; set; }
        private IRepository<BackUpHistory> _backUpHistory { get; set; }

        private readonly BackUpSystemDbContext _dbContext;

        public UnitOfWork(BackUpSystemDbContext context)
        {
            _dbContext = context;
        }

        public IRepository<BackUpConfiguration> BackUpConfigurations
        {
            get
            {
                return _backUpConfigurations == null ? _backUpConfigurations = new Repository<BackUpConfiguration>(_dbContext) : _backUpConfigurations;
            }
        }

        public IRepository<BackUpHistory> BackUpHistory
        {
            get
            {
                return _backUpHistory == null ? _backUpHistory = new Repository<BackUpHistory>(_dbContext) : _backUpHistory;
            }
        }
    
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
