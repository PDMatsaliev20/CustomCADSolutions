using Microsoft.AspNetCore.Identity;

namespace CustomCADs.Infrastructure.Identity
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }
    }
}
