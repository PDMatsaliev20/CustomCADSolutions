using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Infrastructure.Data.Models
{
    public class AppUser : IdentityUser<string>
    {
        public AppUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        [StringLength(50)]
        public string? FirstName { get; set; } = null!;

        [StringLength(50)]
        public string? LastName { get; set; } = null!;
    }
}
