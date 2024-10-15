using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.DeleteRole;

public class DeleteRoleRequest
{
    [BindFrom("name")]
    public required string Name { get; set; }
}
