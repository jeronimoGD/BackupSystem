using BackUpAgent.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BackUpAgent.Data.Entities
{
    [Index(nameof(ConfigurationName), IsUnique = true)]
    public class BackUpConfiguration
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ConfigurationName { get; set; }
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
}
