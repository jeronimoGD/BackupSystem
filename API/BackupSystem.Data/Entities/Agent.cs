using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace BackupSystem.Data.Entities
{
    [Index(nameof(AgentName), IsUnique = true)]
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string AgentName { get; set; }
        [Required]
        public Guid ConnectionKey { get; set; }
        public bool IsOnline { get; set; } = false;

    }
}
