namespace CustomCADs.API.Dtos
{
    public class UserResponseDto
    {
        public required string Username { get; set; } 
        public required string Email { get; set; } 
        public required string Role { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
