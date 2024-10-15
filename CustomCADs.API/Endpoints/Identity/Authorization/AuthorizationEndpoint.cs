using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.Authorization;

using static StatusCodes;

public class AuthorizationEndpoint(IUserService service) : EndpointWithoutRequest
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
        UserModel model = await service.GetByIdAsync(User.GetId());
        await SendOkAsync(model.RoleName).ConfigureAwait(false);
    }
}
