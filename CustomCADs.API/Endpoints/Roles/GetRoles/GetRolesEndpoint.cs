using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.GetRoles
{
    using static StatusCodes;

    public class GetRolesEndpoint(IRoleService service) : EndpointWithoutRequest<RoleResponseDto[]>
    {
        public override void Configure()
        {
            Get("");
            Group<RolesGroup>();
            Description(d => d
                .WithSummary("Gets All Roles")
                .Produces<RoleModel[]>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IEnumerable<RoleModel> roles = service.GetAll();

            RoleResponseDto[] response = roles
                .Select(r => new RoleResponseDto() 
                { 
                    Name = r.Name, 
                    Description = r.Description 
                }).ToArray();

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
