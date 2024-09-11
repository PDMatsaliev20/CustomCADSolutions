namespace CustomCADs.API.Models.Users
{
    public class UserGetDTO
    {
        public string Username { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
