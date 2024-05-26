using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CustomCADSolutions.Infrastructure.Data.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public AppUser() : base() { }
        public AppUser(string username) : base(username) { }

        [StringLength(50)]
        public string? FirstName { get; set; } = null!;

        [StringLength(50)]
        public string? LastName { get; set; } = null!;
    }
}
