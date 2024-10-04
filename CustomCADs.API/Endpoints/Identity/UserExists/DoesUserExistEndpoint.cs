using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.UserExists
{
    using static StatusCodes;

    public class UserExistsEndpoint(IAppUserManager manager) : Endpoint<UserExistsRequest>
    {
        public override void Configure()
        {
            Get("UserExists/{username}");
            Group<IdentityGroup>();
            Description(d => d
                .WithSummary("Gets info about User Existence Status")
                .Produces<EmptyResponse>(Status200OK));
        }

        public override async Task HandleAsync(UserExistsRequest req, CancellationToken ct)
        {
            AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            bool response = user != null;

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
