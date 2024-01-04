using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.GenericDTOs;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.DTO.BackUpConfigurationDTOs
{
    public class BackUpConfigurationUpdateDTO : GenericUpdateDTO, IMapFrom<BackUpConfiguration>
    {
        [Required]
        public string ConfigurationName { get; set; }

        [Required]
        public Guid AgentId { get; set; }
        [Required]
        public string TarjetDbName { get; set; }
        [Required]
        public string SourceDbName { get; set; }
        public string PeriodicBackUpType { get; set; } // None, Daily, Bi-Week, Monthly
        [Required]
        public bool CreateCloudBackUp { get; set; }
        [Required]
        public bool StoreLastNBackUps { get; set; }
        public int? LastNBackUps { get; set; }
        public List<string>? ExcludedTablesList { get; set; }
    }
}
