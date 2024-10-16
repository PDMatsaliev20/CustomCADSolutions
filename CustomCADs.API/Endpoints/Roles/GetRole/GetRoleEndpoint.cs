using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.UseCases.Roles.Queries.GetByName;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Roles.GetRole;

using static StatusCodes;

public class GetRoleEndpoint(IMediator mediator) : Endpoint<GetRoleRequest, RoleResponseDto>
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
        GetRoleByNameQuery query = new(req.Name);
        RoleModel role = await mediator.Send(query).ConfigureAwait(false);

        RoleResponseDto response = new()
        {
            Name = role.Name,
            Description = role.Description,
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
