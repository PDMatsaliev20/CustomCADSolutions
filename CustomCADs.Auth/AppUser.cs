using Microsoft.AspNetCore.Identity;

namespace CustomCADs.Auth
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
