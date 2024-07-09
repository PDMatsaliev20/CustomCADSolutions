using CustomCADs.App.Models.Users;

namespace CustomCADs.App.Models.Roles.View
{
    public class RoleDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}
