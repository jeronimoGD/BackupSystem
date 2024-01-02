using BackUpAgent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Data
{
    public class BackUpSystemDbContext : DbContext
    {
        public BackUpSystemDbContext(DbContextOptions<BackUpSystemDbContext> options) : base(options)
        {
        }
        public DbSet<BackUpConfiguration> BackUpConfigurations { get; set; }
        public DbSet<BackUpHistory> BackUpHistory { get; set; }
    }
}
