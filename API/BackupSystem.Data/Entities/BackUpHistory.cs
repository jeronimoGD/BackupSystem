using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;

namespace BackupSystem.Data.Entities
{
    [Index(nameof(BackUpName), IsUnique = true)]

    public class BackUpHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BackUpName { get; set; }
        public string BackUpPath { get; set; }
        public bool IsSuccessfull { get; set; }
        public double BackUpSizeInMB { get; set; }
        [Required]
        public DateTime BuckUpDate { get; set; }
        public bool AvailableToDownload { get; set; }
        public string Description { get; set; }
        [ForeignKey("AgentId")]
        public Guid AgentId { get; set; }
    }
}
