using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.Domain.Identity
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }

        public AppRole(string roleName, string? description) : this(roleName)
        {
            Description = description;
        }

        [MaxLength(RoleConstants.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}
