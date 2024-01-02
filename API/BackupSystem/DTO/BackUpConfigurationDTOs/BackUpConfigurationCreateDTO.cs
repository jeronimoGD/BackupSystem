using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.Anotations;
using BackupSystem.DTO.GenericDTOs;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackupSystem.DTO.BackUpConfigurationDTOs
{
    public class BackUpConfigurationCreateDTO : GenericCreateDTO, IMapFrom<BackUpConfiguration>
    {
        [Required]
        public string ConfigurationName { get; set; }
        [Required]
        public Guid AgentId { get; set; }
        public string TarjetDbName { get; set; }

        [EnumDataType(typeof(Periodicity), ErrorMessage = "Invalid periodicity, please select between these options (Daily, Weekly, Biweekl, Monthly)")]
        public Periodicity Periodicity { get; set; }

        [Required]
        public bool CleanEventTables { get; set; }
        [Required]
        public bool CreateCloudBackUp { get; set; }
        [Required]
        public bool StoreLastNBackUps { get; set; }
        public int LastNBackUps { get; set; }
    }

    public enum Periodicity
    {
        Daily,
        Weekly,
        TwoWeeks,
        Monthly
    }
}
