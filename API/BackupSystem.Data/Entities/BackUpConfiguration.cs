using BackupSystem.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackupSystem.Data.Entities
{
    [Index(nameof(ConfigurationName), IsUnique = true)]
    public class BackUpConfiguration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ConfigurationName { get; set; }
        [Required]
        public string SourceDbName { get; set; }
        [Required]
        public string TarjetDbName { get; set; }
        [EnumDataType(typeof(Periodicity), ErrorMessage = "Invalid periodicity, please select between these options (Daily, Weekly, Biweekl, Monthly)")]
        public Periodicity Periodicity { get; set; }
        [Required]
        public bool CreateCloudBackUp { get; set; }
        [Required]
        public int LastNBackUpsToStore { get; set; }
        public string? ExcludedTablesJsonList { get; set; }

        [ForeignKey("AgentId")]
        public Guid AgentId { get; set; }
    }
}
