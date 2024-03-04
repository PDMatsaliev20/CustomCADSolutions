using CustomCADSolutions.App.Resources.Shared;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Constants.DataConstants;

namespace CustomCADSolutions.App.Models.Users
{
    public class RegisterInputModel
    {
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [StringLength(UserConstants.NameMaxLength,
            MinimumLength = UserConstants.NameMinLength,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Length))]
        [Display(Name = "Username", ResourceType = typeof(SharedResources))]
        public string Username { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [EmailAddress(ErrorMessageResourceType = typeof(SharedResources), ErrorMessageResourceName = nameof(SharedResources.InvalidEmail))]
        [Display(Name = "Email", ResourceType = typeof(SharedResources))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [StringLength(UserConstants.PasswordMaxLength,
            MinimumLength = UserConstants.PasswordMinLength,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Length))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(SharedResources))]
        public string Password { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Required))]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm", ResourceType = typeof(SharedResources))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
