using CustomCADs.App.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.App.Models.Users
{
    public class LoginInputModel
    {
        [TempData]
        public string? ErrorMessage { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [StringLength(UserConstants.NameMaxLength, MinimumLength = UserConstants.NameMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [Display(Name = nameof(DisplayResources.Username), ResourceType = typeof(DisplayResources))]
        public string Username { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [StringLength(UserConstants.PasswordMaxLength, MinimumLength = UserConstants.PasswordMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = nameof(DisplayResources.Remember), ResourceType = typeof(DisplayResources))]
        public bool RememberMe { get; set; }
    }
}
