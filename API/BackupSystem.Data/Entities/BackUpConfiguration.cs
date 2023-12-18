using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.Data.Entities
{
    [Index(nameof(ConfigurationName), IsUnique = true)]
    public class BackUpConfiguration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ConfigurationName { get; set; }
        public string TarjetDbName { get; set; }
        public string PeriodicBackUpType { get; set; } // None, Daily, Bi-Week, Monthly
        [Required]
        public bool CleanEventTables { get; set; }
        [Required]
        public bool CreateCloudBackUp { get; set; }
        [Required]
        public bool StoreLastNBackUps { get; set; }
        public int LastNBackUps { get; set; }
    }
}
