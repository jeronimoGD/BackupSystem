using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.GenericDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystem.DTO.BackUpHistoryDTOs
{
    public class BackUpHistoryUpdateDTO : GenericUpdateDTO, IMapFrom<BackUpHistory> 
    {
        [Required]
        public string BackUpName { get; set; }
        public string BackUpSize { get; set; }
        public bool AvailableToDownload { get; set; }
    }
}
