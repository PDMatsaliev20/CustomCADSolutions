using Microsoft.AspNetCore.Identity;

namespace CustomCADs.Auth
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }
    }
}
