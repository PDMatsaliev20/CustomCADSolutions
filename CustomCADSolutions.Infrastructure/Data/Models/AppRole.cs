using Microsoft.AspNetCore.Identity;
namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public AppRole(string role) : this()
        {
            Name = role;
        }
    }
}
