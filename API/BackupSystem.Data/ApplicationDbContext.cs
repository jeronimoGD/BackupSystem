using BackupSystem.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackupSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<BackUpConfiguration> BackUpConfigurations { get; set; }
        public DbSet<BackUpHistory> BackUpHistory { get; set; }
        public DbSet<ApplicationUser> AplicationUsers { get; set; }
    }
}
