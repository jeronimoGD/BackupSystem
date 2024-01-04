using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace BackupSystem.Data.Entities
{
    [Index(nameof(AgentName), IsUnique = true)]
    public class Agent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   
        public Guid AgentKey { get; set; }
        [Required]
        public string AgentName { get; set; }
        [Required]
        public bool IsOnline { get; set; }
        public ICollection<BackUpConfiguration> BackUpConfigurations { get; set; }
        public ICollection<BackUpHistory> BackUpHistoryRecords{ get; set; }
    }
}
