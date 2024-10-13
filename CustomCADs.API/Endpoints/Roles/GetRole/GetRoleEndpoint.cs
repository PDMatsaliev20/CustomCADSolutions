using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.GetRole
{
    using static StatusCodes;

    public class GetRoleEndpoint(IRoleService service) : Endpoint<GetRoleRequest, RoleResponseDto>
    {
        public override void Configure()
        {
            Get("{name}");
            Group<RolesGroup>();
            Description(d => d
                .WithSummary("Gets a Role by the specified name.")
                .Produces<RoleResponseDto>(Status200OK, "application/json")
                .ProducesProblem(Status404NotFound));
        }

        public override async Task HandleAsync(GetRoleRequest req, CancellationToken ct)
        {
            RoleModel role = await service.GetByNameAsync(req.Name).ConfigureAwait(false);

            RoleResponseDto response = new()
            {
                Name = role.Name,
                Description = role.Description,
            };
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
