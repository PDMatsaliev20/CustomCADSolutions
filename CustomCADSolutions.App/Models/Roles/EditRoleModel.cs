using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.App.Constants.Roles;

namespace CustomCADSolutions.App.Models.Roles
{
    public class EditRoleModel
    {
        public string Id { get; set; } = null!;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
