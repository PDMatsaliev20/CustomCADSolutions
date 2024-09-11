using System.ComponentModel.DataAnnotations;

namespace CustomCADs.API.Models.Users
{
    public class UserPostDTO
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
