using CustomCADSolutions.App.Models.Users;

namespace CustomCADSolutions.App.Models.Roles
{
    public class RoleDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ICollection<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
}
