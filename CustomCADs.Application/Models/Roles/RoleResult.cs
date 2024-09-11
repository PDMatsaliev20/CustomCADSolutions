namespace CustomCADs.Application.Models.Roles
{
    public class RoleResult
    {
        public int Count { get; set; }
        public ICollection<RoleModel> Roles { get; set; } = [];
    }
}
