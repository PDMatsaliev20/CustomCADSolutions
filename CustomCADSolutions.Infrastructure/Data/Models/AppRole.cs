using Microsoft.AspNetCore.Identity;
namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }
        public AppRole(string roleName) : base(roleName) { }
    }
}
