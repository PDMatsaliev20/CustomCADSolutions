using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.GetRole
{
    public class GetRoleRequest
    {
        [BindFrom("name")]
        public required string Name { get; set; }
    }
}
