using CustomCADs.API.Models.Roles;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.GetRoles
{
    using static StatusCodes;

    public class GetRolesEndpoint(IRoleService service) : EndpointWithoutRequest<RoleGetDTO[]>
    {
        public override void Configure()
        {
            Get("");
            Group<RolesGroup>();
            Description(d => d.WithSummary("Gets All Roles"));
            Options(opt =>
            {
                opt.Produces<RoleModel[]>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IEnumerable<RoleModel> roles = service.GetAll();

            RoleGetDTO[] response = roles
                .Select(r => new RoleGetDTO() 
                { 
                    Name = r.Name, 
                    Description = r.Description 
                }).ToArray();

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
