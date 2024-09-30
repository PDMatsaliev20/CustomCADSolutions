using FastEndpoints;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;
using static CustomCADs.Domain.DataConstants.UserConstants;

namespace CustomCADs.API.Endpoints.Identity.Register
{
    public class RegisterRequest
    {
        [BindFrom("role")]
        public required string Role { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public required string Username { get; set; } 

        [Required(ErrorMessage = RequiredErrorMessage)]
        [EmailAddress]
        public required string Email { get; set; } 

        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public required string? FirstName { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
            ErrorMessage = LengthErrorMessage)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = RequiredErrorMessage)]
        [StringLength(PasswordMaxLength, MinimumLength = PasswordMinLength,
            ErrorMessage = LengthErrorMessage)]
        [DataType(DataType.Password)]
        public required string Password { get; set; } 

        [Required(ErrorMessage = RequiredErrorMessage)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; } 
    }
}
