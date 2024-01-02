﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace BackUpAgent.Data.Entities
{
    [Index(nameof(BackUpName), IsUnique = true)]

    public class BackUpHistory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BackUpName { get; set; }
        public string BackUpSizeInMB { get; set; }
        public DateTime BuckUpDate { get; set; }
        public bool AvailableToDownload { get; set; }
    }
}
