using CustomCADSolutions.App.Resources.Shared;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.RoleConstants;

namespace CustomCADSolutions.App.Models.Roles.Input
{
    public class AddRoleModel
    {
        [Required(ErrorMessageResourceType = typeof(ErrorMessageResources),
           ErrorMessageResourceName = nameof(ErrorMessageResources.Required))]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
           ErrorMessageResourceType = typeof(ErrorMessageResources),
           ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [Display(Name = nameof(DisplayResources.RoleName), ResourceType = typeof(DisplayResources))]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessageResourceType = typeof(ErrorMessageResources),
            ErrorMessageResourceName = nameof(ErrorMessageResources.Length))]
        [Display(Name = nameof(DisplayResources.RoleDesc), ResourceType = typeof(DisplayResources))]
        public string? Description { get; set; }
    }
}
