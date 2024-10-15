using CustomCADs.Application.Models.Users;

namespace CustomCADs.Application.Models.Roles;

public class RoleModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<UserModel> Users { get; set; } = [];
}
