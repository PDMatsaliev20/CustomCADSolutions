using CustomCADSolutions.App.Resources.Shared;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Data.DataConstants;

namespace CustomCADSolutions.App.Models.Users
{
    public class RegisterInputModel
    {
        public string? ReturnUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [StringLength(UserConstants.NameMaxLength,
            MinimumLength = UserConstants.NameMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [Display(Name = nameof(DisplayResources.Username), ResourceType = typeof(DisplayResources))]
        public string Username { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [EmailAddress(ErrorMessageResourceType = typeof(ErrorMessageResources), ErrorMessageResourceName = nameof(ErrorMessageResources.InvalidEmail))]
        [Display(Name = nameof(DisplayResources.Email), ResourceType = typeof(DisplayResources))]
        public string Email { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [StringLength(UserConstants.PasswordMaxLength,
            MinimumLength = UserConstants.PasswordMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(DisplayResources.Password), ResourceType = typeof(DisplayResources))]
        public string Password { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [DataType(DataType.Password)]
        [Display(Name = nameof(DisplayResources.Confirm), ResourceType = typeof(DisplayResources))]
        [Compare(nameof(this.Password),
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.PasswordMismatch))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
