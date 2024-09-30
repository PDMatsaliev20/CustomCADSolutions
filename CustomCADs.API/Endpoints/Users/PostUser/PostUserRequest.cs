using System.ComponentModel.DataAnnotations;

namespace CustomCADs.API.Endpoints.Users.PostUser
{
    public class PostUserRequest
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Role { get; set; } 

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
