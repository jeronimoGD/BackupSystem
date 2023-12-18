using System.ComponentModel.DataAnnotations;

namespace BackupSystem.DTO.RegisterDTO
{
    public class RegisterRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public List<string> Roles { get; set; }
    }
}
