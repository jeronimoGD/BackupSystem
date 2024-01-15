using BackupSystem.Common.Constants;
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
        [RoleValidation]
        public string Role { get; set; }
    }

    public class RoleValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rolesConstantesType = typeof(DefaultRoles);
            var defaultRoles = rolesConstantesType.GetFields()
                .Where(f => f.IsLiteral && !f.IsInitOnly)
                .Select(f => f.GetValue(null).ToString())
                .ToArray();

            if (value != null && !defaultRoles.Contains(value.ToString()))
            {
                return new ValidationResult($"The introduced role is not valid. Valid roles: {string.Join(", ", defaultRoles)}.");
            }

            return ValidationResult.Success;
        }
    }
}
