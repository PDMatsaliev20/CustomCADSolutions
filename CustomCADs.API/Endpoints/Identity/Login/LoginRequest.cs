using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.UserConstants;

namespace CustomCADs.API.Endpoints.Identity.Login
{
    public class LoginRequest
    {
        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength,
            ErrorMessage = LengthErrorMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = false;
    }
}
