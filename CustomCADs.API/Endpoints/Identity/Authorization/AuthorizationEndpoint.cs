using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Queries.GetById;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Identity.Authorization;

using static StatusCodes;

public class AuthorizationEndpoint(IMediator mediator) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("Authorization");
        Group<IdentityGroup>();
        Description(d => d
            .WithSummary("Gets info about User Authorization.")
            .Produces<EmptyResponse>(Status200OK));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        GetUserByIdQuery query = new(User.GetId());
        UserModel model = await mediator.Send(query);

        await SendOkAsync(model.RoleName).ConfigureAwait(false);
    }
}
