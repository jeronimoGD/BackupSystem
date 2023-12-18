using BackupSystem.Common.Interfaces.Mapping;
using BackupSystem.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace BackupSystem.DTO.UserDTO
{
    public class ApplicationUserUpdateDTO : IMapFrom<ApplicationUser>
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword{ get; set; }
    }
}
