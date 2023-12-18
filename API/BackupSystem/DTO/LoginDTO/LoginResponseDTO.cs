using BackupSystem.DTO.UserDTO;

namespace BackupSystem.DTO.LoginDTO
{
    public class LoginResponseDTO
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
