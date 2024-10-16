using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Queries.GetByUsername;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Users.GetUser;

using static StatusCodes;

public class GetUserEndpoint(IMediator mediator) : Endpoint<GetUserRequest, UserResponseDto>
{
    public override void Configure()
    {
        Get("{username}");
        Group<UsersGroup>();
        Description(d => d
            .WithSummary("Gets a User by the specified name.")
            .Produces<UserResponseDto>(Status200OK, "application/json")
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        GetUserByUsernameQuery query = new(req.Username);
        UserModel model = await mediator.Send(query);

        UserResponseDto response = model.Adapt<UserResponseDto>();
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
