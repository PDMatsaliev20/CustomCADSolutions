using CustomCADs.Application.Models.Roles;
using FastEndpoints;
using Microsoft.AspNetCore.JsonPatch;

namespace CustomCADs.API.Endpoints.Roles.PatchRole
{
    public class PatchRoleRequest
    {
        [BindFrom("name")]
        public required string Name { get; set; }

        [FromBody]
        public required JsonPatchDocument<RoleModel> PatchDocument { get; set; }
    }
}
