using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
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
            Description(d => d.WithSummary("Gets info about User Existence Status"));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status200OK);
            });
        }

        public override async Task HandleAsync(UserExistsRequest req, CancellationToken ct)
        {
            AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            bool response = user != null;

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
