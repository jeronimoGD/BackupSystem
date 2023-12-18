using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Data;
using BackupSystem.Data.Entities;

namespace BackupSystem.Common.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<Agent> _agents { get; set; }
        private IRepository<BackUpConfiguration> _backUpConfigurations { get; set; }
        private IRepository<BackUpHistory> _backUpHistory { get; set; }
        private IRepository<ApplicationUser> _applicationUsers { get; set; }

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public IRepository<Agent> Agents
        {
            get
            {
                return _agents == null ? _agents = new Repository<Agent>(_dbContext) : _agents;
            }
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

        public IRepository<ApplicationUser> ApplicationUsers
        {
            get
            {
                return _applicationUsers == null ? _applicationUsers = new Repository<ApplicationUser>(_dbContext) : _applicationUsers;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
