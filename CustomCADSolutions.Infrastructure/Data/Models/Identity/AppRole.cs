using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static CustomCADSolutions.Infrastructure.Data.DataConstants.RoleConstants;

namespace CustomCADSolutions.Infrastructure.Data.Models.Identity
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }

        public AppRole(string roleName, string? description) : this(roleName)
        {
            Description = description;
        }

        [MaxLength(DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
