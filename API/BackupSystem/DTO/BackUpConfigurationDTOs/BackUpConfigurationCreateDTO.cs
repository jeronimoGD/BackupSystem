using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.GenericDTOs;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.DTO.BackUpConfigurationDTOs
{
    public class BackUpConfigurationCreateDTO : GenericCreateDTO, IMapFrom<BackUpConfiguration>
    {
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
