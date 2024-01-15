using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.Data.Enums;
using BackupSystem.DTO.GenericDTOs;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.DTO.BackUpConfigurationDTOs
{
    public class BackUpConfigurationCreateDTO : GenericCreateDTO, IMapFrom<BackUpConfiguration>
    {
        [Required]
        public string ConfigurationName { get; set; }
        [Required]  
        public Guid AgentId { get; set; }
        [Required]
        public string TarjetDbName { get; set; }
        [Required]
        public string SourceDbName { get; set; }
        [EnumDataType(typeof(Periodicity), ErrorMessage = "Invalid periodicity, please select between these options (Daily, Weekly, Biweekl, Monthly)")]
        public Periodicity Periodicity { get; set; }
        [Required]
        public bool CreateCloudBackUp { get; set; }
        [Required]
        public int LastNBackUpsToStore { get; set; }
        public List<string>? ExcludedTablesList { get; set; }
    }
}
