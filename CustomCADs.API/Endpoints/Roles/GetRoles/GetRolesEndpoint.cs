using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.UseCases.Roles.Queries.GetAll;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Roles.GetRoles;

using static StatusCodes;

public class GetRolesEndpoint(IMediator mediator) : EndpointWithoutRequest<RoleResponseDto[]>
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
        GetAllRolesQuery query = new();
        IEnumerable<RoleModel> roles = await mediator.Send(query, ct).ConfigureAwait(false);

        var response = roles.Adapt<RoleResponseDto[]>();
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
