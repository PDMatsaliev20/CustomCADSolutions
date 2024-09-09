using Microsoft.AspNetCore.Identity;

namespace CustomCADs.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public AppUser() : base() { }

        public AppUser(string username) : base(username) { }

        public AppUser(string username, string email) : this(username)
        {
            Email = email;
        }
    }
}
