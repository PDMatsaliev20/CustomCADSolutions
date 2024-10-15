using CustomCADs.Application.Models.Roles;

namespace CustomCADs.Application.Models.Users;


public class UserModel
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; } 
    public string? LastName { get; set; } 
    public string RoleName { get; set; } = null!;
    public RoleModel Role { get; set; } = null!;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}
