using static CustomCADSolutions.Infrastructure.Data.DataConstants;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.API.Models.Users
{
    public class UserLoginModel
    {
        [Required]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(UserConstants.PasswordMaxLength, MinimumLength = UserConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = false;
    }
}
