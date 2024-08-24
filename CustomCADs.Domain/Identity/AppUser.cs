using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CustomCADs.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public AppUser() : base() { }
        
        public AppUser(string username) : base(username) { }

        public AppUser(string username, string email) : this(username)
        {
            Email = email;
        }
        
        public AppUser(string username, string email, string? firstName, string? lastName) : this(username, email) 
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [StringLength(50)]
        public string? FirstName { get; set; } 

        [StringLength(50)]
        public string? LastName { get; set; }
    }
}
