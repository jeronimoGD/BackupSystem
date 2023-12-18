

using BackupSystem.Data.Entities;

namespace BackupSystem.Common.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        public IRepository<Agent> Agents { get; }
        public IRepository<BackUpConfiguration> BackUpConfigurations { get; }
        public IRepository<BackUpHistory> BackUpHistory { get; }
        public IRepository<ApplicationUser> ApplicationUsers { get; }
        public void Save();
    }
}