using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Queries.GetAll;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Users.GetUsers;

using static StatusCodes;

public class GetUsersEndpoint(IMediator mediator) : Endpoint<GetUsersRequest, GetUsersResponse>
{
    public override void Configure()
    {
        Get("");
        Group<UsersGroup>();
        Description(d => d
            .WithSummary("Gets All Users.")
            .Produces<GetUsersResponse>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(GetUsersRequest req, CancellationToken ct)
    {
        GetAllUsersQuery query = new(
            Username: req.Name,
            Sorting: req.Sorting ?? "",
            Page: req.Page,
            Limit: req.Limit
        );
        UserResult result = await mediator.Send(query, ct).ConfigureAwait(false);
        
        GetUsersResponse response = new()
        {
            Count = result.Count,
            Users = result.Users.Adapt<UserResponseDto[]>(),
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
