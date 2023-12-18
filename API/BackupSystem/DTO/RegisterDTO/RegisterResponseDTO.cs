using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystem.DTO.RegisterDTO
{
    public class RegisterResponseDTO : IMapFrom<ApplicationUser>
    {
        public ApplicationUser CreatedUser { get; set; }
    }
}
