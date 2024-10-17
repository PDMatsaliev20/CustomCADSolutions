using CustomCADs.Domain.Roles;

namespace CustomCADs.Domain.Users;

public class User
{
    public required string Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
    public required string RoleName { get; set; }
    public required Role Role { get; set; }
}
