using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.Register
{
    public class RegisterRequest
    {
        [BindFrom("role")]
        public required string Role { get; set; }
        public required string Username { get; set; } 
        public required string Email { get; set; } 
        public required string Password { get; set; } 
        public required string ConfirmPassword { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
