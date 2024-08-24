namespace CustomCADs.API.Models.Others
{
    public class UserDTO
    {
        public string Username { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
