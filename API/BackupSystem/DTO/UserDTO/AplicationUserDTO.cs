using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupSystem.DTO.UserDTO
{
    public class ApplicationUserDTO : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }
    }
}
