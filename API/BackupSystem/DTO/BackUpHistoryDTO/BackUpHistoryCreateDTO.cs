using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using BackupSystem.DTO.GenericDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystem.DTO.BackUpHistoryDTO
{
    public class BackUpHistoryCreateDTO : GenericCreateDTO ,IMapFrom<BackUpHistory>
    {
        [Required]
        public string BackUpName { get; set; }
        public string BackUpSizeInMB { get; set; }
        public bool AvailableToDownload { get; set; }
    }
}
