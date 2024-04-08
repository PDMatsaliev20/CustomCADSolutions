using CustomCADSolutions.App.Resources.Shared;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.RoleConstants;

namespace CustomCADSolutions.App.Models.Roles
{
    public class AddRoleModel
    {
        [Required(ErrorMessageResourceType = typeof(SharedResources),
           ErrorMessageResourceName = nameof(SharedResources.Required))]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength,
           ErrorMessageResourceType = typeof(SharedResources),
           ErrorMessageResourceName = nameof(SharedResources.Length))]
        [Display(Name = nameof(SharedResources.RoleName), ResourceType = typeof(SharedResources))]
        public string Name { get; set; } = null!;

        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength,
            ErrorMessageResourceType = typeof(SharedResources),
            ErrorMessageResourceName = nameof(SharedResources.Length))]
        [Display(Name = nameof(SharedResources.RoleDesc), ResourceType = typeof(SharedResources))]
        public string? Description { get; set; }
    }
}
